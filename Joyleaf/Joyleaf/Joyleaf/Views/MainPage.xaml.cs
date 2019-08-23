using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Joyleaf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MainPage : ContentPage
    {
        private readonly Image Highfive;

        private HighfivePage highfivePage;
        private SearchPage searchPage;
        private AccountPage accountPage;

        private readonly StackLayout LoadingStack;
        private readonly StackLayout ConnectionErrorStack;
        private readonly StackLayout LoadingErrorStack;
        
        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            CrossConnectivity.Current.ConnectivityChanged += HandleConnectivityChanged;

            Highfive = new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Source = "HighFive",
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            TapGestureRecognizer HighfiveTap = new TapGestureRecognizer();
            HighfiveTap.Tapped += (sender, e) =>
            {
                HighfiveClicked();
            };

            Highfive.GestureRecognizers.Add(HighfiveTap);

            LoadingStack = new StackLayout
            {
                IsVisible = false
            };

            LoadingStack.Children.Add(new ActivityIndicator
            {
                Color = Color.Gray,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                IsRunning = true,
                Margin = new Thickness(0, 0, 0, 3)
            });

            LoadingStack.Children.Add(new Label
            {
                FontSize = 15,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "LOADING",
                TextColor = Color.Gray
            });

            ExploreRelativeLayout.Children.Add(LoadingStack,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getLoadingActivityIndicatorWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getLoadingActivityIndicatorHeight(parent) / 2)));

            double getLoadingActivityIndicatorWidth(RelativeLayout parent) => LoadingStack.Measure(parent.Width, parent.Height).Request.Width;
            double getLoadingActivityIndicatorHeight(RelativeLayout parent) => LoadingStack.Measure(parent.Width, parent.Height).Request.Height;

            ConnectionErrorStack = new StackLayout
            {
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
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getConnectionErrorStackWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getConnectionErrorStackHeight(parent) / 2)));

            double getConnectionErrorStackWidth(RelativeLayout parent) => ConnectionErrorStack.Measure(parent.Width, parent.Height).Request.Width;
            double getConnectionErrorStackHeight(RelativeLayout parent) => ConnectionErrorStack.Measure(parent.Width, parent.Height).Request.Height;

            LoadingErrorStack = new StackLayout
            {
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

            TapGestureRecognizer RetryLoadingTap = new TapGestureRecognizer();
            RetryLoadingTap.Tapped += (sender, e) =>
            {
                RefreshAsync();
            };

            LoadingErrorStack.GestureRecognizers.Add(RetryLoadingTap);

            ExploreRelativeLayout.Children.Add(LoadingErrorStack,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getLoadingErrorStackWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getLoadingErrorStackHeight(parent) / 2)));

            double getLoadingErrorStackWidth(RelativeLayout parent) => LoadingErrorStack.Measure(parent.Width, parent.Height).Request.Width;
            double getLoadingErrorStackHeight(RelativeLayout parent) => LoadingErrorStack.Measure(parent.Width, parent.Height).Request.Height;

            if (CrossConnectivity.Current.IsConnected)
            {
                VerifyAuthAsync();

                RefreshAsync();
            }
            else
            {
                LoadingErrorStack.IsVisible = false;

                ContentStack.Children.Clear();

                ConnectionErrorStack.IsVisible = true;
            }
        }

        private async void HighfiveClicked()
        {
            highfivePage = new HighfivePage();
            await Navigation.PushAsync(highfivePage);
        }

        private async void SearchClicked(object sender, EventArgs e)
        {
            searchPage = new SearchPage();
            await Navigation.PushAsync(searchPage);
        }

        private async void AccountClicked(object sender, EventArgs e)
        {
            accountPage = new AccountPage();
            await Navigation.PushAsync(accountPage);
        }

        private async Task RefreshAsync()
        {
            ConnectionErrorStack.IsVisible = false;
            LoadingErrorStack.IsVisible = false;

            ContentStack.Children.Clear();

            LoadingStack.IsVisible = true;

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

                LoadingStack.IsVisible = false;
            }
            catch (Exception)
            {
                FirebaseBackend.ResetContentTimer();

                LoadingStack.IsVisible = false;

                ContentStack.Children.Clear();

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
                    await DisplayAlert("You have been signed out", "The account owner may have changed the password.", "OK");
                }
            }
        }

        public void HandleConnectivityChanged(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                VerifyAuthAsync();

                RefreshAsync();
            }
            else
            {
                LoadingErrorStack.IsVisible = false;

                ContentStack.Children.Clear();

                ConnectionErrorStack.IsVisible = true;

                if (PopupNavigation.Instance.PopupStack.Count > 0)
                {
                    PopupNavigation.Instance.PopAllAsync();
                }
            }

            if (highfivePage != null)
            {
                highfivePage.HandleConnectivityChanged();
            }

            if (searchPage != null)
            {
                searchPage.HandleConnectivityChanged();
            }
        }

        public void Resume()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                VerifyAuthAsync();

                if (FirebaseBackend.IsContentExpired() || LoadingErrorStack.IsVisible)
                {
                    RefreshAsync();
                }
            }
            else
            {
                LoadingErrorStack.IsVisible = false;

                ContentStack.Children.Clear();

                ConnectionErrorStack.IsVisible = true;

                if (PopupNavigation.Instance.PopupStack.Count > 0)
                {
                    PopupNavigation.Instance.PopAllAsync();
                }
            }

            if (searchPage != null)
            {
                searchPage.HandleConnectivityChanged();
            }
        }
    }
}
