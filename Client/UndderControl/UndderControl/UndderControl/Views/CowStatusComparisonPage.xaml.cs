using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class CowStatusComparisonPage : ContentPage
    {
        public CowStatusComparisonPage()
        {
            InitializeComponent();
            FarmName.Text = App.SelectedFarm.Name;
        }
    }
}
