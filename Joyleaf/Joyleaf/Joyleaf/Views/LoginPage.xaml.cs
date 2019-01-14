using System;
using Xamarin.Forms;
using Plugin.Connectivity;

namespace Joyleaf
{
    
	public partial class LoginPage : ContentPage
	{
	    private readonly LoginPageViewModel _viewModel;

        public LoginPage()
        {
            _viewModel = new LoginPageViewModel(this);
            BindingContext = _viewModel;

            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
            //App.Current.MainPage.DisplayAlert("sad", "asd", "sads");

            UsernameField.Completed += (object sender, EventArgs e) => PasswordField.Focus();
		    PasswordField.Completed += (object sender, EventArgs e) => LogIn_Click();
        }

        async private void LogIn_Click()
        {
            PasswordField.Unfocus();

            if (!(string.IsNullOrEmpty(UsernameField.Text)) && !(string.IsNullOrEmpty(PasswordField.Text)))
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    _viewModel.Login();
                }
                else
                {
                    await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
                }
            }
        }

		async private void LogIn_Click(object sender, EventArgs e)
	    {
            if (CrossConnectivity.Current.IsConnected)
            {
                _viewModel.Login();
            }
            else
            {
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
	    }

        async private void ForgotPassword_Click(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await Navigation.PushAsync(new ForgotPasswordPage());
            }
            else
            {
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }

        async private void SignUp_Click(object sender, EventArgs e)
		{

            if (CrossConnectivity.Current.IsConnected)
            {
                await Navigation.PushAsync(new NamePage());
            }
            else
            {
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
		}

	    private void VisualElement_OnFocused(object sender, FocusEventArgs e)
	    {
            SigninContent.TranslateTo(0, -50, 400, Easing.CubicInOut);
        }

        private void VisualElement_OffFocused(object sender, FocusEventArgs e)
        {
            SigninContent.TranslateTo(0, 0, 350, Easing.CubicOut);
        }

        private void TextfieldChanged(object sender, EventArgs e)
        {

            if (!(string.IsNullOrEmpty(UsernameField.Text)) && !(string.IsNullOrEmpty(PasswordField.Text)))
            {
                btnLogIn.BackgroundColor = Color.FromHex("#00b1b0");
                btnLogIn.IsEnabled = true;
            }
            else
            {
                btnLogIn.BackgroundColor = Color.FromHex("#4000b1b0");
                btnLogIn.IsEnabled = false;
            }
        }

        protected override void OnAppearing()
        {
            if(!CrossConnectivity.Current.IsConnected){
                App.Current.MainPage.DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }
    }
}
