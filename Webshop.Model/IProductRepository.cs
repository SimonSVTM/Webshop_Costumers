using System.Collections.Generic;

namespace Webshop.Model
{
    /// <summary>
    /// Defines the data access operations available for Product.
    /// </summary>
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product GetByID(int productID);
        List<Product> GetByName(string productName);
        List<Product> GetByCategory(Category category);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int productID);
    }
}
