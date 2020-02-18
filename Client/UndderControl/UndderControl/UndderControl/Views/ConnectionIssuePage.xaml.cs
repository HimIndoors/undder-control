using UndderControl.Text;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class ConnectionIssuePage : ContentPage
    {
        public ConnectionIssuePage()
        {
            InitializeComponent();
            ErrorTitle.Text = AppTextResource.ConnIssueTitle;
            ErrorText.Text = AppTextResource.ConnIssueText;
        }
    }
}
