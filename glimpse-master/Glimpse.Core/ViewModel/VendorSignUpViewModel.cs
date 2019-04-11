using MvvmCross.Plugins.Messenger;
using MvvmCross.Core.ViewModels;
using Glimpse.Core.Model;
using Glimpse.Core.Contracts.Services;
using Glimpse.Core.Services.General;

namespace Glimpse.Core.ViewModel
{
    public class VendorSignUpViewModel : BaseViewModel
    {
        private Vendor vendor;

        private readonly IVendorDataService _vendorDataService;
        private readonly IUserDataService _userDataService;

        public VendorSignUpViewModel(IMvxMessenger messenger, IVendorDataService vendorDataService, IUserDataService userDataService) : base(messenger)
        {
            _vendorDataService = vendorDataService;
            _userDataService = userDataService;
            _validEmail = false;
            _validPassword = false;
        }

        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                RaisePropertyChanged(() => CompanyName);

            }
        }


        private Location _location;
        public Location Location
        {
            get
            {
                if (_location == null)
                    _location = new Location();

                return _location;
            }
            set
            {
                _location = value;
                RaisePropertyChanged(() => Location);
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                RaisePropertyChanged(() => Address);

            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);

            }
        }


        private string _businessPhoneNumber;
        public string BusinessPhoneNumber
        {
            get { return _businessPhoneNumber; }
            set
            {
                _businessPhoneNumber = value;
                RaisePropertyChanged(() => BusinessPhoneNumber);

            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }


        private string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                RaisePropertyChanged(() => ConfirmPassword);
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


        private bool _validEmail;
        public bool ValidEmail
        {
            get { return _validEmail; }
            set { _validEmail = value; }
        }

        private bool _validPassword;
        public bool ValidPassword
        {
            get { return _validPassword; }
            set { _validPassword = value; }
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

        public MvxCommand SignUpCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    IsBusy = true;
                    if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(CompanyName) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(BusinessPhoneNumber) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
                    {
                        ErrorMessage = TextSource.GetText("ErrorMissingFieldMsg");
                        IsBusy = false;
                    }
                    //Check email validation
                    else if (!ValidEmail)
                    {
                        ErrorMessage = TextSource.GetText("ErrorEmailIsNotValidMsg");
                        IsBusy = false;
                    }
                    //Password validation
                    else if (!ValidPassword)
                    {
                        ErrorMessage = TextSource.GetText("ErrorPasswordMatchMsg");
                        IsBusy = false;
                    }                   
                    //Check if email exists in db
                    else if (await _vendorDataService.CheckIfVendorExists(Email))
                    {
                        ErrorMessage = Email + " "+ TextSource.GetText("ErrorEmailExistsMsg");
                        IsBusy = false;
                    }
                    else
                    {
                        Vendor newVendor = new Vendor()
                        {
                            CompanyName = _companyName,
                            Email = _email,
                            Telephone = _businessPhoneNumber,
                            Password = _password,
                            Location = _location,
                            Address = Address
                        };

                        Settings.LoginStatus = true;
                        Settings.Email = _email;

                        await _vendorDataService.SignUp(newVendor);
                        IsBusy = false;

                        ShowViewModel<MapViewModel>();
                    }
                });
            }
        }
    }
}
    