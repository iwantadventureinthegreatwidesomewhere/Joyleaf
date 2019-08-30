using System;
using Android.Content;
using Joyleaf.CustomControls;
using Joyleaf.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PostReviewButton), typeof(AndroidPostReviewButton))]

namespace Joyleaf.Droid.CustomRenderers
{
    public class AndroidPostReviewButton : ButtonRenderer
    {
        public AndroidPostReviewButton(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
                Control.SetPadding(0, 0, 0, 0);
        }
    }
}
