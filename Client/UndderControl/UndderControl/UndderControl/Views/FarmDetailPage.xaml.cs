using UndderControl.ViewModels;
using UndderControlLib.Dtos;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class FarmDetailPage : ContentPage
    {
        readonly FarmDetailPageViewModel vm;
        public FarmDetailPage()
        {
            InitializeComponent();
            vm = BindingContext as FarmDetailPageViewModel;
            FarmHerd.ReturnCommand = new Command(() => comboBox.Focus());
        }

        private void ComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            vm.SelectedType = (FarmTypeDto) e.Value;
        }
    }
}
