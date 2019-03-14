using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using System;
using Xamarin.Forms;

namespace Joyleaf.Views
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
                SignInButton.BackgroundColor = Color.FromHex("#00c88c");
                SignInButton.IsEnabled = true;
            }
            else
            {
                SignInButton.BackgroundColor = Color.FromHex("#4000c88c");
                SignInButton.IsEnabled = false;
            }
        }

        private void EntryFocus(object sender, FocusEventArgs e)
        {
            Logo.FadeTo(0, 200);
            SignInStack.TranslateTo(0, -70, 350, Easing.CubicInOut);
        }

        private void EntryUnfocus(object sender, FocusEventArgs e)
        {
            Logo.FadeTo(100, 200);
            SignInStack.TranslateTo(0, 0, 350, Easing.CubicOut);
        }

        protected override void OnAppearing()
        {
            if (!string.IsNullOrEmpty(Settings.FirebaseAuth))
            {
                FirebaseBackend.DeleteAuth();
            }
        }
    }
}