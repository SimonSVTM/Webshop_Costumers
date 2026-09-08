using System.Collections.Generic;
using System.Linq;

namespace Webshop.Model
{
    /// <summary>
    /// In-memory Customer repository, following the same pattern as Category/Product.
    /// </summary>
    public class InMemoryCustomerRepository : ICustomerRepository
    {
        private readonly List<Customer> _customers = new List<Customer>();
        private int _nextId = 1;

        public InMemoryCustomerRepository()
        {
            AddCustomer(new Customer("Peter", "Jensen", "peter.jensen@example.com", "Vestergade 10", "Odense", "Denmark", 120));
            AddCustomer(new Customer("Anna", "Nielsen", "anna.nielsen@example.com", "Nørregade 22", "Aarhus", "Denmark", 250));
            AddCustomer(new Customer("Thomas", "Hansen", "thomas.hansen@example.com", "Søndergade 7", "Copenhagen", "Denmark", 80));
            AddCustomer(new Customer("Maria", "Andersen", "maria.andersen@example.com", "Algade 15", "Aalborg", "Denmark", 410));
        }

        public List<Customer> GetAll()
        {
            return _customers
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToList();
        }

        public Customer GetByID(int customerID)
        {
            return _customers.FirstOrDefault(c => c.CustomerID == customerID);
        }

        public List<Customer> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return GetAll();

            string search = name.Trim();
            return _customers
                .Where(c => ($"{c.FirstName} {c.LastName}").IndexOf(search, System.StringComparison.OrdinalIgnoreCase) >= 0
                         || (c.FirstName ?? "").IndexOf(search, System.StringComparison.OrdinalIgnoreCase) >= 0
                         || (c.LastName ?? "").IndexOf(search, System.StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToList();
        }

        public void AddCustomer(Customer customer)
        {
            customer.CustomerID = _nextId++;
            _customers.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            var existing = GetByID(customer.CustomerID);
            if (existing != null)
            {
                existing.FirstName = customer.FirstName;
                existing.LastName = customer.LastName;
                existing.Email = customer.Email;
                existing.Address = customer.Address;
                existing.City = customer.City;
                existing.Country = customer.Country;
                existing.Points = customer.Points;
            }
        }

        public void DeleteCustomer(int customerID)
        {
            var existing = GetByID(customerID);
            if (existing != null)
                _customers.Remove(existing);
        }
    }
}
