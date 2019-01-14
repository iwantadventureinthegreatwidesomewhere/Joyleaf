using System.ComponentModel;
using Android.Content;
using Joyleaf.CustomControls;
using Joyleaf.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NewButton), typeof(NewAndroidButton))]
namespace Joyleaf.Droid.CustomRenderers
{
    public class NewAndroidButton : ButtonRenderer
    {
        public NewAndroidButton(Context context) : base(context)
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

