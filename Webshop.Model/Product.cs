namespace Webshop.Model
{
    /// <summary>
    /// Represents a product in the webshop.
    /// </summary>
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public Product()
        {
        }

        public Product(string productName, Category category, decimal price, int stockQuantity)
        {
            ProductName = productName;
            Category = category;
            Price = price;
            StockQuantity = stockQuantity;
        }

        public Product(int productID, string productName, Category category, decimal price, int stockQuantity)
        {
            ProductID = productID;
            ProductName = productName;
            Category = category;
            Price = price;
            StockQuantity = stockQuantity;
        }

        public override string ToString()
        {
            return ProductName;
        }
    }
}
