using UndderControl.Text;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class MonitorPage : ContentPage
    {
        public MonitorPage()
        {
            InitializeComponent();
            Title1.HtmlText = AppTextResource.MonitorTitle1;
            Text1.Text = AppTextResource.MonitorText1;
            Title2.Text = AppTextResource.MonitorTitle2;
            Text2.HtmlText = AppTextResource.MonitorText2;
            Title3.Text = AppTextResource.MonitorTitle3;
            Text3.HtmlText = AppTextResource.MonitorText3;
            Title4.HtmlText = AppTextResource.MonitorTitle4;
            Text4.HtmlText = AppTextResource.MonitorText4;
            StatusButton.Text = AppTextResource.MonitorStatusButton;
            SummaryButton.Text = AppTextResource.MonitorSummaryButton;
            CompareButton.Text = AppTextResource.MonitorCompareButton;
        }
    }
}
