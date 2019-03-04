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
        public MainPageView()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            Content.Padding = 25;
            Content.Spacing = 25;

            //-------------------------------------------------------------------------
            var image = new Image { Source = "Logo" };
            Button AwesomeButton = new Button
            {
                BackgroundColor = Color.FromHex("#23C7A5"),
                CornerRadius = 30,
                HeightRequest = 60,
                Image = "AwesomeButton",
                WidthRequest = 60
            };
            AwesomeButton.Clicked += (object sender, EventArgs e) => OnButtonClicked();

            StackLayout ConnectionErrorText = new StackLayout();

            ConnectionErrorText.IsEnabled = false;
            ConnectionErrorText.IsVisible = false;

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

            ExploreRelativeLayout.Children.Add(ConnectionErrorText, Constraint.RelativeToParent(parent => (parent.Width / 2) - (ConnectionErrorText.Width / 2)),Constraint.RelativeToParent(parent => (parent.Height / 2) - (ConnectionErrorText.Height / 2)));

            //-------------------------------------------------------------------------

            if (CrossConnectivity.Current.IsConnected)
            {
                bool valid = Task.Run(async () =>
                {
                    return await FirebaseBackend.IsSavedAuthValidAsync();
                }).Result;

                if (!valid)
                {
                    try
                    {
                        Task.Run(async () =>
                        {
                            await FirebaseBackend.RefreshAuthAsync();
                        });
                    }
                    catch (Exception)
                    {
                        Device.BeginInvokeOnMainThread(() => 
                        {
                            Application.Current.MainPage = new NavigationPage(new SignInPageView());
                        });
                    }
                }

                ExploreRelativeLayout.Children.Add(AwesomeButton,
                Constraint.RelativeToParent(parent => parent.Width - 75),
                Constraint.RelativeToParent(parent => parent.Height - 75)
                );

                RefreshContent();
            }
            else
            {
                ConnectionErrorText.IsEnabled = true;
                ConnectionErrorText.IsVisible = true;

                Scroller.IsEnabled = false;
            }

            //-------------------------------------------------------------------------

            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    bool valid = Task.Run(async () =>
                    {
                        return await FirebaseBackend.IsSavedAuthValidAsync();
                    }).Result;

                    if (!valid)
                    {
                        try
                        {
                            Task.Run(async () =>
                            {
                                await FirebaseBackend.RefreshAuthAsync();
                            });
                        }
                        catch (Exception)
                        {
                            Device.BeginInvokeOnMainThread(() => 
                            {
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
            };
        }

        private void OnButtonClicked()
        {

            Application.Current.MainPage.DisplayAlert("Sup", "Reach new heights \ud83c\udf89", "Poof");
        }

        private void RefreshContent()
        {
            Content.Children.Add(
            new Label
            {
                Text = "Trending Now  \U0001F91F",
                FontSize = 27,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            });
            Content.Children.Add(new StoreItem());
            Content.Children.Add(
            new Label
            {
                Text = "Recommended For You",
                FontSize = 27,
                Margin = new Thickness(0, 20, 0, 0),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            });
            Content.Children.Add(new StoreItem());
            Content.Children.Add(
            new Label
            {
                Text = "New  \ud83c\udf89",
                FontSize = 27,
                Margin = new Thickness(0, 20, 0, 0),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            });

            Content.Children.Add(new StoreItem());
            Content.Children.Add(
            new Label
            {
                Text = "Since You Like [Brand]",
                FontSize = 27,
                Margin = new Thickness(0, 20, 0, 0),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            });
            Content.Children.Add(new StoreItem());

            Content.Children.Add(
            new Label
            {
                Text = "Buds We Love",
                FontSize = 27,
                Margin = new Thickness(0, 20, 0, 0),
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black
            });
            Content.Children.Add(new StoreItem());
        }

        private void EnableLoader()
        {
            //ContentList.Children.Add(Wheel);
        }

        private void DisableLoader()
        {
            //ContentList.Children.Remove(Wheel);
        }

        private void LogoutButtonClick(object sender, EventArgs e)
        {
            FirebaseBackend.DeleteAuth();
            Application.Current.MainPage = new NavigationPage(new SignInPageView());
        }
    }
}