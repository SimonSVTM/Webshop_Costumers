namespace Webshop.Model
{
    /// <summary>
    /// Represents a product category in the webshop.
    /// Maps to the "Category" table in the database.
    /// </summary>
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public Category()
        {
        }

        public Category(string name)
        {
            Name = name;
        }

        public Category(int categoryId, string name)
        {
            CategoryId = categoryId;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
