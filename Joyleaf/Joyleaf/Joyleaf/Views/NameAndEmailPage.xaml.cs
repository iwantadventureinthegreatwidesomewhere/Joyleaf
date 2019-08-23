using Joyleaf.CustomControls;
using Joyleaf.Services;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
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
            NextButton.IsBusy = true;

            await Task.Delay(250);

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
                                NextButton.IsBusy = false;
                            }
                            else
                            {
                                NextButton.IsBusy = false;
                                await DisplayAlert("Email is taken", "That email belongs to an existing account. Try another.", "OK");
                            }
                        }
                        catch (Exception)
                        {
                            NextButton.IsBusy = false;
                            await DisplayAlert("Error", "Whoops, looks like there's a problem. Please try again later.", "OK");
                        }
                    }
                    else
                    {
                        NextButton.IsBusy = false;
                        await DisplayAlert("Invalid email", "The email address you entered is invalid. Please try again.", "OK");
                    }
                }
                else
                {
                    NextButton.IsBusy = false;
                    await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
                }
            }
            else
            {
                NextButton.IsBusy = false;
                NameEntry.Focus();
            }
        }

        private void TextChanged(object sender, EventArgs e)
        {
            NextButton.IsEnabled = !string.IsNullOrEmpty(NameEntry.Text) && !string.IsNullOrEmpty(EmailEntry.Text);
        }
    }
}
