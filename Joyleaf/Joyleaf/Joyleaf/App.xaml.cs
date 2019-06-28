using Joyleaf.Helpers;
using Joyleaf.Views;
using Xamarin.Forms;

namespace Joyleaf
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTE0NDI2QDMxMzYyZTM0MmUzME1zeXVBRGtQbE5wUVRJQ3hlLzVXcGRvWkJKczNvVlU1RHVpTGV6dTB0OVk9");

            if (string.IsNullOrEmpty(Settings.FirebaseAuth))
            {
                MainPage = new NavigationPage(new StartPageView());
            }
            else
            {
                MainPage = new NavigationPage(new MainPageView());
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            System.Collections.Generic.IReadOnlyList<Page> stack = Current.MainPage.Navigation.NavigationStack;

            if (stack[0].GetType() == typeof(MainPageView))
            {
                MainPageView page = (MainPageView)Current.MainPage.Navigation.NavigationStack[0];
                page.Resume();
            }
        }
    }
}