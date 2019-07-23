using System;
using Joyleaf.CustomControls;
using Joyleaf.Services;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class LogInPage : GradientPage
    {
        public LogInPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            EmailEntry.Completed += (object sender, EventArgs e) => PasswordEntry.Focus();             PasswordEntry.Completed += LogInButtonClicked;
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void LogInButtonClicked(object sender, EventArgs e)         {             if (!string.IsNullOrEmpty(EmailEntry.Text))             {                 if (CrossConnectivity.Current.IsConnected)                 {                     try                     {                         FirebaseBackend.SignIn(EmailEntry.Text, PasswordEntry.Text);                     }                     catch (Exception)                     {                         await Application.Current.MainPage.DisplayAlert("Error", "Whoops, looks like there's a problem on our end. Please try again later.", "OK");                     }                 }                 else                 {                     await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");                 }             }
            else
            {
                EmailEntry.Focus();
            }         }

        private async void ForgotPasswordButtonClicked(object sender, EventArgs e)         {             await Navigation.PushAsync(new ForgotPasswordPage());         }

        private void TextChanged(object sender, EventArgs e)         {             LogInButton.IsEnabled = !string.IsNullOrEmpty(EmailEntry.Text) && !string.IsNullOrEmpty(PasswordEntry.Text);         }
    }
}