using CoreGraphics;
using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NewStorePicker), typeof(NewiOSStorePicker))]
namespace Joyleaf.iOS.CustomRenderers
{
    public class NewiOSStorePicker : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                return;
            }

            Control.BorderStyle = UITextBorderStyle.None;
        }
    }
}
