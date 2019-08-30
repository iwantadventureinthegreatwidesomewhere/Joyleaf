using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Text;
using Joyleaf.CustomControls;
using Joyleaf.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PostReviewEditor), typeof(AndroidPostReviewEditor))]

namespace Joyleaf.Droid.CustomRenderers
{
    public class AndroidPostReviewEditor : EditorRenderer
    {
        public AndroidPostReviewEditor(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable GradientDrawable = new GradientDrawable();
                GradientDrawable.SetColor(Android.Graphics.Color.Transparent);

                Control.SetBackground(GradientDrawable);
                Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
            }
        }
    }
}
