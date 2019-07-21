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

    public partial class MainPage : ContentPage
    {
        private readonly Button HighFiveButton;
        private readonly ActivityIndicator LoadingWheel;
        private readonly StackLayout ConnectionErrorText;
        private readonly StackLayout LoadingErrorText;
        
        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            ContentStack.Padding = new Thickness(0, 15);
            ContentStack.Spacing = 35;

            //######################################################

            HighFiveButton = new Button
            {
                CornerRadius = 30,
                HeightRequest = 165,
                HorizontalOptions = LayoutOptions.Center,
                Image = "HighFive",
                WidthRequest = 340
            };

            HighFiveButton.Clicked += HighFiveButtonClick;

            //######################################################

            LoadingWheel = new ActivityIndicator
            {
                Color = Color.Gray,
                IsEnabled = false,
                IsRunning = true,
                IsVisible = false
            };

            ExploreRelativeLayout.Children.Add(LoadingWheel, Constraint.RelativeToParent(parent => (parent.Width / 2) - (LoadingWheel.Width / 2)), Constraint.RelativeToParent(parent => (parent.Height / 2) - (LoadingWheel.Height / 2)));

            //######################################################

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
                TextColor = Color.FromHex("#333333")
            });

            ConnectionErrorText.Children.Add(new Label
            {
                FontSize = 23,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Please check your network connection.",
                TextColor = Color.Gray
            });

            ExploreRelativeLayout.Children.Add(ConnectionErrorText, Constraint.RelativeToParent(parent => (parent.Width / 2) - (ConnectionErrorText.Width / 2)), Constraint.RelativeToParent(parent => (parent.Height / 2) - (ConnectionErrorText.Height / 2)));

            CrossConnectivity.Current.ConnectivityChanged += HandleConnectivityChanged;

            //######################################################

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
                TextColor = Color.FromHex("#333333")
            });

            LoadingErrorText.Children.Add(new Label
            {
                FontSize = 23,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Please tap to retry.",
                TextColor = Color.Gray
            });

            TapGestureRecognizer LoadingRetryTapGesture = new TapGestureRecognizer();
            LoadingRetryTapGesture.Tapped += (s, e) =>
            {
                RefreshContentAsync();
            };

            LoadingErrorText.GestureRecognizers.Add(LoadingRetryTapGesture);

            ExploreRelativeLayout.Children.Add(LoadingErrorText, Constraint.RelativeToParent(parent => (parent.Width / 2) - (LoadingErrorText.Width / 2)), Constraint.RelativeToParent(parent => (parent.Height / 2) - (LoadingErrorText.Height / 2)));

            //######################################################

            RefreshContentAsync();
            VerifyAuthAsync();
        }

        private async Task RefreshContentAsync()
        {
            LoadingErrorText.IsEnabled = false;
            LoadingErrorText.IsVisible = false;

            if (CrossConnectivity.Current.IsConnected)
            {
                scrollView.IsEnabled = false;

                ContentStack.Children.Clear();

                ConnectionErrorText.IsEnabled = false;
                ConnectionErrorText.IsVisible = false;

                LoadingWheel.IsEnabled = true;
                LoadingWheel.IsVisible = true;

                await Task.Delay(250);

                try
                {
                    Content content = await FirebaseBackend.LoadContentAsync();

                    ContentStack.Children.Add(HighFiveButton);

                    int count = 0;

                    foreach (Curated categoryData in content.Curated)
                    {
                        count++;

                        ContentStack.Children.Add(new CategoryStack(categoryData));

                        if(count%2 == 0){
                            ContentStack.Children.Add(new FeaturedItem(content.Featured[(count/2)-1]));
                        }
                    }

                    LoadingWheel.IsEnabled = false;
                    LoadingWheel.IsVisible = false;

                    scrollView.IsEnabled = true;
                }
                catch (Exception)
                {
                    LoadingWheel.IsEnabled = false;
                    LoadingWheel.IsVisible = false;

                    scrollView.IsEnabled = false;

                    ContentStack.Children.Clear();

                    LoadingErrorText.IsEnabled = true;
                    LoadingErrorText.IsVisible = true;
                }
            }
            else
            {
                scrollView.IsEnabled = false;

                ContentStack.Children.Clear();

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

                        Application.Current.MainPage = new NavigationPage(new StartPage());
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

            Application.Current.MainPage = new NavigationPage(new StartPage());
        }
    }
}