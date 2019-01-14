using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof(NewButton), typeof(NewiOSButton))]
namespace Joyleaf.iOS.CustomRenderers
{
    public class NewiOSButton : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetTitleColor(UIColor.White, UIControlState.Disabled);
            }
        }
    }
}

