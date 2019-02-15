using Android.Content;
using Android.Widget;
using Joyleaf.CustomControls;
using Joyleaf.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomAndroidEntry))]

namespace Joyleaf.Droid.CustomRenderers
{
    class CustomAndroidEntry : EntryRenderer
    {
        public CustomAndroidEntry(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetBackgroundResource(Resource.Drawable.Entry);

                Control.SetPadding((int)(Control.PaddingLeft*4.1), (int)(Control.PaddingTop * 1.2), 0, 0);

                CustomEntry entryReturnKey = (CustomEntry)Element;
                if (entryReturnKey != null)
                {  
                    Control.EditorAction += (object sender, TextView.EditorActionEventArgs args) =>
                    {
                        entryReturnKey.InvokeCompleted();
                    };
                }
            }
        }
    }
}