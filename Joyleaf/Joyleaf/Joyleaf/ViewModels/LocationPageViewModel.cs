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
            NewAccount account = new NewAccount();
            account.firstName = firstName;
            account.lastName = lastName;
            account.email = email;
            account.location = location;
            FirebaseBackend.SignUp(account, password);
        }
    }
}