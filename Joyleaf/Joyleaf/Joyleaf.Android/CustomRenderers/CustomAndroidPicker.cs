using Android.Content;
using Joyleaf.CustomControls;
using Joyleaf.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomAndroidPicker))]

namespace Joyleaf.Droid.CustomRenderers
{
    public class CustomAndroidPicker: PickerRenderer
    {
        public CustomAndroidPicker(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            Control.SetBackgroundResource(Resource.Drawable.Entry);

            Control.SetPadding((int)(Control.PaddingLeft * 4.1), (int)(Control.PaddingTop * -0.3), 0, 0);
        }
    }
}