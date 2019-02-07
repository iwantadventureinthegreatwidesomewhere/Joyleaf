using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;

namespace Joyleaf
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class EmailPage : ContentPage
    {
        private readonly EmailPageViewModel _viewModel;
        string firstName;
        string lastName;
        
        public EmailPage(string firstName, string lastName)
        {
            _viewModel = new EmailPageViewModel(this);
            BindingContext = _viewModel;
            this.firstName = firstName;
            this.lastName = lastName;

            InitializeComponent();

            EmailField.Completed += (object sender, EventArgs e) => Next_Click();
        }

        async private void Next_Click()
        {
            EmailField.Unfocus();

            if (!(string.IsNullOrEmpty(EmailField.Text)))
            {

                if (CrossConnectivity.Current.IsConnected)
                {

                    if (EmailField.VerifyText(EmailField.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                    {
                        try
                        {
                            if(FirebaseBackend.IsEmailAvailable(EmailField.Text))
                            {
                                await Navigation.PushAsync(new PasswordPage(firstName, lastName, EmailField.Text));
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Email is taken", "That email belongs to an existing account. Try another.", "OK");
                            }
                        }
                        catch(Exception)
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Whoops, looks like there is a problem on our end. Please try again later.", "OK");
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

        async private void Next_Click(object sender, EventArgs e)
        {

            if (CrossConnectivity.Current.IsConnected)
            {

                if (EmailField.VerifyText(EmailField.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    await Navigation.PushAsync(new PasswordPage(firstName, lastName, EmailField.Text));
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
