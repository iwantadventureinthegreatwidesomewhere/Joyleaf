using Xamarin.Forms;

namespace Joyleaf
{
    
    public class PasswordPageViewModel : ContentPage
    {
        private Page Page { get; set; }
        public string Password { get; set; }

        public PasswordPageViewModel(Page view)
        {
            Page = view;
        }
    }
}