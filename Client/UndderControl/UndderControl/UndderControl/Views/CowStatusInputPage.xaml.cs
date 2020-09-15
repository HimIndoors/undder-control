using Prism.Events;
using Syncfusion.XForms.Buttons;
using System;
using UndderControl.Custom;
using UndderControl.Events;
using UndderControl.Text;
using UndderControl.ViewModels;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class CowStatusInputPage : ContentPage
    {
        readonly CowStatusInputPageViewModel vm;
        public CowStatusInputPage(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            vm = BindingContext as CowStatusInputPageViewModel;
            #region TextSetup
            FarmName.Text = App.SelectedFarm.Name;
            Date.Text = DateTime.Now.ToShortDateString();
            IdLabel.Text = AppTextResource.CowStatusInputLabel;
            #endregion TextSetup

            InfectionSegment.Effects.Add(Effect.Resolve($"PMN.{nameof(PreventTapJackingEffect)}"));
            ButtonNext.Effects.Add(Effect.Resolve($"PMN.{nameof(PreventTapJackingEffect)}"));
            ButtonFinish.Effects.Add(Effect.Resolve($"PMN.{nameof(PreventTapJackingEffect)}"));

            SelectionIndicatorSettings selectionIndicator = new SelectionIndicatorSettings
            {
                Position = SelectionIndicatorPosition.Fill,
                Color = Color.FromHex("#FF4081")
            };
            InfectionSegment.SelectionIndicatorSettings = selectionIndicator;

            eventAggregator.GetEvent<CowStatusRefreshEvent>().Subscribe(ClearScreen);
        }

        private void ClearScreen()
        {
            IdEntry.Text = null;
            InfectionSegment.SelectedIndex = 1;
        }

        private void InfectionSegment_SelectionChanged(object sender, Syncfusion.XForms.Buttons.SelectionChangedEventArgs e)
        {
            if (e.Index == 0)
            {
                vm.CowInfected = true;
            }
            else
            {
                vm.CowInfected = false;
            }
        }
    }
}
