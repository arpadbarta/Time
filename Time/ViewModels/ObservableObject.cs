using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Time.ViewModels
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void Set<T>(ref T refValue, T newValue, Action callBack = null, [CallerMemberName] string propertyName = null)
        {
            if (Equals(refValue, newValue))
            {
                return;
            }

            refValue = newValue;
            callBack?.Invoke();

            RaisePropertyChanged(propertyName);
        }
    }
}
