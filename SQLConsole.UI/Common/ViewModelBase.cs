using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace SQLConsole.UI.ViewModels
{
    /// <summary>
    /// Abstract base class of all ViewModels.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase()
        {}

        /// <summary>
        /// Gets the dispay name of current ViewModel.
        /// </summary>
        public virtual String DisplayName { get; protected set; }

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        public void NotifyError(string message)
        {
            MessageBox.Show(message);
        }
    }
}