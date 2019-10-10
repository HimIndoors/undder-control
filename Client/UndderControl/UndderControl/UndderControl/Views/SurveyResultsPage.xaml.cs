using Prism.Events;
using UndderControl.Events;
using UndderControl.Text;
using UndderControl.ViewModels;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class SurveyResultsPage : ContentPage
    {
        private readonly SurveyResultsPageViewModel _vm;
        public SurveyResultsPage(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            ImprovementTitle.Text = AppTextResource.SurveyResultImprovementTitle;
            CompareButton.Text = AppTextResource.SurveyResultCompareButton;

            _vm = BindingContext as SurveyResultsPageViewModel;
            UpdateView();
            eventAggregator.GetEvent<StatementsUpdatedEvent>().Subscribe(UpdateView);
        }

        private void UpdateView()
        {
            if (_vm.Statements != null)
            {
                foreach (var item in _vm.Statements)
                {
                    if(item.Value.Count >0)
                    {
                        var itemLabel = new Label() { Text = item.Key.ToUpper() }; //Enforcing uppercase for subtitles
                        itemLabel.SetDynamicResource(StyleProperty, "TextSubtitle");
                        StatementStack.Children.Add(itemLabel);
                        foreach (string statement in item.Value)
                        {
                            var label = new Label() { Text = statement };
                            label.SetDynamicResource(StyleProperty, "Text");
                            StatementStack.Children.Add(label);
                        }
                    }
                }
            }
        }

        private string ReturnHexValue(int score)
        {
            string hexValue;
            switch (score)
            {
                case 1:
                    hexValue = "#F9AC95";
                    break;
                case 2:
                    hexValue = "#F9AC95";
                    break;
                case 3:
                    hexValue = "#FFDBAA";
                    break;
                case 4:
                    hexValue = "#D3E7AF";
                    break;
                case 5:
                    hexValue = "#D3E7AF";
                    break;
                default:
                    hexValue = "#030303";
                    break;
            }
            return hexValue;
        }
    }
}
