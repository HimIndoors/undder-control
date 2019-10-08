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
            Text1.Text = AppTextResource.CowStatusText1;
            Text2.HtmlText = AppTextResource.CowStatusText2;
            Text3.Text = AppTextResource.CowStatusText3;
            Text4.HtmlText = AppTextResource.CowStatusText4;
            Text5.Text = AppTextResource.CowStatusText5;
            InputTitle.Text = AppTextResource.CowStatusInputTitle;
            InputCaption.Text = AppTextResource.CowStatusInputCaption;
            ButtonDryOff.Text = AppTextResource.CowStatusButtonDryOff;
            ButtonCalving.Text = AppTextResource.CowStatusButtonCalving;
            #endregion TextSetup
        }
    }
}
