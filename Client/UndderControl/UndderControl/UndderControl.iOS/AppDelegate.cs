using FFImageLoading.Forms.Platform;
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

            new SfComboBoxRenderer();
            new SfNumericTextBoxRenderer();
            Forms9Patch.iOS.Settings.Initialize(this);
            SfChartRenderer.Init();
            SfSegmentedControlRenderer.Init();
            SfListViewRenderer.Init();
            SfTextInputLayoutRenderer.Init();
            SfBorderRenderer.Init();
            SfButtonRenderer.Init();
            SfMaskedEditRenderer.Init();

            Firebase.Core.App.Configure();

            LoadApplication(new App(new iOSInitializer()));

            CachedImageRenderer.Init();
            CachedImageRenderer.InitImageSourceHandler();

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
