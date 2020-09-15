using UndderControl.Custom;
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
            Text1.Text = AppTextResource.CowStatusFinishText1;
            Text2.Text = AppTextResource.CowStatusFinishText2;
            #endregion TextSetup

            FinishButton.Effects.Add(Effect.Resolve($"PMN.{nameof(PreventTapJackingEffect)}"));
        }
    }
}
