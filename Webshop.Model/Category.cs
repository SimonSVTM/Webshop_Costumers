namespace Webshop.Model
{
    /// <summary>
    /// Represents a product category in the webshop.
    /// Maps to the "Category" table in the database.
    /// </summary>
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Category()
        {
        }

        public Category(string categoryName)
        {
            CategoryName = categoryName;
        }

        public Category(int categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
        }

        public override string ToString()
        {
            return CategoryName;
        }
    }
}
