using Joyleaf.Helpers;
using System;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class ContentItemView : ContentPage
    {
        public ContentItemView(Item item)
        {
            //NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}