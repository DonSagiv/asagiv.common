using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace asagiv.common
{
    public abstract class PropertyChangedModel : INotifyPropertyChanged
    {
        #region Delegates
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Methods
        protected virtual void RaiseAndSetIfChanged<T>(ref T field, T value, [CallerMemberName]string propertyName = "")
        {
            // Raise changed if the original value is null.
            if(field?.Equals(value) ?? false)
            {
                return;
            }

            field = value;

            RaisePropertyChanged(propertyName);
        }

        protected virtual void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
