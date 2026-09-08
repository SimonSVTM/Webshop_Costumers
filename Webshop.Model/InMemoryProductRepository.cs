using System.Collections.Generic;
using System.Linq;

namespace Webshop.Model
{
    /// <summary>
    /// In-memory Product repository, following the same pattern as Category.
    /// </summary>
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new List<Product>();
        private readonly List<Category> _categories = new List<Category>();
        private int _nextId = 1;

        public InMemoryProductRepository()
        {
            _categories.Add(new Category(1, "Electronics"));
            _categories.Add(new Category(2, "Clothing"));
            _categories.Add(new Category(3, "Books"));
            _categories.Add(new Category(4, "Home & Garden"));

            AddProduct(new Product("Laptop", _categories[0], 7999.00m, 10));
            AddProduct(new Product("Headphones", _categories[0], 899.00m, 25));
            AddProduct(new Product("T-Shirt", _categories[1], 199.00m, 40));
            AddProduct(new Product("C# Programming", _categories[2], 349.00m, 15));
            AddProduct(new Product("Coffee Table", _categories[3], 1299.00m, 5));
        }

        public List<Product> GetAll()
        {
            return _products.OrderBy(p => p.ProductName).ToList();
        }

        public Product GetByID(int productID)
        {
            return _products.FirstOrDefault(p => p.ProductID == productID);
        }

        public List<Product> GetByName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
                return GetAll();

            return _products
                .Where(p => p.ProductName != null &&
                            p.ProductName.IndexOf(productName.Trim(), System.StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(p => p.ProductName)
                .ToList();
        }

        public List<Product> GetByCategory(Category category)
        {
            if (category == null)
                return new List<Product>();

            return _products
                .Where(p => p.Category != null && p.Category.CategoryId == category.CategoryId)
                .OrderBy(p => p.ProductName)
                .ToList();
        }

        public void AddProduct(Product product)
        {
            product.ProductID = _nextId++;
            _products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            var existing = GetByID(product.ProductID);
            if (existing != null)
            {
                existing.ProductName = product.ProductName;
                existing.Category = product.Category;
                existing.Price = product.Price;
                existing.StockQuantity = product.StockQuantity;
            }
        }

        public void DeleteProduct(int productID)
        {
            var existing = GetByID(productID);
            if (existing != null)
                _products.Remove(existing);
        }
    }
}
