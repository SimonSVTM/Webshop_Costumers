using System;
using System.Collections.Generic;
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

        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();
            string sql = @"SELECT p.ProductID, p.ProductName, p.Price, p.StockQuantity,
                                  c.CategoryId, c.Name AS CategoryName
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

        public Product GetByID(int productID)
        {
            string sql = @"SELECT p.ProductID, p.ProductName, p.Price, p.StockQuantity,
                                  c.CategoryId, c.Name AS CategoryName
                           FROM dbo.Product p
                           INNER JOIN dbo.Category c ON p.CategoryId = c.CategoryId
                           WHERE p.ProductID = @ProductID;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ProductID", productID);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapProduct(reader);
                    }
                }
            }
            return null;
        }

        public List<Product> GetByName(string productName)
        {
            List<Product> products = new List<Product>();
            string sql = @"SELECT p.ProductID, p.ProductName, p.Price, p.StockQuantity,
                                  c.CategoryId, c.Name AS CategoryName
                           FROM dbo.Product p
                           INNER JOIN dbo.Category c ON p.CategoryId = c.CategoryId
                           WHERE p.ProductName LIKE @ProductName;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ProductName", "%" + productName + "%");
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

        public List<Product> GetByCategory(Category category)
        {
            List<Product> products = new List<Product>();
            if (category == null) return products;

            string sql = @"SELECT p.ProductID, p.ProductName, p.Price, p.StockQuantity,
                                  c.CategoryId, c.Name AS CategoryName
                           FROM dbo.Product p
                           INNER JOIN dbo.Category c ON p.CategoryId = c.CategoryId
                           WHERE p.CategoryId = @CategoryId;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CategoryId", category.CategoryId);
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

        private Product MapProduct(SqlDataReader reader)
        {
            Category category = new Category
            {
                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                Name = reader["CategoryName"].ToString()
            };

            return new Product
            {
                ProductID = Convert.ToInt32(reader["ProductID"]),
                ProductName = reader["ProductName"].ToString(),
                Price = Convert.ToDecimal(reader["Price"]),
                StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                Category = category
            };
        }
    }
}