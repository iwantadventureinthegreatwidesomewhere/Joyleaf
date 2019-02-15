using Plugin.Connectivity;
using System;
using Xamarin.Forms;

namespace Joyleaf
{
    public partial class SignInPageView : ContentPage
    {
        public SignInPageView()
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
                await Navigation.PushAsync(new ForgotPasswordPageView());
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
                await Navigation.PushAsync(new NamePageView());
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

        protected override void OnAppearing()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Application.Current.MainPage.DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
            else if (!String.IsNullOrEmpty(Settings.FirebaseAuth) && !FirebaseBackend.IsSavedAuthValid())
            {
                Application.Current.MainPage.DisplayAlert("You've been signed out", "The account owner may have changed the password.", "OK");
            }
        }
    }
}