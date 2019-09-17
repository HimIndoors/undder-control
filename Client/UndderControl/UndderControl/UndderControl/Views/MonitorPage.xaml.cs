using UndderControl.Text;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class MonitorPage : ContentPage
    {
        public MonitorPage()
        {
            InitializeComponent();
            Title1.Text = AppResource.MonitorTitle1;
            Text1.Text = AppResource.MonitorText1;
            Title2.Text = AppResource.MonitorTitle2;
            Text2.Text = AppResource.MonitorText2;
            Title3.Text = AppResource.MonitorTitle3;
            Text3.Text = AppResource.MonitorText3;
            Title4.Text = AppResource.MonitorTitle4;
            Text4.Text = AppResource.MonitorText4;
            StatusButton.Text = AppResource.MonitorStatusButton;
            SummaryButton.Text = AppResource.MonitorSummaryButton;
            CompareButton.Text = AppResource.MonitorCompareButton;
        }
    }
}
