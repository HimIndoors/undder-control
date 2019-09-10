using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace UndderControl.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class UserSettings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants
        private const string UserTokenKey = "usertoken";
        private const string UserIdKey = "userid";
        private const string TermsVersionKey = "termsversion";
        #endregion

        /// <summary>
        /// The cached user ID received from login.
        /// </summary>
        public static int UserId
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserIdKey, default(int));
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserIdKey, value);
            }
        }

        /// <summary>
        /// The cached user token received from login.
        /// </summary>
        public static string UserToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserTokenKey, default(string));
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserTokenKey, value);
            }
        }

        /// <summary>
        /// The cached terms version the user last accepted.
        /// </summary>
        public static string TermsVersion
        {
            get
            {
                return AppSettings.GetValueOrDefault(TermsVersionKey, default(string));
            }
            set
            {
                AppSettings.AddOrUpdateValue(TermsVersionKey, value);
            }
        }

    }
}
