using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Webshop.UI.ViewModel
{
    /// <summary>
    /// Base class for all ViewModels. Implements INotifyPropertyChanged
    /// so the View is automatically updated when a property changes.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the backing field and raises PropertyChanged only when the value actually changes.
        /// </summary>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
