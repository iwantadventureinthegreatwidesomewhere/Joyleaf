using System;
using Xamarin.Forms;

namespace Joyleaf
{
    public class ForgotPasswordPageViewModel : ContentPage
    {
        private Page Page { get; set; }

        public string EmailAddress { get; set; }

        public ForgotPasswordPageViewModel(Page view)
        {
            Page = view;
        }

        public void SendPasswordReset()
        {
            try
            {
                FirebaseBackend.SendPasswordReset(EmailAddress);
                Application.Current.MainPage.Navigation.PopToRootAsync();
            }
            catch(Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Whoops, looks like there is a problem on our end. Please try again later.", "OK");
            }
        }
    }
}

