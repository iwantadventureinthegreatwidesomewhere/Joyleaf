using System;
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

            try
            {
                FirebaseBackend.SignUp(account, password);
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            catch(Exception)
            {
                Application.Current.MainPage.DisplayAlert("Incorrect password for " + email, "The password you entered is incorrect. Please try again.", "Try Again");
            }
        }
    }
}