using System;
using System.Threading.Tasks;
using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class AccountPage : ContentPage
    {
        private readonly Account account;

        public AccountPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            StackLayout MenuStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Spacing = 0
            };

            MenuStack.Children.Add(new BoxView
            {
                Color = Color.LightGray,
                HeightRequest = 0.5,
                HorizontalOptions = LayoutOptions.FillAndExpand
            });

            StackLayout ResetPasswordStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal
            };

            ResetPasswordStack.Children.Add(new Image
            {
                HeightRequest = 25,
                Margin = new Thickness(25, 0, 7, 0),
                Source = "ResetPassword",
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 25
            });

            ResetPasswordStack.Children.Add(new Label
            {
                FontFamily = (OnPlatform<string>)Application.Current.Resources["SF-Regular"],
                FontSize = 17,
                HorizontalOptions = LayoutOptions.Start,
                Margin = Device.RuntimePlatform == Device.iOS ? new Thickness(0, 15) : new Thickness(0, 10),
                Text = "Reset Password",
                TextColor = Color.Black
            });

            ResetPasswordStack.Children.Add(new Image
            {
                HeightRequest = 15,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Margin = new Thickness(0, 0, 25, 0),
                Source = "AccountArrow",
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 15
            });

            TapGestureRecognizer ResetPasswordTap = new TapGestureRecognizer();
            ResetPasswordTap.Tapped += async (sender, e) =>
            {
                ResetPasswordStack.BackgroundColor = Color.LightGray;
                await Task.Delay(100);
                ResetPasswordStack.BackgroundColor = Color.White;

                if (CrossConnectivity.Current.IsConnected)
                {
                    FirebaseBackend.SendPasswordReset(FirebaseBackend.GetAuth().User.Email, false);
                }
                else
                {
                    await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
                }
            };

            ResetPasswordStack.GestureRecognizers.Add(ResetPasswordTap);

            MenuStack.Children.Add(ResetPasswordStack);

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
                FontFamily = (OnPlatform<string>)Application.Current.Resources["SF-Regular"],
                FontSize = 17,
                HorizontalOptions = LayoutOptions.Start,
                Margin = Device.RuntimePlatform == Device.iOS ? new Thickness(0, 15) : new Thickness(0, 10),
                Text = "Contact Support",
                TextColor = Color.Black
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
            ContactSupportTap.Tapped += async (sender, e) =>
            {
                ContactSupportStack.BackgroundColor = Color.LightGray;
                await Task.Delay(100);
                ContactSupportStack.BackgroundColor = Color.White;

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
                FontFamily = (OnPlatform<string>)Application.Current.Resources["SF-Regular"],
                FontSize = 17,
                HorizontalOptions = LayoutOptions.Start,
                Margin = Device.RuntimePlatform == Device.iOS ? new Thickness(0, 15) : new Thickness(0, 10),
                Text = "Terms of Use & Privacy",
                TextColor = Color.Black
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
            TermsTap.Tapped += async (sender, e) =>
            {
                TermsStack.BackgroundColor = Color.LightGray;
                await Task.Delay(100);
                TermsStack.BackgroundColor = Color.White;
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
                FontFamily = (OnPlatform<string>)Application.Current.Resources["SF-Regular"],
                FontSize = 17,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = Device.RuntimePlatform == Device.iOS ? new Thickness(0, 15) : new Thickness(0, 10),
                Text = "Sign Out",
                TextColor = Color.FromHex("#EC5B55")
            });

            TapGestureRecognizer LogoutTap = new TapGestureRecognizer();
            LogoutTap.Tapped += async (sender, e) =>
            {
                LogoutStack.BackgroundColor = Color.LightGray;
                await Task.Delay(100);
                LogoutStack.BackgroundColor = Color.White;

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
