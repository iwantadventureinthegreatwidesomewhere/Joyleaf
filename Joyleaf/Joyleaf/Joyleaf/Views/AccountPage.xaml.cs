using System;
using Joyleaf.Helpers;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            StackLayout MenuStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical
            };

            MenuStack.Children.Add(new BoxView
            {
                Color = Color.LightGray,
                HeightRequest = 0.5,
                HorizontalOptions = LayoutOptions.FillAndExpand
            });

            StackLayout ChangePasswordStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal
            };

            ChangePasswordStack.Children.Add(new Image
            {
                HeightRequest = 25,
                Margin = new Thickness(25, 0, 7, 0),
                Source = "ChangePassword",
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 25
            });

            ChangePasswordStack.Children.Add(new Label
            {
                FontSize = 17,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 10),
                Text = "Change Password"
            });

            ChangePasswordStack.Children.Add(new Image
            {
                HeightRequest = 15,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(0, 0, 25, 0),
                Source = "AccountArrow",
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 15
            });

            TapGestureRecognizer ChangePasswordTap = new TapGestureRecognizer();
            ChangePasswordTap.Tapped += (sender, e) =>
            {
                
            };

            ChangePasswordStack.GestureRecognizers.Add(ChangePasswordTap);

            MenuStack.Children.Add(ChangePasswordStack);

            MenuStack.Children.Add(new BoxView
            {
                Color = Color.LightGray,
                HeightRequest = 0.5,
                HorizontalOptions = LayoutOptions.FillAndExpand
            });

            StackLayout ManageReviewsStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal
            };

            ManageReviewsStack.Children.Add(new Image
            {
                HeightRequest = 25,
                Margin = new Thickness(25, 0, 7, 0),
                Source = "EditReviews",
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 25
            });

            ManageReviewsStack.Children.Add(new Label
            {
                FontSize = 17,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 10),
                Text = "Manage My Reviews"
            });

            ManageReviewsStack.Children.Add(new Image
            {
                HeightRequest = 15,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(0, 0, 25, 0),
                Source = "AccountArrow",
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 15
            });

            TapGestureRecognizer ManageReviewsTap = new TapGestureRecognizer();
            ManageReviewsTap.Tapped += (sender, e) =>
            {

            };

            ManageReviewsStack.GestureRecognizers.Add(ManageReviewsTap);

            MenuStack.Children.Add(ManageReviewsStack);

            MenuStack.Children.Add(new BoxView
            {
                Color = Color.LightGray,
                HeightRequest = 0.5,
                HorizontalOptions = LayoutOptions.FillAndExpand
            });

            StackLayout ContactSupportStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal
            };

            ContactSupportStack.Children.Add(new Image
            {
                HeightRequest = 25,
                Margin = new Thickness(25, 0, 7, 0),
                Source = "ContactSupport",
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 25
            });

            ContactSupportStack.Children.Add(new Label
            {
                FontSize = 17,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 10),
                Text = "Contact Support"
            });

            ContactSupportStack.Children.Add(new Image
            {
                HeightRequest = 15,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(0, 0, 25, 0),
                Source = "AccountArrow",
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 15
            });

            TapGestureRecognizer ContactSupportTap = new TapGestureRecognizer();
            ContactSupportTap.Tapped += (sender, e) =>
            {
                Device.OpenUri(new Uri("mailto:support@joyleaf.ca"));
            };

            ContactSupportStack.GestureRecognizers.Add(ContactSupportTap);

            MenuStack.Children.Add(ContactSupportStack);

            MenuStack.Children.Add(new BoxView
            {
                Color = Color.LightGray,
                HeightRequest = 0.5,
                HorizontalOptions = LayoutOptions.FillAndExpand
            });

            StackLayout TermsStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal
            };

            TermsStack.Children.Add(new Image
            {
                HeightRequest = 25,
                Margin = new Thickness(25, 0, 7, 0),
                Source = "Terms",
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 25
            });

            TermsStack.Children.Add(new Label
            {
                FontSize = 17,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 10),
                Text = "Terms of Use & Privacy"
            });

            TermsStack.Children.Add(new Image
            {
                HeightRequest = 15,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(0, 0, 25, 0),
                Source = "AccountArrow",
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 15
            });

            TapGestureRecognizer TermsTap = new TapGestureRecognizer();
            TermsTap.Tapped += (sender, e) =>
            {

            };

            TermsStack.GestureRecognizers.Add(TermsTap);

            MenuStack.Children.Add(TermsStack);

            MenuStack.Children.Add(new BoxView
            {
                Color = Color.LightGray,
                HeightRequest = 0.5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0, 0, 30)
            });

            MenuStack.Children.Add(new BoxView
            {
                Color = Color.LightGray,
                HeightRequest = 0.5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            });

            StackLayout LogoutStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal
            };

            LogoutStack.Children.Add(new Label
            {
                FontSize = 17,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 10),
                Text = "Sign Out",
                TextColor = Color.FromHex("#EC5B55")
            });

            TapGestureRecognizer LogoutTap = new TapGestureRecognizer();
            LogoutTap.Tapped += async (sender, e) =>
            {
                bool answer = await DisplayAlert("Are you sure you want to sign out?", "", "Yes", "Cancel");
                if (answer)
                {
                    Logout();
                }
            };

            LogoutStack.GestureRecognizers.Add(LogoutTap);

            MenuStack.Children.Add(LogoutStack);

            MenuStack.Children.Add(new BoxView
            {
                Color = Color.LightGray,
                HeightRequest = 0.5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            });

            ContentStack.Children.Add(MenuStack);
        }

        private void Logout()
        {
            Settings.ResetSettings();

            MainPage page = (MainPage)Application.Current.MainPage.Navigation.NavigationStack[0];
            CrossConnectivity.Current.ConnectivityChanged -= page.HandleConnectivityChanged;

            Application.Current.MainPage = new NavigationPage(new StartPage());
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
