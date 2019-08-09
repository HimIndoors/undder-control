using UndderControl.Text;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class TermsPage : ContentPage
    {
        public TermsPage()
        {
            InitializeComponent();
            Terms.Text = AppResource.Terms;
            TermsTitle.Text = AppResource.TermsTitle;
        }
    }
}
