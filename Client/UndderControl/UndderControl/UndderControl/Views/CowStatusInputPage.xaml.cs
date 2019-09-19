using Syncfusion.XForms.Buttons;
using System;
using UndderControl.Text;
using UndderControl.ViewModels;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class CowStatusInputPage : ContentPage
    {
        public CowStatusInputPage()
        {
            
            InitializeComponent();
            #region TextSetup
            FarmName.Text = App.SelectedFarm.Name;
            Date.Text = DateTime.Now.ToShortDateString();
            IdLabel.Text = AppTextResource.CowStatusInputLabel;
            ButtonNext.Text = AppTextResource.CowStatusInputButtonNext;
            ButtonFinish.Text = AppTextResource.CowStatusInputButtonFinish;
            #endregion TextSetup

            SelectionIndicatorSettings selectionIndicator = new SelectionIndicatorSettings();
            selectionIndicator.Position = SelectionIndicatorPosition.Fill;
            selectionIndicator.Color = Color.FromHex("#009994");
            InfectionSegment.SelectionIndicatorSettings = selectionIndicator;
        }
    }
}
