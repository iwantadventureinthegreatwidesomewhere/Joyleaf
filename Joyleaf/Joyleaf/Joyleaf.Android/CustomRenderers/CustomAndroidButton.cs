using Android.Content;
using Joyleaf.CustomControls;
using Joyleaf.Droid.CustomRenderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomAndroidButton))]

namespace Joyleaf.Droid.CustomRenderers
{
    public class CustomAndroidButton : ButtonRenderer
    {
        public CustomAndroidButton(Context context) : base(context)
        {
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Control.SetBackgroundResource(Resource.Drawable.Button);

            Control.SetTextColor(Android.Graphics.Color.White);
        }
    }
}