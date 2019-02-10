using System;
using Xamarin.Forms;

namespace Joyleaf
{
    public class LoginPageViewModel : MainPageViewModel
    {
        private Page Page { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public LoginPageViewModel(Page view)
        {
            Page = view;
        }

        public void Login()
        {
            try
            {
                FirebaseBackend.SignIn(Username, Password);
            }
            catch(Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Whoops, looks like there is a problem on our end. Please try again later.", "OK");
            }
        }
    }
}