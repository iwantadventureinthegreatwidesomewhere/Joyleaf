using CoreGraphics;
using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NewSearchBar), typeof(NewiOSSearchBar))]
namespace Joyleaf.iOS.CustomRenderers
{
    public class NewiOSSearchBar : SearchBarRenderer
    {
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Text")
            {
                Control.ShowsCancelButton = false;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> args)
        {
            base.OnElementChanged(args);

            Control.BackgroundImage = new UIKit.UIImage();

        }
    }
}