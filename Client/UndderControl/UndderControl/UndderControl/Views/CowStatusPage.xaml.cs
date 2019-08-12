using UndderControl.Text;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class CowStatusPage : ContentPage
    {
        public CowStatusPage()
        {
            InitializeComponent();
            #region TextSetup
            Text1.Text = AppResource.CowStatusText1;
            Text2.Text = AppResource.CowStatusText2;
            Text3.Text = AppResource.CowStatusText3;
            Text4.Text = AppResource.CowStatusText4;
            Text5.Text = AppResource.CowStatusText5;
            InputTitle.Text = AppResource.CowStatusInputTitle;
            InputCaption.Text = AppResource.CowStatusInputCaption;
            ButtonDryOff.Text = AppResource.CowStatusButtonDryOff;
            ButtonCalving.Text = AppResource.CowStatusButtonCalving;
            #endregion TextSetup
        }
    }
}
