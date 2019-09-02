using UndderControl.Text;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class NoResultsComparison : ContentPage
    {
        public NoResultsComparison()
        {
            InitializeComponent();
            PageText.Text = AppResource.NoResultsCompareText;
            PageCaption.Text = AppResource.NoResultsCompareCaption;
        }
    }
}
