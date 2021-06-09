using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using notifyme.shared.ViewModels;

namespace notifyme.server.Components
{
    public class BaseMVVMComponent : ComponentBase, IDisposable, IAsyncDisposable
    {
        private BaseViewModel _vm;
        
        protected void BindViewModelToLifeCycle<T>(T vm) where T: BaseViewModel
        {
            _vm = vm;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _vm.PropertyChanged += OnVmOnPropertyChanged;
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
            GC.SuppressFinalize(this);
            if (_vm is null) return;
            _vm.PropertyChanged -= OnVmOnPropertyChanged;
        }

        public ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            return _vm is null ? 
                ValueTask.CompletedTask : 
                new ValueTask(new Task(() => _vm.PropertyChanged -= OnVmOnPropertyChanged));
        }
    }
}