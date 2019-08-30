using System;
using Android.Content;
using Joyleaf.CustomControls;
using Joyleaf.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TextFixButton), typeof(AndroidTextFixButton))]

namespace Joyleaf.Droid.CustomRenderers
{
    public class AndroidTextFixButton : ButtonRenderer
    {
        public AndroidTextFixButton(Context context) : base(context)
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
