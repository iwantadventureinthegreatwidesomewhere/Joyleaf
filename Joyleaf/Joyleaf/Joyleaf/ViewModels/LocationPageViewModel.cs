using Xamarin.Forms;

namespace Joyleaf
{

    public class LocationPageViewModel : ContentPage
    {
        private Page Page { get; set; }
        public string SelectedItem;

        public LocationPageViewModel(Page view)
        {
            Page = view;
        }

        public void CreateAccount(string firstName, string lastName, string email, string password, string location){
            Account account = new Account(firstName, lastName, email, location);
            FirebaseBackend.SignUp(account, password);
        }
    }
}