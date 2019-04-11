using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Shared.Attributes;
using Glimpse.Core.ViewModel;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using Glimpse.Droid.Extensions;
using Square.TimesSquare;
using Glimpse.Droid.Activities;
using Glimpse.Droid;
using Glimpse.Droid.Views;
using Android.Gms.Location.Places.UI;
using Android.App;
using Android.Content;
using Android.Gms.Maps.Model;
using Android.Util;
using Glimpse.Core.Model;
using Android.Text;
using Java.Lang;
using Glimpse.Droid.Helpers;
using MvvmCross.Binding.BindingContext;
using Android.Graphics;
using Android.Support.Design.Widget;

namespace Glimpse.Droid.Views
{
    [MvxFragment(typeof(Glimpse.Core.ViewModel.LoginMainViewModel), Resource.Id.login_content, true)]
    [Register("glimpse.droid.views.VendorSignUpFragment")]
    public class VendorSignUpFragment : MvxFragment<VendorSignUpViewModel>
    {
        private static readonly int PLACE_PICKER_REQUEST = 1;
        private Button _selectBuisinessLocationButton;
        private TextView _addressTextView;
        private EditText _password;
        private EditText _confirmPassword;
        private EditText _email;
        private BindableProgress _bindableProgress;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            return this.BindingInflate(Resource.Layout.VendorSignUpView, null);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            (this.Activity as LoginActivity).SetCustomTitle("Vendor Sign Up");

            //Set all fonts
            Typeface tf0 = Typeface.CreateFromAsset(this.Activity.Assets, "Avenir-Light.otf");
            Typeface tf1 = Typeface.CreateFromAsset(this.Activity.Assets, "Avenir-Heavy.otf");

            _addressTextView = (this.Activity as LoginActivity).FindViewById<TextView>(Resource.Id.txtAddress);
            _selectBuisinessLocationButton = (this.Activity as LoginActivity).FindViewById<Button>(Resource.Id.selectBusinessLocationButton);
            _selectBuisinessLocationButton.Click += OnSelectBuisinessLocationTapped;

            _email = (this.Activity as LoginActivity).FindViewById<EditText>(Resource.Id.txtEmail);
            _email.SetTypeface(tf0, TypefaceStyle.Normal);

            _email.AfterTextChanged += _email_AfterTextChanged;

            _password = (this.Activity as LoginActivity).FindViewById<EditText>(Resource.Id.txtPassword);
            _password.SetTypeface(tf0, TypefaceStyle.Normal);
            _password.AfterTextChanged += _confirmPassword_AfterTextChanged;

            _confirmPassword = (this.Activity as LoginActivity).FindViewById<EditText>(Resource.Id.txtConfirmPassword);
            _confirmPassword.SetTypeface(tf0, TypefaceStyle.Normal);
            _confirmPassword.AfterTextChanged += _confirmPassword_AfterTextChanged;

            Button btnSignUp = view.FindViewById<Button>(Resource.Id.SignUpButton);
             btnSignUp.SetTypeface(tf1, TypefaceStyle.Normal);

            Button angry_btn = (this.Activity as LoginActivity).FindViewById<Button>(Resource.Id.add);

            angry_btn.SetTypeface(tf1, TypefaceStyle.Normal);


           

            _addressTextView.SetTypeface(tf0, TypefaceStyle.Normal);
            _selectBuisinessLocationButton.SetTypeface(tf0, TypefaceStyle.Normal);


            _bindableProgress = new BindableProgress(this.Context);
            _bindableProgress.Title = ViewModel.TextSource.GetText("Progress");
            var set = this.CreateBindingSet<VendorSignUpFragment, VendorSignUpViewModel>();
            set.Bind(_bindableProgress) .For(p => p.Visible).To(vm => vm.IsBusy);
            set.Apply();            
        }

      

        private void _email_AfterTextChanged(object sender, AfterTextChangedEventArgs e)
        {
            if ((!string.IsNullOrEmpty(ViewModel.Email)) && !Patterns.EmailAddress.Matcher(ViewModel.Email).Matches())
            {
                if (Glimpse.Core.Services.General.Settings.Language == "Français")
                    _email.Error = "Entrez un courriel valide";
                else
                    _email.Error = "Enter a valid email address";
                ViewModel.ValidEmail = false;             
            }
            else
            {
                _email.Error = null;
                ViewModel.ValidEmail = true;
            }

        }

        private void _confirmPassword_AfterTextChanged(object sender, AfterTextChangedEventArgs e)
        {
            if ((!string.IsNullOrEmpty(ViewModel.Password))  && (!string.IsNullOrEmpty(ViewModel.ConfirmPassword)) && (!ViewModel.Password.Equals(ViewModel.ConfirmPassword)))
            {
                if (Glimpse.Core.Services.General.Settings.Language == "Français")
                    _confirmPassword.Error = "Les mots de passe ne correspondent pas";
                else
                    _confirmPassword.Error = "Passwords do not match";
                ViewModel.ValidPassword = false;
            }
            else
            {
                _confirmPassword.Error = null;
                ViewModel.ValidPassword = true;
            }


        }

        private void OnSelectBuisinessLocationTapped(object sender, EventArgs eventArgs)
        {
            PlacePicker.IntentBuilder builder = new PlacePicker.IntentBuilder();
            //If the user already picked a location, the place picker will zoom on the previously selected location. Else, it will default to the geolocation
            if (ViewModel.Location.Lat != 0 && ViewModel.Location.Lng != 0)
             builder.SetLatLngBounds( new LatLngBounds(new LatLng(ViewModel.Location.Lat-0.002500, ViewModel.Location.Lng - 0.002500), new LatLng(ViewModel.Location.Lat + 0.002500, ViewModel.Location.Lng + 0.002500)));     
               
            StartActivityForResult(builder.Build(this.Activity as LoginActivity), PLACE_PICKER_REQUEST);
        }



        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (requestCode == PLACE_PICKER_REQUEST && resultCode == (int) Result.Ok)
            {
                setLocationProperty(data);
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

  
        /// <summary>
        /// This method sets the value of the address text box as well as the view model's location property
        /// </summary>
        /// <param name="data"></param>
        private void setLocationProperty(Intent data)
        {
            var placePicked = PlacePicker.GetPlace(this.Context, data);
            _addressTextView.Text = placePicked?.AddressFormatted?.ToString();

            ViewModel.Location = new Location()
            {
                Lat = placePicked.LatLng.Latitude,
                Lng = placePicked.LatLng.Longitude
            };
        }

        public override void OnStart()
        {
            base.OnStart();
        }       
    
    }
}