using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Joyleaf
{
    public partial class App : Application
    {
        public App()
        {
            if (CrossConnectivity.Current.IsConnected && FirebaseBackend.IsSavedAuthValid())
            {
                MainPage = new NavigationPage(new MainPageView());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPageView());
            }
        }
    }
}