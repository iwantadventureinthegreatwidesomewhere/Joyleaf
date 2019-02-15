using Plugin.Connectivity;
using System;
using Xamarin.Forms;

namespace Joyleaf
{
    public partial class LocationPage : ContentPage
    {
        private string firstName, lastName, email, password;
        private string selectedItem;

        public LocationPage(string firstName, string lastName, string email, string password)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.password = password;

            InitializeComponent();

            NextButton.CornerRadius = 23;
        }

        private async void NextButtonClick(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                string location = (string)LocationPicker.ItemsSource[LocationPicker.SelectedIndex];

                Account account = new Account(firstName, lastName, location);

                try
                {
                    FirebaseBackend.SignUp(account, email, password);
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

        private void PickerChanged(object sender, EventArgs e)
        {
            selectedItem = (string)LocationPicker.ItemsSource[LocationPicker.SelectedIndex];

            if (LocationPicker.SelectedIndex != -1)
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