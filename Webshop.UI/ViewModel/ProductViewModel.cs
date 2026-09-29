using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Webshop.Model;

namespace Webshop.UI.ViewModel
{
    /// <summary>
    /// ViewModel for listing, searching, adding, updating, deleting products
    /// and adding a selected product to the shopping cart.
    /// </summary>
    public class ProductViewModel : ViewModelBase
    {
        private readonly IProductRepository _repository;

        public ObservableCollection<Product> Products { get; }
        public ObservableCollection<Product> ShoppingCart { get; }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                if (SetField(ref _selectedProduct, value) && _selectedProduct != null)
                {
                    ProductName = _selectedProduct.ProductName;
                    SelectedCategory = _selectedProduct.Category;
                    Price = _selectedProduct.Price;
                    StockQuantity = _selectedProduct.StockQuantity;
                }
            }
        }

        public ObservableCollection<Category> Categories { get; }

        private string _productName;
        public string ProductName { get => _productName; set => SetField(ref _productName, value); }

        private Category _selectedCategory;
        public Category SelectedCategory { get => _selectedCategory; set => SetField(ref _selectedCategory, value); }

        private decimal _price;
        public decimal Price { get => _price; set => SetField(ref _price, value); }

        private int _stockQuantity;
        public int StockQuantity { get => _stockQuantity; set => SetField(ref _stockQuantity, value); }

        // Søgetekst til LINQ-søgning
        private string _searchText;
        public string SearchText { get => _searchText; set => SetField(ref _searchText, value); }

        private Category _searchCategory;
        public Category SearchCategory { get => _searchCategory; set => SetField(ref _searchCategory, value); }

        public RelayCommand AddProductCommand { get; }
        public RelayCommand AddToCartCommand { get; }
        public RelayCommand UpdateProductCommand { get; }
        public RelayCommand DeleteProductCommand { get; }
        public RelayCommand GetAllCommand { get; }
        public RelayCommand SearchCommand { get; }
        public RelayCommand SortByPriceCommand { get; }
        public RelayCommand ClearCategorySelectionCommand { get; }
        public RelayCommand ClearCommand { get; }

        public ProductViewModel() : this(new ProductRepository(), new CategoryRepository())
        {
        }

        public ProductViewModel(IProductRepository repository, ICategoryRepository crepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            Products = new ObservableCollection<Product>();
            ShoppingCart = new ObservableCollection<Product>();
            Categories = new ObservableCollection<Category>(crepository.GetAll());

            AddProductCommand = new RelayCommand(AddProduct, CanAddOrUpdate);
            AddToCartCommand = new RelayCommand(AddProductToShoppingCart, CanAddToCart);
            UpdateProductCommand = new RelayCommand(UpdateProduct, CanAddOrUpdate);
            DeleteProductCommand = new RelayCommand(DeleteProduct, CanDelete);
            GetAllCommand = new RelayCommand(_ => LoadProducts(_repository.GetAll()));

            // Søg-knap kommandoer
            SearchCommand = new RelayCommand(_ => UpdateProducts());
            SortByPriceCommand = new RelayCommand(_ => SortProductsByPrice());
            ClearCategorySelectionCommand = new RelayCommand(_ => SearchCategory = null);

            ClearCommand = new RelayCommand(ClearInputFields);

            LoadProducts(_repository.GetAll());
        }

        private void LoadProducts(System.Collections.Generic.IEnumerable<Product> products)
        {
            Products.Clear();
            foreach (var product in products)
                Products.Add(product);
        }

        /// <summary>
        /// Filtrerer produkter ud fra SearchText og SearchCategory vha. LINQ
        /// </summary>
        private void UpdateProducts()
        {
            // Hent ud fra kategori først
            var productsByCategory = _repository.GetByCategory(SearchCategory);

            // Hvis der er søgetekst, hente ud fra søgning og tage snitfladen (intersect)
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var productsBySearch = _repository.GetByName(SearchText);
                // Bevarer kun de produkter, der findes i begge lister
                productsByCategory = productsByCategory.Where(p => productsBySearch.Any(s => s.ProductID == p.ProductID)).ToList();
            }

            LoadProducts(productsByCategory);
        }

        /// <summary>
        /// Sorterer den nuværende liste efter pris vha. LINQ OrderBy
        /// </summary>
        private void SortProductsByPrice()
        {
            var sortedList = Products.OrderBy(p => p.Price).ToList();
            LoadProducts(sortedList);
        }

        private bool CanAddOrUpdate(object parameter)
        {
            return !string.IsNullOrWhiteSpace(ProductName)
                && SelectedCategory != null
                && Price >= 0
                && StockQuantity >= 0;
        }

        private bool CanDelete(object parameter) => SelectedProduct != null;

        private bool CanAddToCart(object parameter)
        {
            return SelectedProduct != null && SelectedProduct.StockQuantity > 0;
        }

        private void AddProduct(object parameter)
        {
            try
            {
                var product = new Product(ProductName.Trim(), SelectedCategory, Price, StockQuantity);
                _repository.AddProduct(product);
                LoadProducts(_repository.GetAll());
                ClearInputFields(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not add product: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProductToShoppingCart(object parameter)
        {
            try
            {
                if (!CanAddToCart(null))
                    return;

                ShoppingCart.Add(SelectedProduct);
                SelectedProduct.StockQuantity--;
                _repository.UpdateProduct(SelectedProduct);
                LoadProducts(_repository.GetAll());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not add product to shopping cart: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateProduct(object parameter)
        {
            try
            {
                SelectedProduct.ProductName = ProductName.Trim();
                SelectedProduct.Category = SelectedCategory;
                SelectedProduct.Price = Price;
                SelectedProduct.StockQuantity = StockQuantity;

                _repository.UpdateProduct(SelectedProduct);
                LoadProducts(_repository.GetAll());
                ClearInputFields(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not update product: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteProduct(object parameter)
        {
            try
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete '{SelectedProduct.ProductName}'?",
                    "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                    return;

                _repository.DeleteProduct(SelectedProduct.ProductID);
                LoadProducts(_repository.GetAll());
                ClearInputFields(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not delete product: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearInputFields(object parameter)
        {
            SelectedProduct = null;
            ProductName = string.Empty;
            SelectedCategory = null;
            Price = 0;
            StockQuantity = 0;
            SearchText = string.Empty;
            SearchCategory = null;
        }
    }
}