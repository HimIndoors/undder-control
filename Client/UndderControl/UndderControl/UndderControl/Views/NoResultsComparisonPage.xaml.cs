using UndderControl.Text;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class NoResultsComparisonPage : ContentPage
    {
        public NoResultsComparisonPage()
        {
            InitializeComponent();
            Title = AppTextResource.NoResultsComparePageTitle;
            PageText.Text = AppTextResource.NoResultsCompareText;
            PageCaption.Text = AppTextResource.NoResultsCompareCaption;
        }
    }
}
