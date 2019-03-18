using Joyleaf.CustomControls;
using Plugin.Connectivity;
using System;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class PasswordPageView : GradientPage
    {
        private string email;

        public PasswordPageView(string email)
        {
            this.email = email;

            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            PasswordEntry.Completed += (object sender, EventArgs e) => ConfirmPasswordEntry.Focus();
            ConfirmPasswordEntry.Completed += NextButtonClicked;
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void NextButtonClicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (PasswordEntry.VerifyText(@"^[ -~]+$") && ConfirmPasswordEntry.VerifyText(@"^[ -~]+$"))
                {
                    if (string.Equals(PasswordEntry.Text, ConfirmPasswordEntry.Text))
                    {
                        int count = PasswordEntry.Text.Length;

                        if (count >= 8)
                        {
                            await Navigation.PushAsync(new RegionPageView(email, PasswordEntry.Text));
                        }
                        else
                        {
                            await DisplayAlert("Choose a stronger password", "Make sure to use at least eight characters. Please try again.", "Try Again");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Passwords do not match", "The passwords you entered do not match. Please try again.", "Try Again");
                    }
                }
                else
                {
                    await DisplayAlert("Invalid password", "The password you entered is invalid. Please try again.", "Try Again");
                }
            }
            else
            {
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }

        private void TextChanged(object sender, EventArgs e)
        {

            if (!(string.IsNullOrEmpty(PasswordEntry.Text)) && !(string.IsNullOrEmpty(ConfirmPasswordEntry.Text)))
            {
                NextButton.IsEnabled = true;
            }
            else
            {
                NextButton.IsEnabled = false;
            }
        }
    }
}