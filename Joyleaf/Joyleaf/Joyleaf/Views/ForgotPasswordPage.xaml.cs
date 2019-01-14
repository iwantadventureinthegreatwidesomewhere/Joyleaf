using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using Firebase.Auth;

using Xamarin.Forms;

namespace Joyleaf
{
    public partial class ForgotPasswordPage : ContentPage
    {
        private readonly ForgotPasswordPageViewModel _viewModel;

        public ForgotPasswordPage()
        {
            _viewModel = new ForgotPasswordPageViewModel(this);
            BindingContext = _viewModel;

            InitializeComponent();

            EmailField.Completed += (object sender, EventArgs e) => Send_Click();
        }

        async private void Send_Click()
        {
            EmailField.Unfocus();

            if (!(string.IsNullOrEmpty(EmailField.Text)))
            {

                if (CrossConnectivity.Current.IsConnected)
                {

                    if (EmailField.verifyEmail(EmailField.Text))
                    {

                        if(_viewModel.SendForgotPassword()){
                            await Navigation.PopToRootAsync();
                        }
                    }
                    else
                    {
                        await DisplayAlert("Invalid email", "The email address you entered is invalid. Please try again.", "Try Again");
                    }
                }
                else
                {
                    await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
                }
            }
        }

        async private void Send_Click(object sender, EventArgs e)
        {

            if (CrossConnectivity.Current.IsConnected)
            {

                if (EmailField.verifyEmail(EmailField.Text))
                {

                }
                else
                {
                    await DisplayAlert("Invalid email", "The email address you entered is invalid. Please try again.", "Try Again");
                }
            }
            else
            {
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }

        private void TextfieldChanged(object sender, EventArgs e)
        {

            if (!(string.IsNullOrEmpty(EmailField.Text)))
            {
                btnSend.BackgroundColor = Color.FromHex("#00b1b0");
                btnSend.IsEnabled = true;
            }
            else
            {
                btnSend.BackgroundColor = Color.FromHex("#4000b1b0");
                btnSend.IsEnabled = false;
            }
        }
    }
}
