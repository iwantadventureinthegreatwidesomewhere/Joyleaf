using Plugin.Connectivity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Joyleaf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class NamePageView : ContentPage
    {
        public NamePageView()
        {
            InitializeComponent();

            NextButton.CornerRadius = 23;

            FirstNameEntry.Completed += (object sender, EventArgs e) => LastNameEntry.Focus();
            LastNameEntry.Completed += NextButtonClick;
        }

        private async void NextButtonClick(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (FirstNameEntry.VerifyText(@"^[ -~]+$") && LastNameEntry.VerifyText(@"^[ -~]+$"))
                {
                    await Navigation.PushAsync(new EmailPageView(FirstNameEntry.Text, LastNameEntry.Text));
                }
                else
                {
                    await DisplayAlert("Invalid name", "The name you entered is invalid. Please try again.", "Try Again");
                }
            }
            else
            {
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }

        private void TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(FirstNameEntry.Text) && !string.IsNullOrEmpty(LastNameEntry.Text))
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