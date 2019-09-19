using UndderControl.Text;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class MonitorPage : ContentPage
    {
        public MonitorPage()
        {
            InitializeComponent();
            Title1.Text = AppTextResource.MonitorTitle1;
            Text1.Text = AppTextResource.MonitorText1;
            Title2.Text = AppTextResource.MonitorTitle2;
            Text2.Text = AppTextResource.MonitorText2;
            Title3.Text = AppTextResource.MonitorTitle3;
            Text3.Text = AppTextResource.MonitorText3;
            Title4.Text = AppTextResource.MonitorTitle4;
            Text4.Text = AppTextResource.MonitorText4;
            StatusButton.Text = AppTextResource.MonitorStatusButton;
            SummaryButton.Text = AppTextResource.MonitorSummaryButton;
            CompareButton.Text = AppTextResource.MonitorCompareButton;
        }
    }
}
