using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.SafetyNet;
using Android.Gms.Security;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using SQLitePCL;

namespace UndderControl.Droid
{
    [Activity(Label = "UndderControl", Icon = "@mipmap/ic_launcher", Theme = "@style/SplashTheme", MainLauncher = true)]
    public class SplashActivity : Activity
    { 
        private static readonly int REQUEST_GOOGLE_PLAY_SERVICES_DOWNLOAD = 1000;
        GoogleApiClient googleApiClient;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ProviderInstallerCheck(this);
            StartActivity(typeof(MainActivity));
            Finish();
        }

        public void ProviderInstallerCheck(Context context)
        {
            try
            {
                // Checking if the ProviderInstaller is installed and updated
                ProviderInstaller.InstallIfNeeded(context);
                Toast.MakeText(context, "Provider installed and updated!", ToastLength.Long).Show();
            }
            catch (GooglePlayServicesRepairableException e)
            {
                /* If the ProviderInstaller is installed but not updated
                A popup asks the user to do a manual update of the Google Play Services
                 */
                #pragma warning disable CS0618 // Type or member is obsolete
                GooglePlayServicesUtil.ShowErrorNotification(e.ConnectionStatusCode, context);
                #pragma warning restore CS0618 // Type or member is obsolete
                Toast.MakeText(context, "Provider it outdated. Please update your Google Play Service", ToastLength.Long).Show();

            }
            catch (GooglePlayServicesNotAvailableException e)
            {
                /* If the ProviderInstaller is not installed but not updated
                A popup redirects the user to the Google Play Services page on the Google PlayStore
                and let the user download them.
                 */
                #pragma warning disable CS0618 // Type or member is obsolete
                Dialog dialog = GooglePlayServicesUtil.GetErrorDialog(e.ErrorCode, this, REQUEST_GOOGLE_PLAY_SERVICES_DOWNLOAD);
                #pragma warning restore CS0618 // Type or member is obsolete

                dialog.SetCancelable(false);
                dialog.Show();
            }
        }
    }
}