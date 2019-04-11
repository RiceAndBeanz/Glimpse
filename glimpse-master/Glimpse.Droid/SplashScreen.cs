using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace Glimpse.Droid
{
    [Activity(
        MainLauncher = true,
        Label = "@string/ApplicationName",
        Icon = "@drawable/Logo",
        Theme = "@style/Theme.Splash", NoHistory = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, 
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }
    }
}