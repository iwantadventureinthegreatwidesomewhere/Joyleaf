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

            Control.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 20);
            Control.BorderStyle = UITextBorderStyle.None;
            Control.Layer.CornerRadius = 23;
            Control.Layer.MasksToBounds = true;

            Control.LeftView = new UIView(new CGRect(0, 0, 16, Control.Frame.Height));
            Control.LeftViewMode = UITextFieldViewMode.Always;
        }
    }
}