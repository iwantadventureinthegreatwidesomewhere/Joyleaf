using Joyleaf.CustomControls;
using Joyleaf.Services;
using Plugin.Connectivity;
using System;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class ForgotPasswordPageView :GradientPage
    {
        public ForgotPasswordPageView()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            EmailEntry.Completed += SendButtonClick;
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SendButtonClick(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (EmailEntry.VerifyText(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    try
                    {
                        bool status = FirebaseBackend.SendPasswordReset(EmailEntry.Text);

                        if (status)
                        {
                            await Application.Current.MainPage.Navigation.PopAsync();
                        }
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
                SendButton.IsEnabled = true;
            }
            else
            {
                SendButton.IsEnabled = false;
            }
        }
    }
}