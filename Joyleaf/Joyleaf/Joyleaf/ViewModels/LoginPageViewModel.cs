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
            FirebaseBackend.SignIn(Username, Password);
        }
    }
}