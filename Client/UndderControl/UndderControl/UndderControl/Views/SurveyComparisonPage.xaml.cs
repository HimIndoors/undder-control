using System;
using UndderControl.ViewModels;
using Xamarin.Forms;

namespace UndderControl.Views
{
    public partial class SurveyComparisonPage : ContentPage
    {
        private readonly SurveyComparisonPageViewModel vm;
        public SurveyComparisonPage()
        {
            InitializeComponent();
            vm = BindingContext as SurveyComparisonPageViewModel;
            CreateImprovements();
        }

        private void CreateImprovements()
        {
            foreach (var item in vm.Improvements)
            {
                if (item.Value.Count > 0)
                {
                    var itemLabel = new Label() { Text = item.Key };
                    itemLabel.SetDynamicResource(StyleProperty, "ResultSubtitle");
                    ImprovementStack.Children.Add(itemLabel);

                    foreach (string area in item.Value)
                    {
                        var label = new Label() { Text = area.ToUpper() }; //Forcing Uppercase
                        label.SetDynamicResource(StyleProperty, "TextSubtitle");
                        ImprovementStack.Children.Add(label);
                    }
                }
            }
        }
    }
}
