using Prism.Events;
using System;
using System.Text;
using UndderControl.Custom;
using UndderControl.Events;
using UndderControl.Text;
using UndderControl.ViewModels;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class CowStatusResultsPage : ContentPage
    {
        private readonly CowStatusResultsPageViewModel _vm;
        public CowStatusResultsPage()
        {
            InitializeComponent();
            _vm = BindingContext as CowStatusResultsPageViewModel;
            FarmName.Text = App.SelectedFarm.Name;

            var newInfectionRate = (int)Math.Round((double)(100 * _vm.Results[AppTextResource.CsNewInfection]) / _vm.Results[AppTextResource.CsNotInfectedAtDryoff]);
            var preventionRate = (int)Math.Round((double)(100 * _vm.Results[AppTextResource.CsPreventionOfNewInfection]) / _vm.Results[AppTextResource.CsNotInfectedAtDryoff]);
            var failureToCureRate = (int)Math.Round((double)(100 * _vm.Results[AppTextResource.CsFailureToCure]) / _vm.Results[AppTextResource.CsInfectedAtDryoff]);
            var cureRate = (int)Math.Round((double)(100 * _vm.Results[AppTextResource.CsCure]) / _vm.Results[AppTextResource.CsInfectedAtDryoff]);

            Cell1KeyValue.Text = _vm.Results[AppTextResource.CsNotInfectedAtDryoff].ToString();
            Cell2KeyValue.Text = _vm.Results[AppTextResource.CsInfectedAtDryoff].ToString();
            Cell3KeyValue.Text = _vm.Results[AppTextResource.CsNotInfectedAfterCalving].ToString();
            Cell4KeyValue.Text = _vm.Results[AppTextResource.CsInfectedAfterCalving].ToString();
            Cell5KeyValue.Text = _vm.Results[AppTextResource.CsNewInfection].ToString();
            Cell6KeyValue.Text = _vm.Results[AppTextResource.CsPreventionOfNewInfection].ToString();
            Cell7KeyValue.Text = _vm.Results[AppTextResource.CsFailureToCure].ToString();
            Cell8KeyValue.Text = _vm.Results[AppTextResource.CsCure].ToString();
            CellniRate.Text = newInfectionRate.ToString();
            CellpRate.Text = preventionRate.ToString();
            CellftcRate.Text = failureToCureRate.ToString();
            CellcRate.Text = cureRate.ToString();
        }
    }
}
