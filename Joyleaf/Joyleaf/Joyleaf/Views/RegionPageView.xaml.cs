using Joyleaf.CustomControls;
using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using System;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class RegionPageView : GradientPage
    {
        private string email, password;
        private string selectedItem;

        public RegionPageView(string email, string password)
        {
            this.email = email;
            this.password = password;

            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void NextButtonClicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                string region = (string)RegionPicker.ItemsSource[RegionPicker.SelectedIndex];

                Account account = new Account(region);

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
            selectedItem = (string)RegionPicker.ItemsSource[RegionPicker.SelectedIndex];

            if (RegionPicker.SelectedIndex != -1)
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