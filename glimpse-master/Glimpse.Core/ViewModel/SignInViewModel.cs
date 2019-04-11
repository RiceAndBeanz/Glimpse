using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Glimpse.Core.Contracts.Services;
using Glimpse.Core.Model;
using Glimpse.Core.Services.General;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Plugin.RestClient;
using Plugin.Settings.Abstractions;

namespace Glimpse.Core.ViewModel
{
    public class SignInViewModel : BaseViewModel
    {
        private readonly IVendorDataService _vendorDataService;

        private readonly ILoginDataService _loginDataService;
        private string _email;
        private string _password;
        private Vendor currentVendor;

        public SignInViewModel(IMvxMessenger messenger, IVendorDataService vendorDataService, ILoginDataService loginDataService) : base(messenger)
        {
            _vendorDataService = vendorDataService;         
            _loginDataService = loginDataService;
        }


        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                RaisePropertyChanged(() => ErrorMessage);
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }


        /// <summary>
        /// Init method so that list is refreshed when show view model is called
        /// The parameter is required to be able to get this method called since none exist with empty argument...
        /// </summary>
        /// <param name="index"></param>
        public void Init(int index)
        {
            ErrorMessage = "";
        }


        public MvxCommand SignInCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    IsBusy = true;
                    if ((!string.IsNullOrEmpty(Email)) && (!string.IsNullOrEmpty(Password)))
                    {
                        currentVendor = await _vendorDataService.SearchVendorByEmail(Email);

                        if (currentVendor != null)
                        {
                            if (await _loginDataService.AuthenticateVendor(Email, Password))
                            {
                                IsBusy = false;
                                ShowViewModel<MapViewModel>();
                            }
                            IsBusy = false;
                        }
                        else
                        {
                            IsBusy = false;
                            ErrorMessage = TextSource.GetText("IncorrectInputMsg");
                        }
                    }
                    else
                    {
                        IsBusy = false;
                        ErrorMessage = TextSource.GetText("MissingField");                        
                    }

                });
            }
        }

    }
}
