using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Joyleaf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MainPageView : TabbedPage
    {
        private Button AwesomeButton;
        private StackLayout ConnectionErrorText;
        private Timer RefreshTimer;

        public MainPageView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            //#####################################################################################

            Content.Padding = 25;
            Content.Spacing = 25;

            Image image = new Image { Source = "Logo" };

            AwesomeButton = new Button
            {
                BackgroundColor = Color.FromHex("#23C7A5"),
                CornerRadius = 30,
                HeightRequest = 60,
                Image = "AwesomeButton",
                WidthRequest = 60
            };
            AwesomeButton.Clicked += (object sender, EventArgs e) => AwesomeButtonClick();

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

            //#####################################################################################

            Checks();

            CrossConnectivity.Current.ConnectivityChanged += HandleConnectionChecksEvent;

            RefreshTimer = new Timer(1000 * 60 * 60);
            RefreshTimer.Elapsed += HandleTimedRefreshEvent;
            RefreshTimer.Start();
        }
















        private void RefreshContent()
        {
            Content.Children.Clear();
            Content.Children.Add(
            new Label
            {
                Text = "Trending Now  \U0001F91F",
                FontSize = 27,
                Margin = new Thickness(5, 0, 0, 0),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            });
            Content.Children.Add(new StoreItem());
            Content.Children.Add(
            new Label
            {
                Text = "Recommended For You",
                FontSize = 27,
                Margin = new Thickness(5, 20, 0, 0),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            });
            Content.Children.Add(new StoreItem());
            Content.Children.Add(
            new Label
            {
                Text = "New  \ud83c\udf89",
                FontSize = 27,
                Margin = new Thickness(5, 20, 0, 0),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            });
            Content.Children.Add(new StoreItem());
        }










        public void Checks()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                bool valid = Task.Run(async () =>
                {
                    return await FirebaseBackend.IsSavedAuthValidAsync();
                }).Result;

                if (!valid)
                {
                    bool freshAuthValid = Task.Run(async () =>
                    {
                        await FirebaseBackend.RefreshAuthAsync();
                        return await FirebaseBackend.IsSavedAuthValidAsync();
                    }).Result;

                    if (!freshAuthValid)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            RefreshTimer.Stop();
                            Application.Current.MainPage = new NavigationPage(new SignInPageView());
                        });
                    }
                }

                ConnectionErrorText.IsEnabled = false;
                ConnectionErrorText.IsVisible = false;

                ExploreRelativeLayout.Children.Add(AwesomeButton,
                Constraint.RelativeToParent(parent => parent.Width - 75),
                Constraint.RelativeToParent(parent => parent.Height - 75)
                );

                RefreshContent();

                Scroller.IsEnabled = true;
            }
            else
            {
                Content.Children.Clear();

                ExploreRelativeLayout.Children.Remove(AwesomeButton);

                ConnectionErrorText.IsEnabled = true;
                ConnectionErrorText.IsVisible = true;

                Scroller.IsEnabled = false;
            }
        }

        private void HandleConnectionChecksEvent(object sender, EventArgs a)
        {
            Checks();
        }

        private void HandleTimedRefreshEvent(object sender, ElapsedEventArgs e)
        {
            RefreshContent();
        }

        private void AwesomeButtonClick()
        {
        }

        private void LogoutButtonClick(object sender, EventArgs e)
        {
            CrossConnectivity.Current.ConnectivityChanged -= HandleConnectionChecksEvent;
            RefreshTimer.Stop();
            Application.Current.MainPage = new NavigationPage(new SignInPageView());
        }
    }
}