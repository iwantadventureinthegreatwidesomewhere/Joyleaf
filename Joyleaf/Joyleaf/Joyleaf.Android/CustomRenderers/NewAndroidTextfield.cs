using Android.Content;
using Android.Views.InputMethods;
using Android.Widget;
using Joyleaf.CustomControls;
using Joyleaf.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(NewEntry), typeof(MyEntryRenderer))]
namespace Joyleaf.Droid.CustomRenderers
{
    class MyEntryRenderer : EntryRenderer
    {
        public MyEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SetPadding((int)(Control.PaddingLeft*4.1), (int)(Control.PaddingTop * 1.2), 0, 0);
                Control.SetBackgroundResource(Resource.Drawable.Textfield);

                NewEntry entryReturnKey = (NewEntry)this.Element;
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