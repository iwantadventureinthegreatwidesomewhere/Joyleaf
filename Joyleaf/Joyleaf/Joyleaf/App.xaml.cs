using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Joyleaf
{
    public partial class App : Application
    {
        public App ()
        {
            if(CrossConnectivity.Current.IsConnected){
                if (FirebaseBackend.IsSavedAuthValid())
                {
                    var main = new MainPage();
                    main.EnableLoader();
                    MainPage = new NavigationPage(main);
                }
                else
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
            }
            else{
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void OnResume(){




        }
    }
}
