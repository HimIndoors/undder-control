using UndderControl.Text;
using UndderControl.ViewModels;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class RootPage : ContentPage
    {
        readonly RootPageViewModel _vm;

        public RootPage()
        {
            InitializeComponent();
            //SetValue(NavigationPage.HasNavigationBarProperty, false);
            _vm = BindingContext as RootPageViewModel;

            RootAssessment1.Text = AppTextResource.RootAssessment1;
            RootAssessment2.Text = AppTextResource.RootAssessment2;
            RootAssessment3.Text = AppTextResource.RootAssessment3;
            RootMonitor1.Text = AppTextResource.RootMonitor1;
            RootMonitor2.Text = AppTextResource.RootMonitor2;
            RootMonitor3.Text = AppTextResource.RootMonitor3;
        }

        private void TapGestureRecognizer_Tapped_Assessment(object sender, System.EventArgs e)
        {
            _vm.FrameAssessmentColour = "#e1e1e1";
        }

        private void TapGestureRecognizer_Tapped_Monitor(object sender, System.EventArgs e)
        {
            _vm.FrameMonitorColour = "#e1e1e1";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
