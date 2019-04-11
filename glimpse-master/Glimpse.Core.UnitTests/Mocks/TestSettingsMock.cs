using Plugin.Settings.Abstractions;
using System;
using Plugin.Settings;

namespace Glimpse.Core.UnitTests.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class TestSettingsMock
    {
        private const string UserNameKey = "username_key";
        private static  string userName = string.Empty;

        private const string PasswordKey = "password_key";
        private static  string password = string.Empty;

        private const string LoggedInKey = "login_key";
        private static  bool isLoggedIn = false;

        private const string IsVendorAccountKey = "is_vendor_account_key";
        private static bool isVendorAccount = false;

        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static string UserName
        {
            get { return userName; }
            set { value = userName; }
        }

        public static string Password
        {
            get { return password; }
            set { password = value; }
        }
        public static bool LoginStatus
        {
            get { return isLoggedIn; }
            set { isLoggedIn = value; }
        }

        public static bool IsVendorAccount
        {
            get { return isVendorAccount; }
            set { isVendorAccount = value; }
        }

    }
}