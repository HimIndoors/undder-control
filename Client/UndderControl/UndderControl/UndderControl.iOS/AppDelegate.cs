﻿using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;
using UIKit;
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
            LoadApplication(new App(new iOSInitializer()));

            AppCenter.Start(Config.AppCenterIosKey, typeof(Analytics), typeof(Crashes));
            Syncfusion.SfChart.XForms.iOS.Renderers.SfChartRenderer.Init();
            Plugin.InputKit.Platforms.iOS.Config.Init();

            return base.FinishedLaunching(app, options);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.Register<IMetricsManagerService, IOSMetricsManagerService>();
            containerRegistry.Register<ICloseApplicationService, IOSCloseApplicationService>();
        }
    }
}
