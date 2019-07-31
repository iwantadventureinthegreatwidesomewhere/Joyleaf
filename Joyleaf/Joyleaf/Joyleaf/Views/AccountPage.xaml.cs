using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            CrossConnectivity.Current.ConnectivityChanged += HandleConnectivityChanged;
        }

        private async void HandleConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await Navigation.PopAsync();
            }
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
