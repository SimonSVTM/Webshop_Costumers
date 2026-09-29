using System.Globalization;
using Webshop.Linq;


namespace Webshop.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("Øvelse 2");
            //Øvelse 2
            List<int> tal = new() { 12, 7, 3, 25, 8, 19, 4, 25, 30, 1 };

            List<string> byer = new() { "Odense", "Aarhus", "Aalborg", "København", "Odense", "Esbjerg", "Randers", "Roskilde" };

            var resultat1 = from t in tal where t < 10 select t; // forespørgselssyntaks

            var resultat2 = tal.Where(t => t < 10); // metodesyntaks

            Console.WriteLine(string.Join(", ", resultat2)); // 7, 3, 8, 4, 1
            Console.WriteLine(string.Join(", ", tal));
            Console.WriteLine(string.Join(", ", tal.Where(t => t % 2 == 0)));
            Console.WriteLine(string.Join(", ", tal.Where(t => t > 10).OrderByDescending(t => t)));
            Console.WriteLine(string.Join(", ", tal.Select(t => Math.Pow(t, 2))));
            Console.WriteLine(string.Join(", ", byer.Where(p => p.StartsWith("R"))));
            Console.WriteLine(string.Join(", ", tal.Count(), tal.Count(), tal.Sum(), tal.Average(), tal.Min(), tal.Max()));
            Console.WriteLine(string.Join(", ", tal.Where(t => t > 20).FirstOrDefault()));
            Console.WriteLine(string.Join(", ", byer.Distinct().OrderBy(b=> b)));

            var danishComparer = StringComparer.Create(new CultureInfo("da-DK"), ignoreCase: true);

            var result = byer.Distinct()
                .OrderBy(b => b, danishComparer);

            Console.WriteLine(string.Join(", ", result));

            var store = tal.Where(t => t > 20);
            tal.Add(99);
            Console.WriteLine(string.Join(", ", tal));
            var nu = store.ToList();

           

            Console.WriteLine();
            Console.WriteLine("Øvelse 3.1");
            //Øvelse 3.1
            var dyre = WebshopData.Products.Where(p => p.Price > 2000);

            foreach (var p in dyre)
                Console.WriteLine($"{p.ProductName}: {p.Price} kr.");
            Console.WriteLine();
            var pc4 = WebshopData.Products.Where(p => p.CategoryId == 4);

            foreach (var p in pc4)
                Console.WriteLine($"{p.ProductName}: {p.Price} kr.");

            Console.WriteLine();

            Console.WriteLine(string.Join(", ", WebshopData.Customers.Select(c => c.City).Distinct()));

            var dyre2 = WebshopData.Products.OrderByDescending(p => p.Price).Take(3);

            foreach (var p in dyre2)
                Console.WriteLine($"{p.ProductName}: {p.Price} kr.");
            Console.WriteLine();

            Console.WriteLine(string.Join(", ", WebshopData.Customers.Where(c => c.City == "Aarhus" && c.Points > 300).Select(c => $"{c.FirstName} {c.LastName}")));

            Console.WriteLine(string.Join(", ", WebshopData.Products.Where(p => p.ProductName.Contains("Telefon")).Select(p => $"{p.ProductName} {p.Price}")));
            Console.WriteLine();
            Console.WriteLine("Øvelse 3.2");
            //Øvelse 3.2
            Console.WriteLine(string.Join(", ", WebshopData.Products.Average(p => p.Price)));
            Console.WriteLine(string.Join(", ", WebshopData.Products.Max(p => p.Price)));
            Console.WriteLine(string.Join(", ", WebshopData.Products.Min(p => p.Price)));
            Console.WriteLine(string.Join(", ", WebshopData.Customers.Count()));
            Console.WriteLine(string.Join(", ", WebshopData.Products.GroupBy(p => p.CategoryId).Select(g => new { CategoryId = g.Key, Count = g.Count()})));
            Console.WriteLine();
            Console.WriteLine("Øvelse 3.3");
            //Øvelse 3.3
            var produkter = from p in WebshopData.Products

                            join c in WebshopData.Categories

                            on p.CategoryId equals c.CategoryId

                            select new { p.ProductName, c.CategoryName };

            foreach (var x in produkter)

                Console.WriteLine($"{x.ProductName}: {x.CategoryName}");


            var orders = from c in WebshopData.Customers
                         join o in WebshopData.Orders
                         on c.CustomerId equals o.CustomerId
                         select new { c.FirstName, o.OrderId, o.OrderDate };
            foreach (var x in orders)

                Console.WriteLine($"{x.FirstName}: {x.OrderId}, {x.OrderDate}");
            Console.WriteLine();
            var orders2 = from c in WebshopData.Customers
                         join o in WebshopData.Orders
                         on c.CustomerId equals o.CustomerId
                         where o.Status == "Pending"
                         select new { c.FirstName, o.OrderId, o.OrderDate };
            foreach (var x in orders2)

                Console.WriteLine($"{x.FirstName}: {x.OrderId}, {x.OrderDate}");
            Console.WriteLine();

            //Øvelse 3.4
            Console.WriteLine("Øvelse 3.4");
            var customerLevels = WebshopData.Customers
            .Select(c => new
            {
                FullName = $"{c.FirstName} {c.LastName}",
                Points = c.Points,
                Level = c.Points > 500 ? "GOLD" :
                        c.Points >= 301 ? "SILVER" : "BRONZE"
            });

            foreach (var c in customerLevels)
            {
                Console.WriteLine($"{c.FullName} - {c.Points} point - Level: {c.Level}");
            }
            Console.WriteLine();
            Console.WriteLine(string.Join(", ", WebshopData.Products.GroupBy(p => p.CategoryId).Where(g => g.Average(p => p.Price) > 2100).Select(g => new
            {
                CategoryId = g.Key,
                AvgPrice = g.Average(p => p.Price)
            })));
            Console.WriteLine();
            var kundernesOrdrer = from c in WebshopData.Customers
                                  join o in WebshopData.Orders
                                  on c.CustomerId equals o.CustomerId into customerOrdersGroup
                                  select new
                                  {
                                      CustomerName = $"{c.FirstName} {c.LastName}",
                                      City = c.City,
                                      Orders = customerOrdersGroup // Dette er listen af ordrer for kunden
                                  };

            foreach (var kunde in kundernesOrdrer)
            {
                Console.WriteLine($"Kunde: {kunde.CustomerName} ({kunde.City})");

                if (kunde.Orders.Any())
                    foreach (var ordre in kunde.Orders)
                        Console.WriteLine($"  - Ordre ID: {ordre.OrderId}, Dato: {ordre.OrderDate:yyyy-MM-dd}, Status: {ordre.Status}");
              
                else
                    Console.WriteLine("  - Ingen ordrer registreret");
                

                Console.WriteLine(); // Blank linje for overskuelighed
            }

        }
    }
}
