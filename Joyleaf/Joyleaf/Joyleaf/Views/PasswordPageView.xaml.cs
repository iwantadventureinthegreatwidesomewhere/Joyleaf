using Plugin.Connectivity;
using System;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class PasswordPageView : ContentPage
    {
        private string firstName, lastName, email;

        public PasswordPageView(string firstName, string lastName, string email)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;

            InitializeComponent();

            NextButton.CornerRadius = 23;

            PasswordEntry.Completed += (object sender, EventArgs e) => ConfirmPasswordEntry.Focus();
            ConfirmPasswordEntry.Completed += NextButtonClick;
        }

        private async void NextButtonClick(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (PasswordEntry.VerifyText(@"^[ -~]+$") && ConfirmPasswordEntry.VerifyText(@"^[ -~]+$"))
                {
                    if (string.Equals(PasswordEntry.Text, ConfirmPasswordEntry.Text))
                    {
                        int count = PasswordEntry.Text.Length;

                        if (count >= 6)
                        {
                            await Navigation.PushAsync(new LocationPageView(firstName, lastName, email, PasswordEntry.Text));
                        }
                        else
                        {
                            await DisplayAlert("Choose a stronger password", "Make sure to use at least six characters. Please try again.", "Try Again");
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
                NextButton.BackgroundColor = Color.FromHex("#00c88c");
                NextButton.IsEnabled = true;
            }
            else
            {
                NextButton.BackgroundColor = Color.FromHex("#4000c88c");
                NextButton.IsEnabled = false;
            }
        }
    }
}