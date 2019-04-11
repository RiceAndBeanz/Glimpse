
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Glimpse.Core.Services.General;
using Glimpse.Core.ViewModel;
using MvvmCross.Droid.Shared.Caching;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V4;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace Glimpse.Droid.Activities
{
    [Activity(Label = "Login Activity", Theme = "@style/AppTheme",
         LaunchMode = LaunchMode.SingleTop,
         ScreenOrientation = ScreenOrientation.Portrait,
         Name = "glimpse.droid.activities.LoginActivity")]
    public class LoginActivity : MvxCachingFragmentCompatActivity<LoginMainViewModel>
    {
        private static LoginActivity loginActivity;

        public new LoginMainViewModel ViewModel
        {
            get { return base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            if (CheckAuthenticationStatus())
            {
                SetContentView(Resource.Layout.LoginMainView);
                loginActivity = this;

                ViewModel.ShowLoginPage();
            }
            else
            {
                StartActivity(typeof(MainActivity));
                Finish();
            }
            
        }


      /*  public override void OnBeforeFragmentChanging(IMvxCachedFragmentInfo fragmentInfo,
            FragmentTransaction transaction)
        {
            var currentFrag = SupportFragmentManager.FindFragmentById(Resource.Id.login_content) as MvxFragment;

            if ((currentFrag != null) && (currentFrag.FindAssociatedViewModelType() != fragmentInfo.ViewModelType))
                fragmentInfo.AddToBackStack = true;
            base.OnBeforeFragmentChanging(fragmentInfo, transaction);
        } */

        public static LoginActivity getInstance()
        {
            return loginActivity;
        }

        private bool CheckAuthenticationStatus()
        {
            if (!Settings.LoginStatus)
            {
                if (MainActivity.getInstance() != null)
                {
                    MainActivity.getInstance().Finish();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
