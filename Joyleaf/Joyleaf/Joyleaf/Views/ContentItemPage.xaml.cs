using System;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class ContentItemPage : ContentPage
    {
        public ContentItemPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
        }
    }
}
