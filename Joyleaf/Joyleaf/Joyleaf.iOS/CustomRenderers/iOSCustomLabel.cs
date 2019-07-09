using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(iOSCustomLabel))]

namespace Joyleaf.iOS.CustomRenderers
{
    public class iOSCustomLabel : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.TextAlignment = UITextAlignment.Justified;
            }
        }
    }
}