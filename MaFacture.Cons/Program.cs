using System;
using MaFacture.BLL;
using MaFacture.BO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaFacture.Cons
{
    class Program
    {
        static void Main(string[] args)
        {

            string choice = "y";
            do
            {
                Console.Clear();
                Console.WriteLine("----------------------------Create a product----------------------------");
                Console.Write("Enter reference\t:");
                string reference = Console.ReadLine();
                Console.Write("Enter name\t:");
                string name = Console.ReadLine();
                Console.Write("Enter unit price\t:");
                double price = double.Parse(Console.ReadLine());
                Console.Write("Enter tax\t:");
                float tax = float.Parse(Console.ReadLine());

                Product product = new Product(reference, name, price, tax, null, null);
                ProductBLO productBLO = new ProductBLO(ConfigurationManager.AppSettings["DbFolder"]);
                productBLO.CreateProduct(product);

                IEnumerable<Product> products = productBLO.GetAllProducts();
                foreach (Product p in products)
                {
                    Console.WriteLine($"{p.Reference}\t{p.Name}");
                }

                Console.Write("Create another product ?[y/n]:");
                choice = Console.ReadLine();
            }
            while (choice.ToLower() != "n");
            Console.WriteLine("Program end !");

            Console.ReadKey();
        }
    }
}
