using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using UndderControl.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Syncfusion.SfNumericTextBox.XForms;
using Syncfusion.XForms.Themes;

[assembly: ExportRenderer(typeof(CowStatusResultsPage), typeof(UndderControl.iOS.Custom.CowStatusResultsRenderer))]
namespace UndderControl.iOS.Custom
{
    public class CowStatusResultsRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
                SetAppTheme();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"\t\t\tERROR: {ex.Message}");
            }
        }

        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            base.TraitCollectionDidChange(previousTraitCollection);
            Console.WriteLine($"TraitCollectionDidChange: {TraitCollection.UserInterfaceStyle} != {previousTraitCollection.UserInterfaceStyle}");

            if (this.TraitCollection.UserInterfaceStyle != previousTraitCollection.UserInterfaceStyle)
            {
                SetAppTheme();
            }
        }

        void SetAppTheme()
        {
            ICollection<ResourceDictionary> mergedDictionaries = Prism.PrismApplicationBase.Current.Resources.MergedDictionaries;
            var numericUpDownStyles = mergedDictionaries.OfType<SfNumericTextBoxStyles>().FirstOrDefault();

            if (numericUpDownStyles == null)
            {
                mergedDictionaries.Add(new SfNumericTextBoxStyles());
            }

            if (this.TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
            {
                var darktheme = mergedDictionaries.OfType<DarkTheme>().FirstOrDefault();

                if (darktheme != null)
                {
                    return;
                }
                mergedDictionaries.Add(new DarkTheme());
            }
            else
            {
                var lightTheme = mergedDictionaries.OfType<LightTheme>().FirstOrDefault();

                if (lightTheme != null)
                {
                    return;
                }
                mergedDictionaries.Add(new LightTheme());
            }
        }
    }
}