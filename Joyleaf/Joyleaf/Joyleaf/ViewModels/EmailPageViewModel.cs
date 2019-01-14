using Xamarin.Forms;

namespace Joyleaf
{
    
    public class EmailPageViewModel : ContentPage
    {
        private Page Page { get; set; }
        public string EmailAddress { get; set; }

        public EmailPageViewModel(Page view)
        {
            Page = view;
        }
    }
}

