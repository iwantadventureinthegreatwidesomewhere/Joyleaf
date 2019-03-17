using CoreGraphics;
using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(IOSCustomPicker))]

namespace Joyleaf.iOS.CustomRenderers
{
    public class IOSCustomPicker : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            Control.Layer.CornerRadius = 23;

            Control.BackgroundColor = UIColor.FromRGB(244, 243, 250);

            Control.Layer.BorderColor = UIColor.FromRGB(244, 243, 250).CGColor;
            Control.Layer.BorderWidth = 1;

            Control.Layer.MasksToBounds = true;

            Control.LeftView = new UIView(new CGRect(0, 0, 10, Control.Frame.Height));
            Control.LeftViewMode = UITextFieldViewMode.Always;
        }
    }
}