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
                Text = "Offline",
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

            RefreshTimer = new Timer(1000 * 60 * 60 * 3);
            RefreshTimer.Elapsed += HandleTimedRefreshEvent;
            RefreshTimer.Start();
        }
















        private void RefreshContent()
        {
            Content.Children.Clear();

            Content.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 27,
                Margin = new Thickness(7, 0),
                Text = "Explore",
                TextColor = Color.Black
            });



            var i = new ExploreBox
            {
                IsWhite = false,
                StartColor = Color.FromHex("#5b10c1"),
                EndColor = Color.FromHex("#1a92f0")
            };


            var s = new StackLayout();
            s.Children.Add(new Label
            {
                Text = "Trending Now  \U0001F91F",
                FontSize = 22,
                Margin = new Thickness(10),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White
            });
            i.Content = s;
            Content.Children.Add(i);







            var i4 = new ExploreBox
            {
                IsWhite = true
            };


            var s4 = new StackLayout();
            s4.Children.Add(new Label
            {
                Text = "Happy Vibes",
                FontSize = 22,
                Margin = new Thickness(10),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            });
            i4.Content = s4;
            Content.Children.Add(i4);



            var i5 = new ExploreBox
            {
                IsWhite = true
            };


            var s5 = new StackLayout();
            s5.Children.Add(new Label
            {
                Text = "Brands We Love",
                FontSize = 22,
                Margin = new Thickness(10),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            });
            i5.Content = s5;
            Content.Children.Add(i5);





            var i3 = new ExploreBox
            {
                IsWhite = false,
                StartColor = Color.FromHex("#fe548f"),
                EndColor = Color.FromHex("#f59e3d")
            };


            var s3 = new StackLayout();
            s3.Children.Add(new Label
            {
                Text = "New Cannabis  \ud83c\udf89",
                FontSize = 22,
                Margin = new Thickness(10),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White
            });
            i3.Content = s3;
            Content.Children.Add(i3);


            var i6 = new ExploreBox
            {
                IsWhite = true
            };

            var s6 = new StackLayout();
            s6.Children.Add(new Label
            {
                Text = "By Aurora",
                FontSize = 22,
                Margin = new Thickness(10),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            });
            i6.Content = s6;
            Content.Children.Add(i6);




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
                            CrossConnectivity.Current.ConnectivityChanged -= HandleConnectionChecksEvent;
                            RefreshTimer.Stop();
                            FirebaseBackend.DeleteAuth();
                            Application.Current.MainPage = new NavigationPage(new SignInPageView());
                            Application.Current.MainPage.DisplayAlert("You have been signed out", "The account owner may have changed the password.", "OK");
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
            FirebaseBackend.DeleteAuth();
            Application.Current.MainPage = new NavigationPage(new SignInPageView());
        }
    }
}