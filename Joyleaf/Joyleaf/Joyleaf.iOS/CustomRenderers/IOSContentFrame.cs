using CoreGraphics;
using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentFrame), typeof(IOSContentFrame))]

namespace Joyleaf.iOS.CustomRenderers
{
    public class IOSContentFrame : FrameRenderer
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            Layer.ShadowColor = UIColor.LightGray.CGColor;
            Layer.ShadowOpacity = 0.5f;
            Layer.ShadowRadius = 15f;
            Layer.MasksToBounds = false;
        }
    }
}