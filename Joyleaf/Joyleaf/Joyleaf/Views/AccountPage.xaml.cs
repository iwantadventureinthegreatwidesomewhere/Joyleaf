using System;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
