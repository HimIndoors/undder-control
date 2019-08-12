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
            IdLabel.Text = AppResource.CowStatusInputLabel;
            ButtonNext.Text = AppResource.CowStatusInputButtonNext;
            ButtonFinish.Text = AppResource.CowStatusInputButtonFinish;

            #endregion TextSetup
        }
    }
}
