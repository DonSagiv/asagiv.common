using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace asagiv.common
{
    /// <summary>
    /// INotifyPropertyChanged base class for models in an MVVM pattern UI.
    /// </summary>
    public abstract class PropertyChangedModel : INotifyPropertyChanged
    {
        #region Delegates
        /// <summary>
        /// Implementation of INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Methods
        /// <summary>
        /// General use Raise Property Changed event notifier.
        /// </summary>
        /// <typeparam name="T">Type of the property and field in question.</typeparam>
        /// <param name="field">Field that is being referenced.</param>
        /// <param name="value">Value to set the field to.</param>
        /// <param name="propertyName">Name of the property being changed.</param>
        protected virtual void RaiseAndSetIfChanged<T>(ref T field, T value, [CallerMemberName]string propertyName = "")
        {
            // Raise changed if the original value is null.
            if(field?.Equals(value) ?? false)
            {
                return;
            }

            // Set the field to the value.
            field = value;

            // Raise the PropertyChanged event.
            RaisePropertyChanged(propertyName);
        }

        /// <summary>
        /// Invokes the PropertyChanged event with the property name as the argument.
        /// </summary>
        /// <param name="propertyName">Name of the property that was changed.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
