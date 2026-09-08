using System.Collections.ObjectModel;
using System.Windows;
using Webshop.Model;

namespace Webshop.UI.ViewModel
{
    /// <summary>
    /// ViewModel for managing categories: listing, adding, updating and deleting.
    /// </summary>
    public class CategoryViewModel : ViewModelBase
    {
        private readonly ICategoryRepository _repository;

        public ObservableCollection<Category> Categories { get; }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetField(ref _selectedCategory, value))
                {
                    // Fill the input fields with the selected category so it can be edited
                    if (_selectedCategory != null)
                    {
                        Name = _selectedCategory.Name;
                    }
                }
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        public RelayCommand AddCommand { get; }
        public RelayCommand UpdateCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ClearCommand { get; }

        public CategoryViewModel() : this(new CategoryRepository())
        {
        }

        // Constructor overload that accepts a repository, useful for unit testing with a fake/mock repository
        public CategoryViewModel(ICategoryRepository repository)
        {
            _repository = repository;
            Categories = new ObservableCollection<Category>();

            AddCommand = new RelayCommand(AddCategory, CanAdd);
            UpdateCommand = new RelayCommand(UpdateCategory, CanUpdate);
            DeleteCommand = new RelayCommand(DeleteCategory, CanUpdate);
            ClearCommand = new RelayCommand(ClearInputFields);

            LoadCategories();
        }

        private void LoadCategories()
        {
            Categories.Clear();
            foreach (var category in _repository.GetAll())
            {
                Categories.Add(category);
            }
        }

        private bool CanAdd(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private bool CanUpdate(object parameter)
        {
            return SelectedCategory != null && !string.IsNullOrWhiteSpace(Name);
        }

        private void AddCategory(object parameter)
        {
            try
            {
                var newCategory = new Category(Name.Trim());
                _repository.Add(newCategory);
                LoadCategories();
                ClearInputFields(null);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Could not add category: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCategory(object parameter)
        {
            try
            {
                SelectedCategory.Name = Name.Trim();
                _repository.Update(SelectedCategory);
                LoadCategories();
                ClearInputFields(null);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Could not update category: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteCategory(object parameter)
        {
            try
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete '{SelectedCategory.Name}'?",
                    "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                    return;

                _repository.Delete(SelectedCategory.CategoryId);
                LoadCategories();
                ClearInputFields(null);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Could not delete category: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearInputFields(object parameter)
        {
            SelectedCategory = null;
            Name = string.Empty;
        }
    }
}
