using CoreGraphics;
using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ShadowFrame), typeof(iOSShadowFrame))]

namespace Joyleaf.iOS.CustomRenderers
{
    public class iOSShadowFrame : FrameRenderer
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            Layer.ShadowColor = UIColor.LightGray.CGColor;
            Layer.ShadowOpacity = 0.5f;
            Layer.ShadowRadius = 30f;
            Layer.MasksToBounds = false;
        }
    }
}