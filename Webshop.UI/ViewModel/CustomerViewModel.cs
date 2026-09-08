using System;
using System.Collections.ObjectModel;
using System.Windows;
using Webshop.Model;

namespace Webshop.UI.ViewModel
{
    /// <summary>
    /// ViewModel for listing, searching, adding, updating and deleting customers.
    /// </summary>
    public class CustomerViewModel : ViewModelBase
    {
        private readonly ICustomerRepository _repository;

        public ObservableCollection<Customer> Customers { get; }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                if (SetField(ref _selectedCustomer, value) && _selectedCustomer != null)
                {
                    FirstName = _selectedCustomer.FirstName;
                    LastName = _selectedCustomer.LastName;
                    Email = _selectedCustomer.Email;
                    Address = _selectedCustomer.Address;
                    City = _selectedCustomer.City;
                    Country = _selectedCustomer.Country;
                    Points = _selectedCustomer.Points;
                }
            }
        }

        private string _firstName;
        public string FirstName { get => _firstName; set => SetField(ref _firstName, value); }
        private string _lastName;
        public string LastName { get => _lastName; set => SetField(ref _lastName, value); }
        private string _email;
        public string Email { get => _email; set => SetField(ref _email, value); }
        private string _address;
        public string Address { get => _address; set => SetField(ref _address, value); }
        private string _city;
        public string City { get => _city; set => SetField(ref _city, value); }
        private string _country;
        public string Country { get => _country; set => SetField(ref _country, value); }
        private int _points;
        public int Points { get => _points; set => SetField(ref _points, value); }

        private string _searchName;
        public string SearchName { get => _searchName; set => SetField(ref _searchName, value); }

        private string _searchID;
        public string SearchID { get => _searchID; set => SetField(ref _searchID, value); }

        public RelayCommand AddCustomerCommand { get; }
        public RelayCommand UpdateCustomerCommand { get; }
        public RelayCommand DeleteCustomerCommand { get; }
        public RelayCommand GetAllCommand { get; }
        public RelayCommand GetByIDCommand { get; }
        public RelayCommand GetByNameCommand { get; }
        public RelayCommand ClearCommand { get; }

        public CustomerViewModel() : this(new InMemoryCustomerRepository()) { }

        public CustomerViewModel(ICustomerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            Customers = new ObservableCollection<Customer>();

            AddCustomerCommand = new RelayCommand(AddCustomer, CanAddOrUpdate);
            UpdateCustomerCommand = new RelayCommand(UpdateCustomer, CanAddOrUpdate);
            DeleteCustomerCommand = new RelayCommand(DeleteCustomer, CanDelete);
            GetAllCommand = new RelayCommand(_ => LoadCustomers(_repository.GetAll()));
            GetByIDCommand = new RelayCommand(GetByID, CanSearchByID);
            GetByNameCommand = new RelayCommand(_ => LoadCustomers(_repository.GetByName(SearchName)));
            ClearCommand = new RelayCommand(ClearInputFields);

            LoadCustomers(_repository.GetAll());
        }

        private void LoadCustomers(System.Collections.Generic.IEnumerable<Customer> customers)
        {
            Customers.Clear();
            foreach (var customer in customers)
                Customers.Add(customer);
        }

        private bool CanAddOrUpdate(object parameter)
        {
            return !string.IsNullOrWhiteSpace(FirstName)
                && !string.IsNullOrWhiteSpace(LastName)
                && !string.IsNullOrWhiteSpace(Email)
                && !string.IsNullOrWhiteSpace(Address)
                && !string.IsNullOrWhiteSpace(City)
                && !string.IsNullOrWhiteSpace(Country)
                && Points >= 0;
        }

        private bool CanDelete(object parameter) => SelectedCustomer != null;

        private bool CanSearchByID(object parameter)
        {
            return int.TryParse(parameter?.ToString(), out _);
        }

        private void AddCustomer(object parameter)
        {
            try
            {
                var customer = new Customer(FirstName.Trim(), LastName.Trim(), Email.Trim(),
                    Address.Trim(), City.Trim(), Country.Trim(), Points);
                _repository.AddCustomer(customer);
                LoadCustomers(_repository.GetAll());
                ClearInputFields(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not add customer: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCustomer(object parameter)
        {
            try
            {
                SelectedCustomer.FirstName = FirstName.Trim();
                SelectedCustomer.LastName = LastName.Trim();
                SelectedCustomer.Email = Email.Trim();
                SelectedCustomer.Address = Address.Trim();
                SelectedCustomer.City = City.Trim();
                SelectedCustomer.Country = Country.Trim();
                SelectedCustomer.Points = Points;

                _repository.UpdateCustomer(SelectedCustomer);
                LoadCustomers(_repository.GetAll());
                ClearInputFields(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not update customer: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteCustomer(object parameter)
        {
            try
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete '{SelectedCustomer.FirstName} {SelectedCustomer.LastName}'?",
                    "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                    return;

                _repository.DeleteCustomer(SelectedCustomer.CustomerID);
                LoadCustomers(_repository.GetAll());
                ClearInputFields(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not delete customer: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GetByID(object parameter)
        {
            if (!int.TryParse(parameter?.ToString(), out int id))
                return;

            var customer = _repository.GetByID(id);
            Customers.Clear();
            if (customer != null)
                Customers.Add(customer);
        }

        private void ClearInputFields(object parameter)
        {
            SelectedCustomer = null;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            Country = string.Empty;
            Points = 0;
            SearchName = string.Empty;
            SearchID = string.Empty;
        }
    }
}
