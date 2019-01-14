using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;

namespace Joyleaf
{
	[XamlCompilation(XamlCompilationOptions.Compile)]

	public partial class NamePage : ContentPage
	{
        private readonly NamePageViewModel _viewModel;
        
		public NamePage ()
		{
            _viewModel = new NamePageViewModel(this);
            BindingContext = _viewModel;

			InitializeComponent ();

            FirstNameField.Completed += (object sender, EventArgs e) => LastNameField.Focus();
            LastNameField.Completed += (object sender, EventArgs e) => Next_Click();
		}

        async private void Next_Click()
        {
            LastNameField.Unfocus();

            if (!(string.IsNullOrEmpty(FirstNameField.Text)) && !(string.IsNullOrEmpty(LastNameField.Text)))
            {

                if (CrossConnectivity.Current.IsConnected)
                {

                    if (FirstNameField.verifyText(FirstNameField.Text) && LastNameField.verifyText(LastNameField.Text))
                    {
                        await Navigation.PushAsync(new EmailPage(FirstNameField.Text, LastNameField.Text));
                    }
                    else
                    {
                        await DisplayAlert("Invalid name", "The name you entered is invalid. Please try again.", "Try Again");
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

                if (FirstNameField.verifyText(FirstNameField.Text) && LastNameField.verifyText(LastNameField.Text))
                {
                    await Navigation.PushAsync(new EmailPage(FirstNameField.Text, LastNameField.Text));
                }
                else
                {
                    await DisplayAlert("Invalid name", "The name you entered is invalid. Please try again.", "Try Again");
                }
            }
            else
            {
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }

        private void TextfieldChanged(object sender, EventArgs e)
        {
            
            if (!(string.IsNullOrEmpty(FirstNameField.Text)) && !(string.IsNullOrEmpty(LastNameField.Text)))
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