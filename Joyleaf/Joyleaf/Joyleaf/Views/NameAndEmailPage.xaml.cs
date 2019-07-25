using Joyleaf.CustomControls;
using Joyleaf.Services;
using Plugin.Connectivity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Joyleaf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class NameAndEmailPage : GradientPage
    {
        public NameAndEmailPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            NameEntry.Completed += (object sender, EventArgs e) => EmailEntry.Focus();
            EmailEntry.Completed += NextButtonClicked;
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void NextButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(NameEntry.Text))
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (EmailEntry.VerifyText(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                    {
                        try
                        {
                            if (FirebaseBackend.IsEmailAvailable(EmailEntry.Text))
                            {
                                await Navigation.PushAsync(new PasswordPage(NameEntry.Text, EmailEntry.Text));
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Email is taken", "That email belongs to an existing account. Try another.", "OK");
                            }
                        }
                        catch (Exception)
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Whoops, looks like there's a problem on our end. Please try again later.", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Invalid email", "The email address you entered is invalid. Please try again.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
                }
            }
            else
            {
                NameEntry.Focus();
            }
        }

        private void TextChanged(object sender, EventArgs e)
        {
            NextButton.IsEnabled = !string.IsNullOrEmpty(NameEntry.Text) && !string.IsNullOrEmpty(EmailEntry.Text);
        }
    }
}
