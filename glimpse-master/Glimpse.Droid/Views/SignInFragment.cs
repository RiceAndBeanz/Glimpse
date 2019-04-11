using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Glimpse.Core.ViewModel;
using Glimpse.Droid.Activities;
using Glimpse.Droid.Extensions;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Shared.Attributes;
using Glimpse.Droid.Helpers;
using MvvmCross.Binding.BindingContext;
using Android.Text.Method;
using Android.Graphics;

namespace Glimpse.Droid.Views
{
    [MvxFragment(typeof(Glimpse.Core.ViewModel.LoginMainViewModel), Resource.Id.login_content, true)]
    [Register("glimpse.droid.views.SignInFragment")]
    public class SignInFragment : MvxFragment<SignInViewModel>
    {
        private BindableProgress _bindableProgress;
        private EditText _password;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.SignInView, null);

        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            (this.Activity as LoginActivity).SetCustomTitle("Sign In");
            _bindableProgress = new BindableProgress(this.Context);
            _bindableProgress.Title = ViewModel.TextSource.GetText("Progress");
            var set = this.CreateBindingSet<SignInFragment, SignInViewModel>();
            set.Bind(_bindableProgress).For(p => p.Visible).To(vm => vm.IsBusy);
            set.Apply();

            Button btnSignIn = view.FindViewById<Button>(Resource.Id.btnSignIn);
            Typeface tf = Typeface.CreateFromAsset(this.Activity.Assets, "Avenir-Heavy.otf");
            btnSignIn.SetTypeface(tf, TypefaceStyle.Normal);


            EditText txtEmail = view.FindViewById<EditText>(Resource.Id.txtEmail);
            Typeface tf2 = Typeface.CreateFromAsset(this.Activity.Assets, "Avenir-Heavy.otf");
            txtEmail.SetTypeface(tf2, TypefaceStyle.Normal);

            _password = (this.Activity as LoginActivity).FindViewById<EditText>(Resource.Id.txtPassword);
            Typeface tf1 = Typeface.CreateFromAsset(this.Activity.Assets, "Avenir-Heavy.otf");
            _password.SetTypeface(tf1, TypefaceStyle.Normal);
            _password.TextChanged += _password_TextChanged;
        }

        private void _password_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            _password.TransformationMethod=PasswordTransformationMethod.Instance;
            _password.SetSelection(_password.Text.Length);
            _password.TextChanged -= _password_TextChanged;

        }

    }
}