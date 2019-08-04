using Joyleaf.CustomControls;
using Joyleaf.Services;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class ForgotPasswordPage :GradientPage
    {
        public ForgotPasswordPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            EmailEntry.Completed += SendButtonClicked;
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SendButtonClicked(object sender, EventArgs e)
        {
            SendButton.IsBusy = true;

            await Task.Delay(250);

            if (CrossConnectivity.Current.IsConnected)
            {
                if (EmailEntry.VerifyText(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    try
                    {
                        FirebaseBackend.SendPasswordReset(EmailEntry.Text, true);
                        SendButton.IsBusy = false;
                    }
                    catch (Exception)
                    {
                        SendButton.IsBusy = false;
                        await DisplayAlert("Error", "Whoops, looks like there's a problem on our end. Please try again later.", "OK");
                    }
                }
                else
                {
                    SendButton.IsBusy = false;
                    await DisplayAlert("Invalid email", "The email address you entered is invalid. Please try again.", "OK");
                }
            }
            else
            {
                SendButton.IsBusy = false;
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }

        private void TextChanged(object sender, EventArgs e)
        {
            SendButton.IsEnabled = !string.IsNullOrEmpty(EmailEntry.Text);
        }
    }
}
