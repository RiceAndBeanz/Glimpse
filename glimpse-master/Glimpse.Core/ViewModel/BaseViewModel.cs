using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MvvmCross.Plugins.Messenger;

namespace Glimpse.Core.ViewModel
{
    public class BaseViewModel: MvxViewModel, IDisposable
    {
        protected IMvxMessenger Messenger;

        public BaseViewModel(IMvxMessenger messenger) 
        {
            Messenger = messenger;
        }

        protected BaseViewModel()
        {
        }

        public IMvxLanguageBinder TextSource => 
            new MvxLanguageBinder("", GetType().Name);

        protected async Task ReloadDataAsync()
        {
            try
            {
                await InitializeAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        protected virtual Task InitializeAsync()
        {
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            Messenger = null;
        }

        public MvxCommand ShowCommand<TViewModel>()
            where TViewModel : IMvxViewModel
        {
            return new MvxCommand(() => ShowViewModel<TViewModel>());
        }

    }
}