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
        }
    }
}
