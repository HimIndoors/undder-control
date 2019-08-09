using UndderControl.Text;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class AssessmentPage : ContentPage
    {
        public AssessmentPage()
        {
            InitializeComponent();
            PageTitle.Text = AppResource.AssessPageTitle;
            Title1.Text = AppResource.AssessTitle1;
            Text1.Text = AppResource.AssessText1;
            Title2.Text = AppResource.AssessTitle1;
            Text2.Text = AppResource.AssessText1;
            Title3.Text = AppResource.AssessTitle1;
            Text3.Text = AppResource.AssessText1;
            Title4.Text = AppResource.AssessTitle1;
            Text4.Text = AppResource.AssessText1;
            QuestionButton.Text = AppResource.AssessQuestionButton;
            SummaryButton.Text = AppResource.AssessSummaryButton;
            CompareButton.Text = AppResource.AssessCompareButton;
        }
    }
}
