using System;
using Teste.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Teste
{
    class Program
    {

        static void Print<T>(string message, IEnumerable<T> collection)
        {
            Console.WriteLine(message);
            foreach (T obj in collection)
            {
                Console.WriteLine(obj);
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            Category c1 = new Category() { Id = 1, Name = "Tools", Tier = 2 };
            Category c2 = new Category() { Id = 2, Name = "Computers", Tier = 1 };
            Category c3 = new Category() { Id = 3, Name = "Eletronics", Tier = 1 };

            List<Product> products = new List<Product>()
            {
                new Product() { Id = 1, Name = "Computer", Price = 1110.0, Category = c2},
                new Product() { Id = 2, Name = "Hammer", Price = 90.0, Category = c1},
                new Product() { Id = 3, Name = "TV", Price = 1700.0, Category = c3},
                new Product() { Id = 4, Name = "Notebook", Price = 1300.0, Category = c2},
                new Product() { Id = 5, Name = "Saw", Price = 80.0, Category = c1 }
            };

            //Produtos do Tier == 1 e preco menor que 900.00;
            var r1 = products.Where(p => p.Category.Tier == 1 && p.Price < 900.0);

            //Sintaxe Linq-Sql --Produtos do Tier == 1 e preco menor que 900.00;
            var r1sql = from p in products
                        where p.Category.Tier == 1 &&
                        p.Price < 900
                        select p;
            Print("TIER 1 AND PRICE < 900", r1sql);
            
            // Nomes de Produtos da categoria Tools;
            var r2 = products.Where(p => p.Category.Name == "Tools").Select(p => p.Name);

            //Sintaxe Linq-Sql --Nomes de Produtos da categoria Tools;
            var r2sql = from p in products
                        where p.Category.Name == "Tools"
                        select p.Name;
            Print("NAMES OF PRODUCTS FROM TOOLS", r2sql);

            //Criacao de um objeto anonimo;
            var r3 = products.Where(p => p.Name[0] == 'C').Select(p => new { p.Name, p.Price, CategoryName = p.Category.Name });

            //Sintaxe Linq-Sql --Criacao de um objeto anonimo que projeta O Nome, Preco e Nome da Categoria com alias pois já existe um atributo com o nome Name;
            var r3sql = from p in products
                        where p.Name[0] == 'C'
                        select new {
                            p.Name,
                            p.Price,
                            CategoryName = p.Category.Name
                        };
            Print("OBJECTS STARTED WITH 'C' AND ANONYMOUS OBJECT", r3sql);

            //Produtos da categoria 
            var r4 = products.Where(p => p.Category.Tier == 1).OrderBy(p => p.Price).ThenBy(p => p.Name);

            //Sintaxe Linq-Sql --Produtos da categoria ordenando por Preco e depois por nome (#note que o preco vai depois do nome;)
            var r4sql = from p in products
                        where p.Category.Tier == 1
                        orderby p.Name
                        orderby p.Price
                        select p;
            Print("TIER 1 ORDER BY PRICE THEN BY NAME", r4sql);

            //Pula 2 e pega os 4 proximos
            var r5 = r4.Skip(2).Take(4);

            ////Sintaxe Linq-Sql --Pula 2 e pega os 4 proximos
            var r5sql = (from p in r4 select p)
                .Skip(2)
                .Take(4);
            Print("TIER 1 ORDER BY PRICE THEN BY NAME, SKIP 2 AND TAKE 4", r5sql);

            //Pega o primeiro objeto dos produtos
            var r6 = products.FirstOrDefault();
            Console.WriteLine("First product" + r6);

            //Pegando primeiro objeto com valor maior que 3000; (não existe objeto na lista que o preco seja maior que 3.000)
            var r7 = products.Where(p => p.Price > 3000.0).FirstOrDefault();
            Console.WriteLine("First or Default product" + r7);

            //Só usar quando retorna apenas um elemento
            var r8 = products.Where(p => p.Id == 3).SingleOrDefault();
            Console.WriteLine("Single or Default" + r8);

            //Pegando o maximo valor, baseado no Preco
            var r9 = products.Max(p => p.Price);
            Console.WriteLine("Max Price Value : " + r9);

            //Pegando o minimo valor, baseado no Preco
            var r10 = products.Min(p => p.Price);
            Console.WriteLine("Min Price Value : " + r10);

            //Somando os precos dos elementos da Categoria 1;
            var r11 = products.Where(p => p.Category.Id == 1).Sum(p => p.Price);
            Console.WriteLine("Category 1 sum Prices : " + r11);

            //Pegando a média de precos dos elementos da Categoria 1;
            var r12 = products.Where(p => p.Category.Id == 1).Average(p => p.Price);
            Console.WriteLine("Category 1 Average Prices : " + r12);

            //Pegando a média de precos dos elementos da Categoria 5(que não existe) e tratando com o
            //DefaultIfEmpty para caso de retronar vazio, ele está retornando por default: 0.0;
            var r13 = products.Where(p => p.Category.Id == 5).Select(p => p.Price).DefaultIfEmpty(0.0).Average();
            Console.WriteLine("Category 5 Average(Default If Empty) Prices : " + r13);

            //Aggregate(Reduce ou MapReduce) Soma Personalizada;
            var r14 = products.Where(p => p.Category.Id == 1).Select(p => p.Price).Aggregate(0.0, (x, y) => x + y);
            Console.WriteLine("Category 1 Aggreagate Sum : " + r14);

            Console.WriteLine();

            //Imprimindo todos os produtos por Categoria;
            var r15 = products.GroupBy(p => p.Category);
            foreach (IGrouping<Category, Product> group in r15)
            {
                Console.WriteLine("Category : " + group.Key.Name);
                foreach (Product p in group)
                {
                    Console.WriteLine(p);
                }
                Console.WriteLine();
            }

            //Sintaxe Linq-Sql --Imprimindo todos os produtos por Categoria;
            var r15sql = from p in products
                         group p by p.Category;
            foreach (IGrouping<Category, Product> group in r15)
            {
                Console.WriteLine("Category : " + group.Key.Name);
                foreach (Product p in group)
                {
                    Console.WriteLine(p);
                }
                Console.WriteLine();
            }
        }
    }
}
