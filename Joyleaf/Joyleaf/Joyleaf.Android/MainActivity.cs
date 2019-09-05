using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using CarouselView.FormsPlugin.Android;

namespace Joyleaf.Droid
{
    [Activity(Label = "Joyleaf", Icon = "@drawable/icon", Theme = "@style/MyTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CarouselViewRenderer.Init();
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            
            LoadApplication(new App());
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
            }
        }
    }
}
