using Joyleaf.CustomControls;
using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Joyleaf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MainPageView : TabbedPage
    {
        private readonly StackLayout ConnectionErrorText;
        private readonly Button HighFiveButton;
        private readonly StackLayout LoadingErrorText;
        private readonly ActivityIndicator LoadingWheel;

        public MainPageView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasNavigationBar(ExplorePage, false);

            ContentStack.Padding = 25;
            ContentStack.Spacing = 25;

            ConnectionErrorText = new StackLayout
            {
                IsEnabled = false,
                IsVisible = false
            };

            ConnectionErrorText.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 35,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "You're Offline",
                TextColor = Color.Black
            });

            ConnectionErrorText.Children.Add(new Label
            {
                FontSize = 23,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Please check your network connection.",
                TextColor = Color.Gray
            });

            ExploreRelativeLayout.Children.Add(ConnectionErrorText, Constraint.RelativeToParent(parent => (parent.Width / 2) - (ConnectionErrorText.Width / 2)), Constraint.RelativeToParent(parent => (parent.Height / 2) - (ConnectionErrorText.Height / 2)));

            HighFiveButton = new Button
            {
                BackgroundColor = Color.FromHex("#23C7A5"),
                CornerRadius = 30,
                HeightRequest = 60,
                Image = "HighFive",
                WidthRequest = 60,

            };

            HighFiveButton.Clicked += HighFiveButtonClick;

            LoadingErrorText = new StackLayout
            {
                IsEnabled = false,
                IsVisible = false
            };

            LoadingErrorText.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 25,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Loading Error",
                TextColor = Color.Black
            });

            LoadingErrorText.Children.Add(new Label
            {
                FontSize = 23,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Please tap to retry.",
                TextColor = Color.Gray
            });

            TapGestureRecognizer LoadingErrorRetryGesture = new TapGestureRecognizer();
            LoadingErrorRetryGesture.Tapped += (s, e) =>
            {
                RefreshContentAsync();
            };

            LoadingErrorText.GestureRecognizers.Add(LoadingErrorRetryGesture);

            ExploreRelativeLayout.Children.Add(LoadingErrorText, Constraint.RelativeToParent(parent => (parent.Width / 2) - (LoadingErrorText.Width / 2)), Constraint.RelativeToParent(parent => (parent.Height / 2) - (LoadingErrorText.Height / 2)));

            LoadingWheel = new ActivityIndicator
            {
                Color = Color.Gray,
                IsEnabled = false,
                IsRunning = true,
                IsVisible = false
            };

            ExploreRelativeLayout.Children.Add(LoadingWheel, Constraint.RelativeToParent(parent => (parent.Width / 2) - (LoadingWheel.Width / 2)), Constraint.RelativeToParent(parent => (parent.Height / 2) - (LoadingWheel.Height / 2)));

            CrossConnectivity.Current.ConnectivityChanged += HandleConnectivityChanged;

            RefreshContentAsync();
            VerifyAuthAsync();
        }

        private async Task RefreshContentAsync()
        {
            LoadingErrorText.IsEnabled = false;
            LoadingErrorText.IsVisible = false;

            if (CrossConnectivity.Current.IsConnected)
            {
                Scroller.IsEnabled = false;

                ContentStack.Children.Clear();

                ConnectionErrorText.IsEnabled = false;
                ConnectionErrorText.IsVisible = false;

                LoadingWheel.IsEnabled = true;
                LoadingWheel.IsVisible = true;

                await Task.Delay(250);

                try
                {
                    Content content = await FirebaseBackend.LoadContentAsync();

                    ContentStack.Children.Add(new Label
                    {
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 27,
                        Margin = new Thickness(7, 0),
                        Text = "Explore",
                        TextColor = Color.Black
                    });

                    foreach (Datum datum in content.Data)
                    {
                        ContentFrame contentItem = new ContentFrame(datum);
                        ContentStack.Children.Add(contentItem);
                    }

                    LoadingWheel.IsEnabled = false;
                    LoadingWheel.IsVisible = false;

                    ExploreRelativeLayout.Children.Add(HighFiveButton, Constraint.RelativeToParent(parent => parent.Width - 75), Constraint.RelativeToParent(parent => parent.Height - 75));

                    Scroller.IsEnabled = true;
                }
                catch (Exception)
                {
                    LoadingWheel.IsEnabled = false;
                    LoadingWheel.IsVisible = false;

                    Scroller.IsEnabled = false;

                    ContentStack.Children.Clear();

                    ExploreRelativeLayout.Children.Remove(HighFiveButton);

                    LoadingErrorText.IsEnabled = true;
                    LoadingErrorText.IsVisible = true;
                }
            }
            else
            {
                Scroller.IsEnabled = false;

                ContentStack.Children.Clear();

                ExploreRelativeLayout.Children.Remove(HighFiveButton);

                ConnectionErrorText.IsEnabled = true;
                ConnectionErrorText.IsVisible = true;
            }
        }

        private async Task VerifyAuthAsync()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                bool valid = await FirebaseBackend.IsCurrentAuthValidAsync();

                if (!valid)
                {
                    await FirebaseBackend.RefreshAuthAsync();
                    bool isFreshAuthValid = await FirebaseBackend.IsCurrentAuthValidAsync();

                    if (!isFreshAuthValid)
                    {
                        Settings.ResetSettings();

                        CrossConnectivity.Current.ConnectivityChanged -= HandleConnectivityChanged;

                        Application.Current.MainPage = new NavigationPage(new StartPageView());
                        await Application.Current.MainPage.DisplayAlert("You have been signed out", "The account owner may have changed the password.", "OK");
                    }
                }
            }
        }

        public void Resume()
        {
            if (FirebaseBackend.IsContentExpired() || LoadingErrorText.IsVisible)
            {
                RefreshContentAsync();
            }

            VerifyAuthAsync();
        }

        private void HandleConnectivityChanged(object sender, EventArgs a)
        {
            RefreshContentAsync();
            VerifyAuthAsync();
        }

        private void HighFiveButtonClick(object sender, EventArgs e)
        {

        }

        private void LogoutButtonClick(object sender, EventArgs e)
        {
            Settings.ResetSettings();

            CrossConnectivity.Current.ConnectivityChanged -= HandleConnectivityChanged;

            Application.Current.MainPage = new NavigationPage(new StartPageView());
        }
    }
}