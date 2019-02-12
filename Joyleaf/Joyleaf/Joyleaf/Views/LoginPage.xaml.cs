using Plugin.Connectivity;
using System;
using Xamarin.Forms;

namespace Joyleaf
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            SignInButton.CornerRadius = 23;

            EmailEntry.Completed += (object sender, EventArgs e) => PasswordEntry.Focus();
            PasswordEntry.Completed += SignInButtonClick;
        }

        private async void ForgotPasswordButtonClick(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await Navigation.PushAsync(new ForgotPasswordPage());
            }
            else
            {
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }

        private async void SignInButtonClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(EmailEntry.Text) && !string.IsNullOrEmpty(PasswordEntry.Text))
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    try
                    {
                        FirebaseBackend.SignIn(EmailEntry.Text, PasswordEntry.Text);
                    }
                    catch (Exception)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Whoops, looks like there is a problem on our end. Please try again later.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
                }
            }
        }

        private async void SignUpButtonClick(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await Navigation.PushAsync(new NamePage());
            }
            else
            {
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }

        private void TextChanged(object sender, EventArgs e)
        {

            if (!(string.IsNullOrEmpty(EmailEntry.Text)) && !(string.IsNullOrEmpty(PasswordEntry.Text)))
            {
                SignInButton.BackgroundColor = Color.FromHex("#23C7A5");
                SignInButton.IsEnabled = true;
            }
            else
            {
                SignInButton.BackgroundColor = Color.FromHex("#4023C7A5");
                SignInButton.IsEnabled = false;
            }
        }

        private void EntryOnFocus(object sender, FocusEventArgs e)
        {
            SignInStack.TranslateTo(0, -50, 400, Easing.CubicInOut);
        }

        private void EntryOffFocus(object sender, FocusEventArgs e)
        {
            SignInStack.TranslateTo(0, 0, 350, Easing.CubicOut);
        }

        protected override void OnAppearing()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Application.Current.MainPage.DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }
    }
}