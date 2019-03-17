using CoreAnimation;
using CoreGraphics;
using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExploreFrame), typeof(IOSExploreFrame))]

namespace Joyleaf.iOS.CustomRenderers
{
    public class IOSExploreFrame : FrameRenderer
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            ExploreFrame frame = (ExploreFrame)Element;

            if (frame.IsWhite)
            {
                Layer.ShadowColor = UIColor.LightGray.CGColor;
                Layer.ShadowOpacity = 0.5f;
                Layer.ShadowRadius = 15f;
                Layer.MasksToBounds = false;
            }
            else
            {
                CGColor startColor = frame.StartColor.ToCGColor();
                CGColor endColor = frame.EndColor.ToCGColor();

                var gradientLayer = new CAGradientLayer
                {
                    StartPoint = new CGPoint(0.5, 0),
                    EndPoint = new CGPoint(0.5, 1)
                };

                gradientLayer.Frame = rect;

                gradientLayer.Colors = new CGColor[] { startColor, endColor };

                UIBezierPath path = UIBezierPath.FromRoundedRect(rect, 13f);
                CAShapeLayer shape = new CAShapeLayer
                {
                    Path = path.CGPath
                };

                Layer.Mask = shape;

                NativeView.Layer.InsertSublayer(gradientLayer, 0);
            }
        }
    }
}