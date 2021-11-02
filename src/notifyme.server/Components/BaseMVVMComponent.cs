using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using notifyme.shared.ViewModels;

namespace notifyme.server.Components
{
    public class BaseMvvmComponent : ComponentBase, IDisposable, IAsyncDisposable
    {
        private BaseViewModel _vm;
        
        protected void BindViewModelToLifeCycle<T>(T vm) where T: BaseViewModel
        {
            _vm = vm;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && _vm != null)
            {
                _vm.PropertyChanged += OnVmOnPropertyChanged;
                _vm.IsLoading.PropertyChanged += OnVmOnPropertyChanged;
                await _vm.InitializeAsync();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async void OnVmOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            await InvokeAsync(StateHasChanged);
        }
        
        public void Dispose()
        {
            if (_vm is null) return;
            _vm.PropertyChanged -= OnVmOnPropertyChanged;
            GC.SuppressFinalize(this);
        }

        public ValueTask DisposeAsync()
        {
            _vm.PropertyChanged -= OnVmOnPropertyChanged;
            GC.SuppressFinalize(this);
            return ValueTask.CompletedTask;
        }
    }
}