using UndderControl.Custom;
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
            Text1.HtmlText = AppTextResource.AssessText1;
            Title2.Text = AppTextResource.AssessTitle2;
            Text2.HtmlText = AppTextResource.AssessText2;
            Title3.Text = AppTextResource.AssessTitle3;
            Text3.HtmlText = AppTextResource.AssessText3;
            Text4.HtmlText = AppTextResource.AssessText4;
            QuestionButton.Text = AppTextResource.AssessQuestionButton;
            SummaryButton.Text = AppTextResource.AssessSummaryButton;
            CompareButton.Text = AppTextResource.AssessCompareButton;

            QuestionButton.Effects.Add(Effect.Resolve($"PMN.{nameof(PreventTapJackingEffect)}"));
            SummaryButton.Effects.Add(Effect.Resolve($"PMN.{nameof(PreventTapJackingEffect)}"));
            CompareButton.Effects.Add(Effect.Resolve($"PMN.{nameof(PreventTapJackingEffect)}"));
        }
    }
}
