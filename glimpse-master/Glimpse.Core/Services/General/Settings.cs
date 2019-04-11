using Android.Hardware.Camera2;
using Glimpse.Core.Model;
using Glimpse.Core.ViewModel;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Glimpse.Core.Services.General
{
    // <summary>
    // This is the Settings static class that can be used in your Core solution or in any
    // of your client applications. All settings are laid out the same exact way with getters
    // and setters. 
    // </summary>
    public static class Settings
    {
        private const string EmailKey = "email_key";
        private static readonly string email = string.Empty;

        private const string PasswordKey = "password_key";
        private static readonly string password = string.Empty;

        private const string LoggedInKey = "login_key";
        private static readonly bool isLoggedIn = false;

        private const string LanguageKey = "language_key";
        private static readonly string language = string.Empty;

        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static string Email
        {
            get { return AppSettings.GetValueOrDefault<string>(EmailKey, email); }
            set { AppSettings.AddOrUpdateValue<string>(EmailKey, value); }
        }

        public static string Password
        {
            get { return AppSettings.GetValueOrDefault<string>(PasswordKey, password); }
            set { AppSettings.AddOrUpdateValue<string>(PasswordKey, value); }
        }

        public static string Language
        {
            get { return AppSettings.GetValueOrDefault<string>(LanguageKey, language); }
            set { AppSettings.AddOrUpdateValue<string>(LanguageKey, value); }
        }

        public static bool LoginStatus
        {
            get { return AppSettings.GetValueOrDefault<bool>(LoggedInKey, isLoggedIn); }
            set { AppSettings.AddOrUpdateValue<bool>(LoggedInKey, value); }
        }
    }
}
