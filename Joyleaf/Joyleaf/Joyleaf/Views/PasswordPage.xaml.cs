using System;
using Xamarin.Forms;
using Plugin.Connectivity;

namespace Joyleaf
{
    
    public partial class PasswordPage : ContentPage
    {
        private readonly PasswordPageViewModel _viewModel;
        string firstName;
        string lastName;
        string email;

        public PasswordPage(string firstName, string lastName, string email)
        {
            _viewModel = new PasswordPageViewModel(this);
            BindingContext = _viewModel;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;

            InitializeComponent();

            PasswordField.Completed += (object sender, EventArgs e) => ConfirmPasswordField.Focus();
            ConfirmPasswordField.Completed += (object sender, EventArgs e) => Next_Click();
        }

        private void Next_Click()
        {
            ConfirmPasswordField.Unfocus();

            if (!(string.IsNullOrEmpty(PasswordField.Text)) && !(string.IsNullOrEmpty(ConfirmPasswordField.Text)))
            {
                CheckPassword();
            }
        }

        private void Next_Click(object sender, EventArgs e)
        {
            CheckPassword();
        }

        private void TextfieldChanged(object sender, EventArgs e)
        {
            
            if (!(string.IsNullOrEmpty(PasswordField.Text)) && !(string.IsNullOrEmpty(ConfirmPasswordField.Text)))
            {
                btnNext.BackgroundColor = Color.FromHex("#00b1b0");
                btnNext.IsEnabled = true;
            }
            else
            {
                btnNext.BackgroundColor = Color.FromHex("#4000b1b0");
                btnNext.IsEnabled = false;
            }
        }

        async private void CheckPassword()
        {

            if (CrossConnectivity.Current.IsConnected)
            {

                if (PasswordField.VerifyText(PasswordField.Text, @"^[ -~]+$") && ConfirmPasswordField.VerifyText(ConfirmPasswordField.Text, @"^[ -~]+$"))
                {

                    if (string.Equals(PasswordField.Text, ConfirmPasswordField.Text))
                    {
                        var count = 0;
                        bool hasUppercase = false;
                        bool hasLowercase = false;
                        bool hasNumber = false;

                        foreach (char c in PasswordField.Text)
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
                            await Navigation.PushAsync(new LocationPage(firstName, lastName, email, PasswordField.Text));
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
    }
}
