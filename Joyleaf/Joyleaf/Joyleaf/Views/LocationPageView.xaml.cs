using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using System;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class LocationPageView : ContentPage
    {
        private string firstName, lastName, email, password;
        private string selectedItem;

        public LocationPageView(string firstName, string lastName, string email, string password)
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
                NextButton.BackgroundColor = Color.FromHex("#00c88c");
                NextButton.IsEnabled = true;
            }
            else
            {
                NextButton.BackgroundColor = Color.FromHex("#4000c88c");
                NextButton.IsEnabled = false;
            }
        }
    }
}