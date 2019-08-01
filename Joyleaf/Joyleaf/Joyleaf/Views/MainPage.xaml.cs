using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Joyleaf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MainPage : ContentPage
    {
        private readonly Image Highfive;
        private readonly ActivityIndicator LoadingActivityIndicator;
        private readonly StackLayout ConnectionErrorStack;
        private readonly StackLayout LoadingErrorStack;
        
        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            Highfive = new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Source = "HighFive",
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            TapGestureRecognizer HighfiveTapGesture = new TapGestureRecognizer();
            HighfiveTapGesture.Tapped += (s, e) =>
            {
                HighfiveClicked();
            };

            Highfive.GestureRecognizers.Add(HighfiveTapGesture);

            LoadingActivityIndicator = new ActivityIndicator
            {
                Color = Color.Gray,
                IsEnabled = false,
                IsRunning = true,
                IsVisible = false
            };

            ExploreRelativeLayout.Children.Add(LoadingActivityIndicator,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getLoadingWheelWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getLoadingWheelHeight(parent) / 2)));
            
            double getLoadingWheelWidth(RelativeLayout parent) => LoadingActivityIndicator.Measure(parent.Width, parent.Height).Request.Width;
            double getLoadingWheelHeight(RelativeLayout parent) => LoadingActivityIndicator.Measure(parent.Width, parent.Height).Request.Height;

            ConnectionErrorStack = new StackLayout
            {
                IsEnabled = false,
                IsVisible = false
            };

            ConnectionErrorStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 35,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "You're Offline",
                TextColor = Color.FromHex("#333333")
            });

            ConnectionErrorStack.Children.Add(new Label
            {
                FontSize = 23,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Please check your network connection.",
                TextColor = Color.Gray
            });

            ExploreRelativeLayout.Children.Add(ConnectionErrorStack,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getConnectionErrorTextWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getConnectionErrorTextHeight(parent) / 2)));

            double getConnectionErrorTextWidth(RelativeLayout parent) => ConnectionErrorStack.Measure(parent.Width, parent.Height).Request.Width;
            double getConnectionErrorTextHeight(RelativeLayout parent) => ConnectionErrorStack.Measure(parent.Width, parent.Height).Request.Height;

            CrossConnectivity.Current.ConnectivityChanged += HandleConnectivityChanged;

            LoadingErrorStack = new StackLayout
            {
                IsEnabled = false,
                IsVisible = false
            };

            LoadingErrorStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 27,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Loading Error",
                TextColor = Color.FromHex("#333333")
            });

            LoadingErrorStack.Children.Add(new Label
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

            LoadingErrorStack.GestureRecognizers.Add(LoadingRetryTapGesture);

            ExploreRelativeLayout.Children.Add(LoadingErrorStack,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getLoadingErrorTextWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getLoadingErrorTextHeight(parent) / 2)));

            double getLoadingErrorTextWidth(RelativeLayout parent) => LoadingErrorStack.Measure(parent.Width, parent.Height).Request.Width;
            double getLoadingErrorTextHeight(RelativeLayout parent) => LoadingErrorStack.Measure(parent.Width, parent.Height).Request.Height;

            RefreshContentAsync();
            VerifyAuthAsync();
        }

        private void HighfiveClicked()
        {



        }

        private async void SearchClicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await Navigation.PushAsync(new SearchPage());
            }
            else
            {
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }

        private async void AccountClicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await Navigation.PushAsync(new AccountPage());
            }
            else
            {
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }

        private async Task RefreshContentAsync()
        {
            ContentStack.Children.Clear();

            ConnectionErrorStack.IsEnabled = false;
            ConnectionErrorStack.IsVisible = false;

            LoadingActivityIndicator.IsEnabled = true;
            LoadingActivityIndicator.IsVisible = true;

            await Task.Delay(250);

            try
            {
                Content content = await FirebaseBackend.LoadContentAsync();

                ContentStack.Children.Add(Highfive);

                int count = 0;

                foreach (Curated category in content.Curated)
                {
                    count++;

                    ContentStack.Children.Add(new CategoryStack(category));

                    if (count % 2 == 0)
                    {
                        ContentStack.Children.Add(new FeaturedItem(content.Featured[(count / 2) - 1]));
                    }
                }

                LoadingActivityIndicator.IsEnabled = false;
                LoadingActivityIndicator.IsVisible = false;
            }
            catch (Exception)
            {
                FirebaseBackend.ResetContentTimer();

                LoadingActivityIndicator.IsEnabled = false;
                LoadingActivityIndicator.IsVisible = false;

                ContentStack.Children.Clear();

                LoadingErrorStack.IsEnabled = true;
                LoadingErrorStack.IsVisible = true;
            }
        }

        private async Task VerifyAuthAsync()
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

        public void Resume()
        {
            if (FirebaseBackend.IsContentExpired() || LoadingErrorStack.IsVisible)
            {
                RefreshContentAsync();
            }

            VerifyAuthAsync();
        }

        private void HandleConnectivityChanged(object sender, EventArgs a)
        {
            LoadingErrorStack.IsEnabled = false;
            LoadingErrorStack.IsVisible = false;

            if (CrossConnectivity.Current.IsConnected)
            {
                RefreshContentAsync();
                VerifyAuthAsync();
            }
            else
            {
                ContentStack.Children.Clear();

                ConnectionErrorStack.IsEnabled = true;
                ConnectionErrorStack.IsVisible = true;

                Navigation.PopToRootAsync();

                if (PopupNavigation.Instance.PopupStack.Count > 0)
                {
                    PopupNavigation.Instance.PopAllAsync();
                }
            }
        }

        private void LogoutButtonClicked(object sender, EventArgs e)
        {
            Settings.ResetSettings();

            CrossConnectivity.Current.ConnectivityChanged -= HandleConnectivityChanged;

            Application.Current.MainPage = new NavigationPage(new StartPage());
        }
    }
}
