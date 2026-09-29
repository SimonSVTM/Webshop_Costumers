// ============================================================
// Webshop – klasser og testdata til LINQ-øvelserne (uge 40)
//
// Kopiér filen ind i et Console App (.NET 6 eller nyere), og brug
// listerne i WebshopData som datakilde for jeres LINQ-forespørgsler.
// Data svarer til INSERT-sætningerne i "Webshop Database.docx" (uge 38),
// så I kan sammenligne jeres LINQ-resultater med resultaterne fra SQL Server.
//
// Eksempel:
//   var telefoner = WebshopData.Products.Where(p => p.CategoryId == 4);
// ============================================================
using System;
using System.Collections.Generic;

namespace Webshop.Linq
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = "";
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public int Points { get; set; }
    }

    // Svarer til tabellen SHOPPINGCART i databasen
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public int PointsUsed { get; set; }
        public string Status { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
    }

    public class OrderItem
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public static class WebshopData
    {
        public static List<Category> Categories { get; } = new()
        {
            new() { CategoryId = 1, CategoryName = "Electronics" },
            new() { CategoryId = 2, CategoryName = "Home Appliances" },
            new() { CategoryId = 3, CategoryName = "Books" },
            new() { CategoryId = 4, CategoryName = "Phones" },
            new() { CategoryId = 5, CategoryName = "Furniture" },
        };

        public static List<Product> Products { get; } = new()
        {
            new() { ProductId = 1, ProductName = "iPhone 13",          CategoryId = 4, Price = 4999.99m, StockQuantity = 3 },
            new() { ProductId = 2, ProductName = "Samsung Galaxy S21", CategoryId = 4, Price = 2699.99m, StockQuantity = 15 },
            new() { ProductId = 3, ProductName = "Nokia Telefon",      CategoryId = 4, Price = 999.99m,  StockQuantity = 2 },
            new() { ProductId = 4, ProductName = "Stationær Telefon",  CategoryId = 4, Price = 349.99m,  StockQuantity = 50 },
            new() { ProductId = 5, ProductName = "Bærbar Computer",    CategoryId = 1, Price = 2200.00m, StockQuantity = 2 },
            new() { ProductId = 6, ProductName = "Tablet",             CategoryId = 1, Price = 1300.00m, StockQuantity = 4 },
        };

        public static List<Customer> Customers { get; } = new()
        {
            new() { CustomerId = 1,  FirstName = "John",    LastName = "Doe",     Email = "john.doe@example.com",       City = "Sønderborg",  Country = "Danmark", Points = 350 },
            new() { CustomerId = 2,  FirstName = "Jane",    LastName = "Smith",   Email = "jane.smith@example.com",     City = "Skanderborg", Country = "Danmark", Points = 450 },
            new() { CustomerId = 3,  FirstName = "Emily",   LastName = "Jones",   Email = "emily.jones@example.com",    City = "København",   Country = "Danmark", Points = 600 },
            new() { CustomerId = 4,  FirstName = "Michael", LastName = "Johnson", Email = "michael.johnson@example.com", City = "Aarhus",     Country = "Danmark", Points = 200 },
            new() { CustomerId = 5,  FirstName = "Sofia",   LastName = "Garcia",  Email = "sofia.garcia@example.com",   City = "Madrid",      Country = "Spanien", Points = 520 },
            new() { CustomerId = 6,  FirstName = "Liam",    LastName = "Brown",   Email = "liam.brown@example.com",     City = "Berlin",      Country = "Tyskland", Points = 100 },
            new() { CustomerId = 7,  FirstName = "Oliver",  LastName = "Taylor",  Email = "oliver.taylor@example.com",  City = "Odense",      Country = "Danmark", Points = 120 },
            new() { CustomerId = 8,  FirstName = "Lucas",   LastName = "Wilson",  Email = "lucas.wilson@example.com",   City = "Sønderborg",  Country = "Danmark", Points = 410 },
            new() { CustomerId = 9,  FirstName = "Emma",    LastName = "Davis",   Email = "emma.davis@example.com",     City = "Aarhus",      Country = "Danmark", Points = 310 },
            new() { CustomerId = 10, FirstName = "Sophia",  LastName = "Müller",  Email = "sophia.mueller@example.com", City = "Hamburg",     Country = "Tyskland", Points = 430 },
        };

        public static List<Order> Orders { get; } = new()
        {
            new() { OrderId = 1, CustomerId = 1, OrderDate = new DateTime(2024, 1, 15),  PointsUsed = 100, Status = "Shipped",   PaymentMethod = "Credit Card" },
            new() { OrderId = 2, CustomerId = 2, OrderDate = new DateTime(2024, 6, 5),   PointsUsed = 50,  Status = "Pending",   PaymentMethod = "PayPal" },
            new() { OrderId = 3, CustomerId = 1, OrderDate = new DateTime(2019, 7, 12),  PointsUsed = 75,  Status = "Delivered", PaymentMethod = "Credit Card" },
            new() { OrderId = 4, CustomerId = 1, OrderDate = new DateTime(2018, 3, 23),  PointsUsed = 20,  Status = "Delivered", PaymentMethod = "Bank Transfer" },
            new() { OrderId = 5, CustomerId = 2, OrderDate = new DateTime(2023, 10, 25), PointsUsed = 0,   Status = "Shipped",   PaymentMethod = "PayPal" },
            new() { OrderId = 6, CustomerId = 3, OrderDate = new DateTime(2024, 5, 17),  PointsUsed = 120, Status = "Pending",   PaymentMethod = "Credit Card" },
            new() { OrderId = 7, CustomerId = 4, OrderDate = new DateTime(2017, 9, 30),  PointsUsed = 300, Status = "Canceled",  PaymentMethod = "Credit Card" },
            new() { OrderId = 8, CustomerId = 3, OrderDate = new DateTime(2016, 11, 20), PointsUsed = 50,  Status = "Canceled",  PaymentMethod = "PayPal" },
            new() { OrderId = 9, CustomerId = 5, OrderDate = new DateTime(2024, 2, 10),  PointsUsed = 100, Status = "Pending",   PaymentMethod = "Bank Transfer" },
        };

        public static List<OrderItem> OrderItems { get; } = new()
        {
            new() { OrderId = 1, ProductId = 1, Quantity = 1, Price = 5799.99m },  // iPhone 13
            new() { OrderId = 1, ProductId = 5, Quantity = 1, Price = 2500.00m },  // Bærbar Computer
            new() { OrderId = 2, ProductId = 2, Quantity = 2, Price = 2999.99m },  // Samsung Galaxy S21
            new() { OrderId = 3, ProductId = 3, Quantity = 1, Price = 1199.99m },  // Nokia Telefon
            new() { OrderId = 3, ProductId = 4, Quantity = 3, Price = 399.99m },   // Stationær Telefon
            new() { OrderId = 4, ProductId = 6, Quantity = 1, Price = 1350.00m },  // Tablet
            new() { OrderId = 4, ProductId = 1, Quantity = 1, Price = 5599.99m },  // iPhone 13
            new() { OrderId = 5, ProductId = 5, Quantity = 1, Price = 2300.00m },  // Bærbar Computer
            new() { OrderId = 5, ProductId = 4, Quantity = 2, Price = 389.99m },   // Stationær Telefon
            new() { OrderId = 5, ProductId = 2, Quantity = 1, Price = 2899.99m },  // Samsung Galaxy S21
        };
    }
}







