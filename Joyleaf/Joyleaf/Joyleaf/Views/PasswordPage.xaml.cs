using Plugin.Connectivity;
using System;
using Xamarin.Forms;

namespace Joyleaf
{
    public partial class PasswordPage : ContentPage
    {
        private string firstName, lastName, email;

        public PasswordPage(string firstName, string lastName, string email)
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
                        var count = 0;
                        bool hasUppercase = false;
                        bool hasLowercase = false;
                        bool hasNumber = false;

                        foreach (char c in PasswordEntry.Text)
                        {
                            if ('A' <= c && c <= 'Z')
                            {
                                hasUppercase = true;
                            }

                            if ('a' <= c && c <= 'z')
                            {
                                hasLowercase = true;
                            }

                            if ('0' <= c && c <= '9')
                            {
                                hasNumber = true;
                            }

                            count++;
                        }

                        if (count >= 6 && hasUppercase && hasLowercase && hasNumber)
                        {
                            await Navigation.PushAsync(new LocationPage(firstName, lastName, email, PasswordEntry.Text));
                        }
                        else
                        {
                            await DisplayAlert("Choose a stronger password", "Make sure to use at least six characters, one uppercase letter, " +
                                               "one lowercase letter, and one number. Please try again.", "Try Again");
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
                NextButton.BackgroundColor = Color.FromHex("#23C7A5");
                NextButton.IsEnabled = true;
            }
            else
            {
                NextButton.BackgroundColor = Color.FromHex("#4023C7A5");
                NextButton.IsEnabled = false;
            }
        }
    }
}