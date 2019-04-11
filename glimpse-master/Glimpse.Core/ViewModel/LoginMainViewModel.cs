using System;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace Glimpse.Core.ViewModel
{
    public class LoginMainViewModel : MvxViewModel
    {
        private readonly Lazy<VendorSignUpViewModel> _signupVendorViewModel;
        private readonly Lazy<MapViewModel> _mapViewModel;
        private readonly Lazy<SignInViewModel> _signInViewModel;
        private readonly Lazy<LoginSettingsViewModel> _settingsViewModel;

        public VendorSignUpViewModel VendorSignUpViewModel => _signupVendorViewModel.Value;

        public MapViewModel MapViewModel => _mapViewModel.Value;
        public SignInViewModel SignInViewModel => _signInViewModel.Value;
 
        public LoginMainViewModel(IMvxMessenger messenger)
        {
            _settingsViewModel = new Lazy<LoginSettingsViewModel>(Mvx.IocConstruct<LoginSettingsViewModel>);
            _mapViewModel = new Lazy<MapViewModel>(Mvx.IocConstruct<MapViewModel>);
            _signupVendorViewModel = new Lazy<VendorSignUpViewModel>(Mvx.IocConstruct<VendorSignUpViewModel>);
            _signInViewModel = new Lazy<SignInViewModel>(Mvx.IocConstruct<SignInViewModel>);
        }

        public void ShowLoginPage()
        {
            ShowViewModel<LoginViewModel>();
        }

    }
}

