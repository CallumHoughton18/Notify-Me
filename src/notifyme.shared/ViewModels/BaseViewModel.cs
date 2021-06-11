using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using notifyme.shared.Helpers;

namespace notifyme.shared.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private ReturnMessage _isLoading = new ReturnMessage(false, "");
        public ReturnMessage IsLoading
        {
            get => _isLoading;
            set => SetValue(ref _isLoading, value);
        }

        private bool _isInitialized = false;
        public bool IsInitialized
        {
            get => _isInitialized;
            set => SetValue(ref _isInitialized, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual async Task InitializeAsync()
        {
            await Task.Run(() => IsInitialized = true);
        }

        protected void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value)) return;
            backingField = value;
            OnPropertyChanged(propertyName);
        }
    }
}
