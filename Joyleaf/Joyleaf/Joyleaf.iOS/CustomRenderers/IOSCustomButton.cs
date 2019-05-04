﻿using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomButton), typeof(IOSCustomButton))]

namespace Joyleaf.iOS.CustomRenderers
{
    public class IOSCustomButton : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetTitleColor(Color.FromHex("#333333").ToUIColor(), UIControlState.Disabled);
            }
        }
    }
}
