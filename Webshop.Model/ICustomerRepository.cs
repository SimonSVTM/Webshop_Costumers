using System.Collections.Generic;

namespace Webshop.Model
{
    /// <summary>
    /// Defines the data access operations available for Customer.
    /// </summary>
    public interface ICustomerRepository
    {
        List<Customer> GetAll();
        Customer GetByID(int customerID);
        List<Customer> GetByName(string name);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int customerID);
    }
}
