using UndderControl.Text;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class NoResultsComparison : ContentPage
    {
        public NoResultsComparison()
        {
            InitializeComponent();
            PageText.Text = AppTextResource.NoResultsCompareText;
            PageCaption.Text = AppTextResource.NoResultsCompareCaption;
        }
    }
}
