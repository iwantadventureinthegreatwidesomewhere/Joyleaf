using CoreGraphics;
using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomiOSPicker))]

namespace Joyleaf.iOS.CustomRenderers
{
    public class CustomiOSPicker : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            Control.LeftView = new UIView(new CGRect(0, 0, 10, Control.Frame.Height));
            Control.LeftViewMode = UITextFieldViewMode.Always;
        }
    }
}
