using Joyleaf.CustomControls;
using Joyleaf.Services;
using Plugin.Connectivity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Joyleaf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class EmailPageView : GradientPage
    {
        public EmailPageView()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            EmailEntry.Completed += NextButtonClicked;
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void NextButtonClicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (EmailEntry.VerifyText(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    try
                    {
                        if (FirebaseBackend.IsEmailAvailable(EmailEntry.Text))
                        {
                            await Navigation.PushAsync(new PasswordPageView(EmailEntry.Text));
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Email is taken", "That email belongs to an existing account. Try another.", "OK");
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
            if (!string.IsNullOrEmpty(EmailEntry.Text))
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