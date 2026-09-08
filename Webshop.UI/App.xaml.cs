using System.Windows;
using Webshop.Model; // Adjust if your repositories/interfaces live in another namespace, e.g., Webshop.Core or Webshop.Data
using Webshop.UI.View;
using Webshop.UI.ViewModel;

namespace Webshop.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 1. Initialize repositories
            ICategoryRepository categoryRepo = new CategoryRepository();
            IProductRepository productRepo = new ProductRepository();
            ICustomerRepository customerRepo = new CustomerRepository();

            // 2. Create MainViewModel and inject repositories (or pass instantiated sub-ViewModels)
            var mainViewModel = new MainViewModel(categoryRepo, productRepo, customerRepo);

            // 3. Instantiate MainWindow and assign DataContext
            var mainWindow = new MainView();
            mainWindow.DataContext = mainViewModel;

            // 4. Show the main window
            mainWindow.Show();
        }
    }
}