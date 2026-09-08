using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Webshop.Model
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository()
        {
            _connectionString = @"Server=localhost;Database=SkoleDB;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        public void Add(Category category)
        {
            string sql = @"INSERT INTO dbo.Category (Name)
                           VALUES (@Name);
                           SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Name", category.Name ?? (object)DBNull.Value);

                connection.Open();

                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    category.CategoryId = Convert.ToInt32(result);
                }
            }
        }

        public void Update(Category category)
        {
            string sql = @"UPDATE dbo.Category
                           SET Name = @Name
                           WHERE CategoryId = @CategoryId;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CategoryId", category.CategoryId);
                command.Parameters.AddWithValue("@Name", category.Name ?? (object)DBNull.Value);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int categoryId)
        {
            string sql = @"DELETE FROM dbo.Category 
                           WHERE CategoryId = @CategoryId;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CategoryId", categoryId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Category> GetAll()
        {
            List<Category> categories = new List<Category>();
            string sql = "SELECT CategoryId, Name FROM dbo.Category;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(MapCategory(reader));
                    }
                }
            }
            return categories;
        }

        public Category GetById(int categoryId)
        {
            string sql = "SELECT CategoryId, Name FROM dbo.Category WHERE CategoryId = @CategoryId;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapCategory(reader);
                    }
                }
            }
            return null;
        }

        private Category MapCategory(SqlDataReader reader)
        {
            return new Category
            {
                CategoryId = Convert.ToInt32(reader["CategoryId"]),
                Name = reader["Name"].ToString()
            };
        }
    }
}