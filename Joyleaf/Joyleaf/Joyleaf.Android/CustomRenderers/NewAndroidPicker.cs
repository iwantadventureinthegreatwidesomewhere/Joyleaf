using System.ComponentModel;
using Android.Content;
using Joyleaf.CustomControls;
using Joyleaf.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NewPicker), typeof(NewAndroidPicker))]
namespace Joyleaf.Droid.CustomRenderers
{

    public class NewAndroidPicker: PickerRenderer
    {

        public NewAndroidPicker(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Picker> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            Control.SetPadding((int)(Control.PaddingLeft * 4.1), (int)(Control.PaddingTop * -0.3), 0, 0);
            Control.SetBackgroundResource(Resource.Drawable.Textfield);
        }
    }
}
