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
        private readonly ActivityIndicator LoadingWheel;

        public MainPageView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

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

            LoadingWheel = new ActivityIndicator
            {
                Color = Color.Gray,
                IsEnabled = false,
                IsRunning = true,
                IsVisible = false
            };

            ExploreRelativeLayout.Children.Add(LoadingWheel, Constraint.RelativeToParent(parent => (parent.Width / 2) - (LoadingWheel.Width / 2)), Constraint.RelativeToParent(parent => (parent.Height / 2) - (LoadingWheel.Height / 2)));

            CrossConnectivity.Current.ConnectivityChanged += HandleConnectivityChanged;

            RefreshContent();
            VerifyAuth();
        }

        private void RefreshContent()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                Scroller.IsEnabled = false;

                ContentStack.Children.Clear();

                ConnectionErrorText.IsEnabled = false;
                ConnectionErrorText.IsVisible = false;

                LoadingWheel.IsEnabled = true;
                LoadingWheel.IsVisible = true;

                FirebaseBackend.LoadContentAsync().ContinueWith((content) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ContentStack.Children.Add(new Label
                        {
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 27,
                            Margin = new Thickness(7, 0),
                            Text = "Explore",
                            TextColor = Color.Black
                        });

                        foreach (Datum datum in content.Result.Data)
                        {
                            ContentFrame contentItem = new ContentFrame(datum);
                            ContentStack.Children.Add(contentItem);
                        }

                        LoadingWheel.IsEnabled = false;
                        LoadingWheel.IsVisible = false;

                        ExploreRelativeLayout.Children.Add(HighFiveButton, Constraint.RelativeToParent(parent => parent.Width - 75), Constraint.RelativeToParent(parent => parent.Height - 75));

                        Scroller.IsEnabled = true;
                    });
                });
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

        private void VerifyAuth()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                FirebaseBackend.IsCurrentAuthValidAsync().ContinueWith((valid) =>
                {
                    if (!valid.Result)
                    {
                        bool isFreshAuthValid = Task.Run(async () =>
                        {
                            await FirebaseBackend.RefreshAuthAsync();
                            return await FirebaseBackend.IsCurrentAuthValidAsync();
                        }).Result;

                        if (!isFreshAuthValid)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                CrossConnectivity.Current.ConnectivityChanged -= HandleConnectivityChanged;

                                Settings.ResetSettings();

                                Application.Current.MainPage = new NavigationPage(new StartPageView());
                                Application.Current.MainPage.DisplayAlert("You have been signed out", "The account owner may have changed the password.", "OK");
                            });
                        }
                    }
                });
            }
        }

        public void Resume()
        {
            if (FirebaseBackend.IsContentExpired())
            {
                RefreshContent();
            }

            VerifyAuth();
        }

        private void HandleConnectivityChanged(object sender, EventArgs a)
        {
            RefreshContent();
            VerifyAuth();
        }

        private void HighFiveButtonClick(object sender, EventArgs e)
        {

        }

        private void LogoutButtonClick(object sender, EventArgs e)
        {
            CrossConnectivity.Current.ConnectivityChanged -= HandleConnectivityChanged;

            Settings.ResetSettings();

            Application.Current.MainPage = new NavigationPage(new StartPageView());
        }
    }
}