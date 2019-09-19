using UndderControl.Text;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class AssessmentPage : ContentPage
    {
        public AssessmentPage()
        {
            InitializeComponent();
            Title1.Text = AppTextResource.AssessTitle1;
            Text1.Text = AppTextResource.AssessText1;
            Title2.Text = AppTextResource.AssessTitle1;
            Text2.Text = AppTextResource.AssessText1;
            Title3.Text = AppTextResource.AssessTitle1;
            Text3.Text = AppTextResource.AssessText1;
            Title4.Text = AppTextResource.AssessTitle1;
            Text4.Text = AppTextResource.AssessText1;
            QuestionButton.Text = AppTextResource.AssessQuestionButton;
            SummaryButton.Text = AppTextResource.AssessSummaryButton;
            CompareButton.Text = AppTextResource.AssessCompareButton;
        }
    }
}
