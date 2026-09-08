using System.Collections.Generic;
using System.Linq;

namespace Webshop.Model
{
    /// <summary>
    /// In-memory implementation of ICategoryRepository.
    /// Stores categories in a simple List instead of a real database.
    ///
    /// This is used because this exercise focuses on DESIGNING the database
    /// (domain model -> relation schema -> database model), not on implementing
    /// database access yet. The repository interface (ICategoryRepository) is
    /// still the same as a "real" database-backed repository would use, so this
    /// class can later be swapped for e.g. a CategoryRepository that talks to
    /// SQL Server, without changing the ViewModel or View at all.
    /// </summary>
    public class InMemoryCategoryRepository : ICategoryRepository
    {
        private readonly List<Category> _categories = new List<Category>();
        private int _nextId = 1;

        public InMemoryCategoryRepository()
        {
            // Seed with a bit of sample data so the app is not empty on startup
            Add(new Category("Electronics"));
            Add(new Category("Clothing"));
            Add(new Category("Books"));
            Add(new Category("Home & Garden"));
        }

        public List<Category> GetAll()
        {
            // Return a copy so callers cannot mutate the internal list directly
            return _categories.OrderBy(c => c.Name).ToList();
        }

        public Category GetById(int categoryId)
        {
            return _categories.FirstOrDefault(c => c.CategoryId == categoryId);
        }

        public void Add(Category category)
        {
            category.CategoryId = _nextId++;
            _categories.Add(category);
        }

        public void Update(Category category)
        {
            var existing = GetById(category.CategoryId);
            if (existing != null)
            {
                existing.Name = category.Name;
            }
        }

        public void Delete(int categoryId)
        {
            var existing = GetById(categoryId);
            if (existing != null)
            {
                _categories.Remove(existing);
            }
        }
    }
}
