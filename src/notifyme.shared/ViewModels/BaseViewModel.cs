using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace notifyme.shared.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetValue(ref _isBusy, value);
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
