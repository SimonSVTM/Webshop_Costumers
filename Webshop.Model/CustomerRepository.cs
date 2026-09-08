using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Webshop.Model
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository()
        {
            _connectionString = @"Server=localhost;Database=SkoleDB;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        public void AddCustomer(Customer customer)
        {
            string sql = @"INSERT INTO dbo.Customer (FirstName, LastName, Email, Address, City, Country, Points)
                           VALUES (@FirstName, @LastName, @Email, @Address, @City, @Country, @Points);
                           SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@FirstName", customer.FirstName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LastName", customer.LastName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", customer.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@City", customer.City ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Country", customer.Country ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Points", customer.Points);

                connection.Open();

                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    customer.CustomerID = Convert.ToInt32(result);
                }
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            string sql = @"UPDATE dbo.Customer
                           SET FirstName = @FirstName,
                               LastName = @LastName,
                               Email = @Email,
                               Address = @Address,
                               City = @City,
                               Country = @Country,
                               Points = @Points
                           WHERE CustomerID = @CustomerID;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                command.Parameters.AddWithValue("@FirstName", customer.FirstName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LastName", customer.LastName ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", customer.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@City", customer.City ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Country", customer.Country ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Points", customer.Points);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteCustomer(int customerID)
        {
            string sql = @"DELETE FROM dbo.Customer 
                           WHERE CustomerID = @CustomerID;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CustomerID", customerID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();
            string sql = "SELECT CustomerID, FirstName, LastName, Email, Address, City, Country, Points FROM dbo.Customer;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(MapCustomer(reader));
                    }
                }
            }
            return customers;
        }

        public Customer GetByID(int customerID)
        {
            string sql = "SELECT CustomerID, FirstName, LastName, Email, Address, City, Country, Points FROM dbo.Customer WHERE CustomerID = @CustomerID;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CustomerID", customerID);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapCustomer(reader);
                    }
                }
            }
            return null;
        }

        public List<Customer> GetByName(string name)
        {
            List<Customer> customers = new List<Customer>();
            string sql = @"SELECT CustomerID, FirstName, LastName, Email, Address, City, Country, Points 
                           FROM dbo.Customer 
                           WHERE FirstName LIKE @Name OR LastName LIKE @Name;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Name", "%" + name + "%");
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(MapCustomer(reader));
                    }
                }
            }
            return customers;
        }

        private Customer MapCustomer(SqlDataReader reader)
        {
            return new Customer
            {
                CustomerID = Convert.ToInt32(reader["CustomerID"]),
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                Email = reader["Email"].ToString(),
                Address = reader["Address"].ToString(),
                City = reader["City"].ToString(),
                Country = reader["Country"].ToString(),
                Points = Convert.ToInt32(reader["Points"])
            };
        }
    }
}