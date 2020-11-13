using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Security;
using Android.OS;
using Android.Widget;

namespace UndderControl.Droid
{
    [Activity(Name = "com.pmn.SplashActivity", Label = "UnDDER CONTROL", Icon = "@mipmap/ic_launcher", Theme = "@style/SplashTheme", MainLauncher = true, TaskAffinity = "")]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));
            Finish();
        }
    }
}