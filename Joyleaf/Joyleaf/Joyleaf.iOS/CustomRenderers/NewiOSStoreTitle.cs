using CoreGraphics;
using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NewStoreTitle), typeof(NewiOSStoreTitle))]
namespace Joyleaf.iOS.CustomRenderers
{
    public class NewiOSStoreTitle : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            Control.Lines = 2;
        }
    }
}
