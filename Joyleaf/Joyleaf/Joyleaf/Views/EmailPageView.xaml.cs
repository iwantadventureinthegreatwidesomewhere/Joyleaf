using Joyleaf.Services;
using Plugin.Connectivity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Joyleaf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class EmailPageView : ContentPage
    {
        private string firstName, lastName;

        public EmailPageView(string firstName, string lastName)
        {
            this.firstName = firstName;
            this.lastName = lastName;

            InitializeComponent();

            NextButton.CornerRadius = 23;

            EmailEntry.Completed += NextButtonClick;
        }

        private async void NextButtonClick(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (EmailEntry.VerifyText(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    try
                    {
                        if (FirebaseBackend.IsEmailAvailable(EmailEntry.Text))
                        {
                            await Navigation.PushAsync(new PasswordPageView(firstName, lastName, EmailEntry.Text));
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
            if (!(string.IsNullOrEmpty(EmailEntry.Text)))
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