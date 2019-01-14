using System.ComponentModel;
using Android.Content;
using Joyleaf.CustomControls;
using Joyleaf.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NewStoreTitle), typeof(NewAndroidStoreTitle))]
namespace Joyleaf.Droid.CustomRenderers
{

    public class NewAndroidStoreTitle : LabelRenderer
    {

        public NewAndroidStoreTitle()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            Control.SetSingleLine(false);
            Control.SetMaxLines(2);
        }
    }
}
