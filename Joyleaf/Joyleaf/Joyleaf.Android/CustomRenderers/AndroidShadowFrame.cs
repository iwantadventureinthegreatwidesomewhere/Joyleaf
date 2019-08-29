
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Support.V4.View;
using Joyleaf.CustomControls;
using Joyleaf.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ShadowFrame), typeof(AndroidShadowFrame))]

namespace Joyleaf.Droid.CustomRenderers
{
    public class AndroidShadowFrame : Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer
    {
        public AndroidShadowFrame(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
                return;

            Control.StateListAnimator = new Android.Animation.StateListAnimator();

            ViewCompat.SetElevation(this, 50);
            ViewCompat.SetElevation(Control, 50);
        }
    }
}
