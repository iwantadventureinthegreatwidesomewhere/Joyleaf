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
                MainPage = new NavigationPage(new StartPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            System.Collections.Generic.IReadOnlyList<Page> stack = Current.MainPage.Navigation.NavigationStack;

            if (stack[0].GetType() == typeof(MainPage))
            {
                MainPage page = (MainPage)Current.MainPage.Navigation.NavigationStack[0];
                page.Resume();
            }
        }
    }
}