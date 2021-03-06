﻿using Joyleaf.CustomControls;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class PasswordPage : GradientPage
    {
        private readonly string name, email;

        public PasswordPage(string name, string email)
        {
            this.name = name;
            this.email = email;

            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            PasswordEntry.Completed += (object sender, EventArgs e) => ConfirmPasswordEntry.Focus();
            ConfirmPasswordEntry.Completed += NextButtonClicked;
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void NextButtonClicked(object sender, EventArgs e)
        {
            NextButton.IsBusy = true;

            await Task.Delay(250);

            if (!string.IsNullOrEmpty(PasswordEntry.Text))
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (PasswordEntry.VerifyText(@"^[ -~]+$") && ConfirmPasswordEntry.VerifyText(@"^[ -~]+$"))
                    {
                        if (string.Equals(PasswordEntry.Text, ConfirmPasswordEntry.Text))
                        {
                            int count = PasswordEntry.Text.Length;

                            if (count >= 8)
                            {
                                await Navigation.PushAsync(new TermsPage(name, email, PasswordEntry.Text));
                                NextButton.IsBusy = false;
                            }
                            else
                            {
                                NextButton.IsBusy = false;
                                await DisplayAlert("Choose a stronger password", "Make sure to use at least eight characters. Please try again.", "OK");
                            }
                        }
                        else
                        {
                            NextButton.IsBusy = false;
                            await DisplayAlert("Passwords do not match", "The passwords you entered do not match. Please try again.", "OK");
                        }
                    }
                    else
                    {
                        NextButton.IsBusy = false;
                        await DisplayAlert("Invalid password", "The password you entered is invalid. Please try again.", "OK");
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
                PasswordEntry.Focus();
            }
        }

        private void TextChanged(object sender, EventArgs e)
        {

            NextButton.IsEnabled = !string.IsNullOrEmpty(PasswordEntry.Text) && !string.IsNullOrEmpty(ConfirmPasswordEntry.Text);
        }
    }
}
