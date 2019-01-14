using Xamarin.Forms;

namespace Joyleaf
{
    
    public class NamePageViewModel : MainPageViewModel
    {
        private Page Page { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public NamePageViewModel(Page view)
        {
            Page = view;
        }
    }
}