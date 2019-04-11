using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using Glimpse.Core.ViewModel;
using Glimpse.Droid.Activities;
using System;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Widget;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V4;

namespace Glimpse.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.left_drawer, true)]
    [Register("glimpse.droid.views.MenuFragment")]
    public class MenuFragment : MvxFragment<MenuViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.fragment_menu, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
        }

        public override void OnStart()
        {
            base.OnStart();
            ViewModel.CloseMenu += OnCloseMenu;

        }

        public override void OnStop()
        {
            base.OnStop();
            ViewModel.CloseMenu -= OnCloseMenu;
        }

        private void OnCloseMenu(object sender, EventArgs e)
        {
            (Activity as MainActivity)?.CloseDrawerMenu();
        }


        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            return true;
        }
    }
}