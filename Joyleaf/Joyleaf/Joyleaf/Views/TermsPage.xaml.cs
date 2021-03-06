﻿using Joyleaf.CustomControls;
using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class TermsPage : GradientPage
    {
        private readonly string name, email, password;

        public TermsPage(string name, string email, string password)
        {
            this.name = name;
            this.email = email;
            this.password = password;

            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            TapGestureRecognizer TermsSpanTap = new TapGestureRecognizer();
            TermsSpanTap.Tapped += (s, e) => {
                Device.OpenUri(new Uri("http://joyleaf.ca/terms"));
            };

            TermsSpan.GestureRecognizers.Add(TermsSpanTap);

            TapGestureRecognizer PrivacySpanTap = new TapGestureRecognizer();
            PrivacySpanTap.Tapped += (s, e) => {
                Device.OpenUri(new Uri("http://joyleaf.ca/privacy"));
            };

            PrivacySpan.GestureRecognizers.Add(PrivacySpanTap);
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void NextButtonClicked(object sender, EventArgs e)
        {
            NextButton.IsBusy = true;

            await Task.Delay(250);

            if (CrossConnectivity.Current.IsConnected)
            {
                Account account = new Account(name, DateTime.Now.ToString("MM/dd/yyyy"));

                try
                {
                    FirebaseBackend.SignUp(account, email, password);
                    NextButton.IsBusy = false;
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
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }
    }
}
