using System.Collections.Generic;

namespace Webshop.Model
{
    /// <summary>
    /// Defines the data access operations available for the Category entity.
    /// </summary>
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category GetById(int categoryId);
        void Add(Category category);
        void Update(Category category);
        void Delete(int categoryId);
    }
}
