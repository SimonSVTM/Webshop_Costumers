namespace Webshop.Model
{
    /// <summary>
    /// Represents a webshop customer.
    /// Maps to the "Customer" table in the database.
    /// </summary>
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Points { get; set; }

        public Customer() { }

        public Customer(string firstName, string lastName, string email,
                        string address, string city, string country, int points)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;
            City = city;
            Country = country;
            Points = points;
        }

        public Customer(int customerID, string firstName, string lastName, string email,
                        string address, string city, string country, int points)
            : this(firstName, lastName, email, address, city, country, points)
        {
            CustomerID = customerID;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} ({Email})";
        }
    }
}
