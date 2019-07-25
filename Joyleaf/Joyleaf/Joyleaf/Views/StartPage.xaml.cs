using Joyleaf.CustomControls;
using Joyleaf.Helpers;
using System;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class StartPage : GradientPage
    {
        public StartPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
        }

        private async void SignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NameAndEmailPage());
        }

        private async void LogInButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LogInPage());
        }

        protected override void OnAppearing()
        {
            if (!string.IsNullOrEmpty(Settings.FirebaseAuth))
            {
                Settings.ResetSettings();
            }
        }
    }
}
