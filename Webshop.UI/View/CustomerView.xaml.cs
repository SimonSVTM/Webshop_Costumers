using System.Windows.Controls;
using Webshop.UI.ViewModel;

namespace Webshop.UI.View
{
    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            InitializeComponent();
            DataContext = new CustomerViewModel();
        }
    }
}
