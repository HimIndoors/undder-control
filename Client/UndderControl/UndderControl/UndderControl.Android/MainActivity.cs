using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using FFImageLoading.Forms.Platform;
using Firebase.Analytics;
using Plugin.CurrentActivity;
using Prism;
using Prism.Ioc;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UndderControl.Droid.Services;
using UndderControl.Services;
using Environment = System.Environment;

namespace UndderControl.Droid
{
    [Activity(Name = "com.pmn.MainActivity", Label = "UnDDER CONTROL", Icon = "@mipmap/ic_launcher", Theme = "@style/SplashTheme", MainLauncher = true, TaskAffinity = "", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            //AppCenter.Start(Config.AppCenterAndroidKey, typeof(Analytics), typeof(Crashes));
            CrossCurrentActivity.Current.Init(this, bundle);
            UserDialogs.Init(() => this);

            base.Window.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);

            Forms9Patch.Droid.Settings.Initialize(this);
            CachedImageRenderer.Init(enableFastRenderer: true);
            CachedImageRenderer.InitImageViewHandler();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            DisplayCrashReport();

            LoadApplication(new App(new AndroidInitializer())); 
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                const string errorFileName = "Fatal.log";
                var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var errorFilePath = Path.Combine(libraryPath, errorFileName);
                var errorMessage = String.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}",
                DateTime.Now, exception.ToString());
                File.WriteAllText(errorFilePath, errorMessage);

                // Log to Android Device Logging.
                Android.Util.Log.Error("Crash Report", errorMessage);

                var firebaseAnalytics = FirebaseAnalytics.GetInstance(Application.Context);
                var bundle = new Bundle();
                bundle.PutString("Exception", exception.Message);
                bundle.PutString("Stack", exception.StackTrace);
                firebaseAnalytics.LogEvent("AppCrash", bundle);

                new AlertDialog.Builder(Application.Context)
                    .SetNeutralButton("Close", (sender, args) =>
                    {
                        // User pressed Close.
                    })
                    .SetMessage("Oops, something has gone wrong, the error has been logged. Please reopen the app and try again.")
                    .SetTitle("Crashed")
                    .Show();
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        /// <summary>
        // If there is an unhandled exception, the exception information is diplayed 
        // on screen the next time the app is started (only in debug configuration)
        /// </summary>
        [Conditional("DEBUG")]
        private void DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var errorFilePath = Path.Combine(libraryPath, errorFilename);

            if (!File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = File.ReadAllText(errorFilePath);
            new AlertDialog.Builder(this)
                .SetPositiveButton("Clear", (sender, args) =>
                {
                    File.Delete(errorFilePath);
                })
                .SetNegativeButton("Close", (sender, args) =>
                {
            // User pressed Close.
        })
                .SetMessage(errorText)
                .SetTitle("Crash Report")
                .Show();
        } 
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.Register<IMetricsManagerService, AndroidMetricsManagerService>();
            containerRegistry.Register<ICloseApplicationService, AndroidCloseApplicationService>(); 
            containerRegistry.Register<ICookieService, AndroidCookieService>();
        }
    }
}

