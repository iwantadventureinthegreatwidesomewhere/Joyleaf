﻿using CoreAnimation;
using CoreGraphics;
using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GradientPage), typeof(iOSGradientPage))]

namespace Joyleaf.iOS.CustomRenderers
{
    public class iOSGradientPage : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                GradientPage page = (GradientPage)Element;

                var gradientLayer = new CAGradientLayer
                {
                    StartPoint = new CGPoint(0.25, 0),
                    EndPoint = new CGPoint(0.75, 1)
                };

                gradientLayer.Frame = View.Bounds;

                gradientLayer.Colors = new CGColor[] { page.StartColor.ToCGColor(), page.EndColor.ToCGColor() };

                View.Layer.InsertSublayer(gradientLayer, 0);
            }
        }
    }
}
