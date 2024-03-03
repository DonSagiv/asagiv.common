using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace asagiv.Domain.Core.Models
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        #region Delegates
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        public void RaiseAndSetIfChanging<TProperty>(ref TProperty field,
            TProperty value,
            Func<TProperty, TProperty> validateFunc = null,
            Action<TProperty> valueChangedAction = null,
            [CallerMemberName] string propertyName = null)
        {
            var validatedValue = validateFunc == null
                ? value
                : validateFunc.Invoke(value);

            RaiseAndSetIfChanged(ref field, validatedValue, valueChangedAction, propertyName);
        }

        public void RaiseAndSetIfChanged<TProperty>(ref TProperty field,
            TProperty value,
            Action<TProperty> valueChangedAction = null,
            [CallerMemberName] string propertyName = null)
        {
            if (field == null && value == null || (field?.Equals(value) ?? false))
            {
                return;
            }

            RaiseAndSet(ref field, value, valueChangedAction, propertyName);
        }

        public void RaiseAndSet<TProperty>(ref TProperty field,
            TProperty value,
            Action<TProperty> valueChangedAction = null,
            [CallerMemberName] string propertyName = null)
        {
            field = value;

            RaisePropertyChanged(propertyName);

            valueChangedAction?.Invoke(value);
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
