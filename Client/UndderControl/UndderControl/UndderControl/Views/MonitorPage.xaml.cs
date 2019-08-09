using UndderControl.Text;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class MonitorPage : ContentPage
    {
        public MonitorPage()
        {
            InitializeComponent();
            PageTitle.Text = AppResource.MonitorPageTitle;
            Title1.Text = AppResource.MonitorTitle1;
            Text1.Text = AppResource.MonitorText1;
            Title2.Text = AppResource.MonitorTitle1;
            Text2.Text = AppResource.MonitorText1;
            Title3.Text = AppResource.MonitorTitle1;
            Text3.Text = AppResource.MonitorText1;
            Title4.Text = AppResource.MonitorTitle1;
            Text4.Text = AppResource.MonitorText1;
            StatusButton.Text = AppResource.MonitorStatusButton;
            SummaryButton.Text = AppResource.MonitorSummaryButton;
            CompareButton.Text = AppResource.MonitorCompareButton;
        }
    }
}
