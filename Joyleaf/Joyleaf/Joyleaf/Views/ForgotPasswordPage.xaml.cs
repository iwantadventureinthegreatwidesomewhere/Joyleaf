using Plugin.Connectivity;
using System;
using Xamarin.Forms;

namespace Joyleaf
{
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();

            EmailEntry.Completed += SendButtonClick;
        }

        private async void SendButtonClick(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (EmailEntry.VerifyText(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    try
                    {
                        FirebaseBackend.SendPasswordReset(EmailEntry.Text);
                        await Application.Current.MainPage.Navigation.PopToRootAsync();
                    }
                    catch (Exception)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Whoops, looks like there is a problem on our end. Please try again later.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Invalid email", "The email address you entered is invalid. Please try again.", "Try Again");
                }
            }
            else
            {
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }

        private void TextChanged(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(EmailEntry.Text)))
            {
                SendButton.BackgroundColor = Color.FromHex("#00b1b0");
                SendButton.IsEnabled = true;
            }
            else
            {
                SendButton.BackgroundColor = Color.FromHex("#4000b1b0");
                SendButton.IsEnabled = false;
            }
        }
    }
}