using Joyleaf.CustomControls;
using Joyleaf.Helpers;
using Joyleaf.Services;
using System;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class StartPageView : GradientPage
    {
        public StartPageView()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
        }

        private async void SignUpButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EmailPageView());
        }

        private async void LogInButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LogInPageView());
        }

        protected override void OnAppearing()
        {
            if (!string.IsNullOrEmpty(Settings.FirebaseAuth))
            {
                FirebaseBackend.DeleteAuth();
            }
        }
    }
}