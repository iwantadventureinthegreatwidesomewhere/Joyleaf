using Joyleaf.Helpers;
using Joyleaf.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Joyleaf
{
    public partial class App : Application
    {
        public App()
        {
            if (string.IsNullOrEmpty(Settings.FirebaseAuth))
            {
                MainPage = new NavigationPage(new SignInPageView());
            }
            else
            {
                MainPage = new NavigationPage(new MainPageView());
            }
        }
    }
}