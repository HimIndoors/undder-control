using System;
using Prism.Events;
using UndderControl.Events;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class ManageFarmsPage : ContentPage
    {
        public ManageFarmsPage(IEventAggregator ea)
        {
            InitializeComponent();
            ea.GetEvent<FarmNavigationEvent>().Subscribe(UpdateView);
        }

        private void UpdateView()
        {
            FarmList.SelectedItem = null;
        }

        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.FromHex("#7CCCBD");
            }
        }
    }
}
