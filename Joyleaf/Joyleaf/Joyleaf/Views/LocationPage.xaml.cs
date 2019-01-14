using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Joyleaf
{
    public partial class LocationPage : ContentPage
    {
        private readonly LocationPageViewModel _viewModel;
        string firstName;
        string lastName;
        string email;
        string password;

        public LocationPage(string firstName, string lastName, string email, string password)
        {
            _viewModel = new LocationPageViewModel(this);
            BindingContext = _viewModel;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.password = password;

            InitializeComponent();
        }

        async void Next_Click(object sender, EventArgs e)
        {

            if(CrossConnectivity.Current.IsConnected){
                string location = (string)locationPicker.ItemsSource[locationPicker.SelectedIndex];
                _viewModel.CreateAccount(firstName, lastName, email, password, location);
            }
            else
            {
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }

        private void PickerChanged(object sender, EventArgs e)
        {
            _viewModel.SelectedItem = (string)locationPicker.ItemsSource[locationPicker.SelectedIndex];

            if (locationPicker.SelectedIndex != -1)
            {
                btnNext.BackgroundColor = Color.FromHex("#00b1b0");
                btnNext.IsEnabled = true;
            }
            else
            {
                btnNext.BackgroundColor = Color.FromHex("#4000b1b0");
                btnNext.IsEnabled = false;
            }
        }
    }
}
