
using Glimpse.Core.Contracts.Services;
using MvvmCross.Core.ViewModels;
using Glimpse.Core.ViewModel;
using MvvmCross.Platform;
using Glimpse.Core.Services.General;
using Glimpse.Core.Utility;
using MvvmCross.Localization;
using Glimpse.Localization;

namespace Glimpse.Core
{
    public class AppStart: MvxNavigatingObject, IMvxAppStart
    {
        public async void Start(object hint = null)
        {
            //Check if the user is logged in before and authenticate
            var authenticator = Mvx.Resolve<ILoginDataService>();
            //Setting the default language of the app
            if (Settings.Language == string.Empty)
                Settings.Language = "English";
            else if(Settings.Language == "Français")
              {
                var re = Mvx.GetSingleton<IMvxTextProvider>();
                ((ResxTextProvider)re).ResourceManager = Strings_Fr.ResourceManager;
              }

            
            if (authenticator.AuthenticateUserLogin())
            {
                ShowViewModel<MainViewModel>();
            }
            else
            { 
                ShowViewModel<LoginMainViewModel>();
            }
        }
    }
}
