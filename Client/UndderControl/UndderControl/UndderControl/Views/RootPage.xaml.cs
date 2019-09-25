using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Events;
using UndderControl.Events;
using UndderControl.Text;
using UndderControl.ViewModels;
using UndderControlLib.Dtos;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class RootPage : ContentPage
    {
        readonly RootPageViewModel _vm;

        public RootPage(IEventAggregator eventAggregator)
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

            eventAggregator.GetEvent<RootPageRefreshEvent>().Subscribe(UpdateSelectedFarm);
        }

        private void UpdateSelectedFarm()
        {
            int selectedIndex = new List<FarmDto>(_vm.FarmList).FindIndex(x => x.ID == App.SelectedFarm.ID);
            FarmPicker.SelectedIndex = selectedIndex;
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
