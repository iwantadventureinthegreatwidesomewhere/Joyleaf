using System;
using Android.Content;
using Joyleaf.CustomControls;
using Joyleaf.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GradientPage), typeof(AndroidGradientPage))]

namespace Joyleaf.Droid.CustomRenderers
{
    public class AndroidGradientPage : PageRenderer
    {
        public AndroidGradientPage(Context context) : base(context)
        {
        }

        private Color StartColor { get; set; }
        private Color EndColor { get; set; }
        protected override void DispatchDraw(
            Android.Graphics.Canvas canvas)
        {
            var gradient = new Android.Graphics.LinearGradient((float)(Width * 0.25), 0, (float)(Width * 0.75), Height,
                StartColor.ToAndroid(),
                EndColor.ToAndroid(),
                Android.Graphics.Shader.TileMode.Mirror);
            var paint = new Android.Graphics.Paint()
            {
                Dither = true,
            };
            paint.SetShader(gradient);
            canvas.DrawPaint(paint);
            base.DispatchDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
                var page = e.NewElement as GradientPage;
                StartColor = page.StartColor;
                EndColor = page.EndColor;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"          ERROR: ", ex.Message);
            }
        }
    }
}
