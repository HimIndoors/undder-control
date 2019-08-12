using UndderControl.Text;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class CowStatusFinishPage : ContentPage
    {
        public CowStatusFinishPage()
        {
            InitializeComponent();

            #region TextSetup
            Text1.Text = AppResource.CowStatusFinishText1;
            Text2.Text = AppResource.CowStatusFinishText2;
            #endregion TextSetup
        }
    }
}
