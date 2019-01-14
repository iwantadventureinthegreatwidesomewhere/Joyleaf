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

        public bool SendForgotPassword(){
            return FirebaseBackend.ForgotPassword(EmailAddress);
        }
    }
}

