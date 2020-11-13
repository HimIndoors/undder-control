using System;
using UndderControl.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
            VersionLabel.Text = "Version " + VersionTracking.CurrentVersion;
            SmallPrintLabel.Text = AppTextResource.SmallPrint;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            MerckLogo.HeightRequest = 0.5 * Math.Sqrt(0.2 * width * height);
        }
    }
}
