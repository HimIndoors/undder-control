using UndderControl.Text;
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
    }
}
