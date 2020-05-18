using UndderControl.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class TermsPage : ContentPage
    {
        public TermsPage()
        {
            InitializeComponent();
            Terms.Text = AppTextResource.Terms;
            TermsTitle.Text = AppTextResource.TermsTitle;
            TermsButton.Text = AppTextResource.TermsAcceptButton.ToUpper(); //Forcing uppercase
        }

        public async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            await Browser.OpenAsync(AppTextResource.TermsUrl, BrowserLaunchMode.SystemPreferred);
        }
    }
}
