using Webshop.Model;

namespace Webshop.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetField(ref _currentView, value);
        }

        public CategoryViewModel CategoryVM { get; }
        public ProductViewModel ProductVM { get; }
        public CustomerViewModel CustomerVM { get; }

        public RelayCommand ShowCategoryViewCommand { get; }
        public RelayCommand ShowProductViewCommand { get; }
        public RelayCommand ShowCustomerViewCommand { get; }

        // Parameterless constructor for XAML design-time preview support
        public MainViewModel() : this(new CategoryRepository(), new ProductRepository(), new CustomerRepository())
        {
        }

        // Main constructor called by App.xaml.cs
        public MainViewModel(ICategoryRepository categoryRepo, IProductRepository productRepo, ICustomerRepository customerRepo)
        {
            CategoryVM = new CategoryViewModel(categoryRepo);
            ProductVM = new ProductViewModel(productRepo, categoryRepo);
            CustomerVM = new CustomerViewModel(customerRepo);

            // Set initial view
            CurrentView = CategoryVM;

            // Navigation commands
            ShowCategoryViewCommand = new RelayCommand(p => CurrentView = CategoryVM);
            ShowProductViewCommand = new RelayCommand(p => CurrentView = ProductVM);
            ShowCustomerViewCommand = new RelayCommand(p => CurrentView = CustomerVM);
        }
    }
}