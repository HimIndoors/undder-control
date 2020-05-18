using FFImageLoading.Forms.Platform;
using Firebase.Analytics;
using Foundation;
using Prism;
using Prism.Ioc;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfChart.XForms.iOS.Renderers;
using Syncfusion.SfNumericTextBox.XForms.iOS;
using Syncfusion.XForms.iOS.Border;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.iOS.ComboBox;
using Syncfusion.XForms.iOS.MaskedEdit;
using Syncfusion.XForms.iOS.TextInputLayout;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using UndderControl.Custom;
using UndderControl.iOS.Custom;
using UndderControl.iOS.Services;
using UndderControl.Services;

namespace UndderControl.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            SfComboBoxRenderer.Init();
            SfNumericTextBoxRenderer.Init();
            Forms9Patch.iOS.Settings.Initialize(this);
            SfChartRenderer.Init();
            SfSegmentedControlRenderer.Init();
            SfListViewRenderer.Init();
            SfTextInputLayoutRenderer.Init();
            SfBorderRenderer.Init();
            SfButtonRenderer.Init();
            SfMaskedEditRenderer.Init();

            Firebase.Core.App.Configure();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            DisplayCrashReport();

            LoadApplication(new App(new iOSInitializer()));

            CachedImageRenderer.Init();
            CachedImageRenderer.InitImageSourceHandler();

            //Thread.Sleep(2000);

            return base.FinishedLaunching(app, options);
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
                var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources); 
                var errorFilePath = Path.Combine(libraryPath, errorFileName);
                var errorMessage = string.Format(CultureInfo.InvariantCulture, "Time: {0}\r\nError: Unhandled Exception\r\n{1}", DateTime.Now, exception.ToString());
                File.WriteAllText(errorFilePath, errorMessage);

                var keys = new List<NSString>();
                var values = new List<NSString>(); 
                keys.Add(new NSString("Exception"));
                values.Add(new NSString(exception.Message));
                keys.Add(new NSString("Stack"));
                values.Add(new NSString(exception.StackTrace));
                var parametersDictionary = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count); 

                Analytics.LogEvent("AppCrash", parametersDictionary);

                var alertView = new UIAlertView()
                {
                    Title = "Crashed",
                    Message = "Oops, something has gone wrong, the error has been logged. Please reopen the app and try again."
                };
                alertView.Clicked += (sender, args) =>
                {
                    //User clicked OK
                };
                alertView.Show();
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
        private static void DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";
            var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
            var errorFilePath = Path.Combine(libraryPath, errorFilename);

            if (!File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = File.ReadAllText(errorFilePath);
            var alertView = new UIAlertView()
            {
                Title = "Crash Report",
                Message = errorText
            };
            alertView.AddButton("OK");
            alertView.Clicked += (sender, args) =>
            {
                //User clicked OK
                File.Delete(errorFilePath);
            };
            alertView.Show();
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.Register<IMetricsManagerService, IOSMetricsManagerService>();
            containerRegistry.Register<ICloseApplicationService, IOSCloseApplicationService>();
            containerRegistry.Register<IClearCookies, IClearCookiesImplementation>();
        }
    }
}
