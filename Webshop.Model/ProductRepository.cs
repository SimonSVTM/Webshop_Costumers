using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace Webshop.Model
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository()
        {
            _connectionString = @"Server=localhost;Database=SkoleDB;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        // Henter alle produkter fra databasen via ADO.NET (bevarer databaseforbindelsen)
        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();
            string sql = @"SELECT p.ProductID, p.ProductName, p.Price, p.StockQuantity,
                                  c.CategoryId, c.CategoryName AS CategoryName
                           FROM dbo.Product p
                           INNER JOIN dbo.Category c ON p.CategoryId = c.CategoryId;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(MapProduct(reader));
                    }
                }
            }
            return products;
        }

        // --- LINQ-OMSKREVNE METODER ---

        // GetByID omskrevet til LINQ FirstOrDefault
        public Product? GetByID(int productID)
        {
            return GetAll().FirstOrDefault(p => p.ProductID == productID);
        }

        // GetByName / Search omskrevet til LINQ Where + Contains
        public List<Product> GetByName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                return GetAll();

            return GetAll()
                .Where(p => p.ProductName != null && p.ProductName.Contains(productName, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // GetByCategory omskrevet til LINQ Where
        public List<Product> GetByCategory(Category category)
        {
            if (category == null) return GetAll();

            return GetAll()
                .Where(p => p.Category != null && p.Category.CategoryId == category.CategoryId)
                .ToList();
        }

        // Overload der tager int categoryId direkte
        public List<Product> GetByCategory(int categoryId)
        {
            return GetAll()
                .Where(p => p.Category != null && p.Category.CategoryId == categoryId)
                .ToList();
        }

        // --- CRUD METODER ---

        public void AddProduct(Product product)
        {
            string sql = @"INSERT INTO dbo.Product (ProductName, CategoryId, Price, StockQuantity)
                           VALUES (@ProductName, @CategoryId, @Price, @StockQuantity);
                           SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ProductName", product.ProductName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CategoryId", product.Category != null ? product.Category.CategoryId : DBNull.Value);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);

                connection.Open();

                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    product.ProductID = Convert.ToInt32(result);
                }
            }
        }

        public void UpdateProduct(Product product)
        {
            string sql = @"UPDATE dbo.Product
                           SET ProductName = @ProductName,
                               CategoryId = @CategoryId,
                               Price = @Price,
                               StockQuantity = @StockQuantity
                           WHERE ProductID = @ProductID;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ProductID", product.ProductID);
                command.Parameters.AddWithValue("@ProductName", product.ProductName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CategoryId", product.Category != null ? product.Category.CategoryId : DBNull.Value);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(int productID)
        {
            string sql = @"DELETE FROM dbo.Product 
                           WHERE ProductID = @ProductID;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ProductID", productID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private Product MapProduct(SqlDataReader reader)
        {
            Category category = new Category
            {
                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                CategoryName = reader["CategoryName"]?.ToString() ?? ""
            };

            return new Product
            {
                ProductID = Convert.ToInt32(reader["ProductID"]),
                ProductName = reader["ProductName"]?.ToString() ?? "",
                Price = Convert.ToDecimal(reader["Price"]),
                StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                Category = category
            };
        }
    }
}