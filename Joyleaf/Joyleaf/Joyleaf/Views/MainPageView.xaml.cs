using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using System;
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
            Content.Spacing = 35;

            Label ConnectionErrorText = new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 33,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 175, 0, 0),
                Text = "Offline",
                TextColor = Color.Black
            };

            Label ConnectionErrorMessageText = new Label
            {
                FontSize = 23,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Please check your network connection.",
                TextColor = Color.Gray
            };

            if (CrossConnectivity.Current.IsConnected)
            {
                if (!FirebaseBackend.IsSavedAuthValid())
                {
                    try
                    {
                        FirebaseBackend.RefreshAuthAsync();
                    }
                    catch (Exception)
                    {
                        Application.Current.MainPage = new NavigationPage(new SignInPageView());
                    }
                }

                RefreshContent();
            }
            else
            {
                Content.Children.Add(ConnectionErrorText);
                Content.Children.Add(ConnectionErrorMessageText);

                Scroller.IsEnabled = false;
            }

            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (!FirebaseBackend.IsSavedAuthValid())
                    {
                        try
                        {
                            FirebaseBackend.RefreshAuthAsync();
                        }
                        catch (Exception)
                        {
                            Application.Current.MainPage = new NavigationPage(new SignInPageView());
                        }
                    }

                    Content.Children.Clear();

                    RefreshContent();

                    Scroller.IsEnabled = true;
                }
                else
                {
                    Content.Children.Clear();

                    Content.Children.Add(ConnectionErrorText);
                    Content.Children.Add(ConnectionErrorMessageText);

                    Scroller.IsEnabled = false;
                }
            };
        }
















        private void RefreshContent()
        {
            Content.Children.Add(new StoreItem());
            Content.Children.Add(new StoreItem());
            Content.Children.Add(new StoreItem());
            Content.Children.Add(new StoreItem());
            /*await Task.Run(() =>
            {



                Device.BeginInvokeOnMainThread(() => { DisableLoader(); });
            });*/
        }

        private void EnableLoader()
        {
            //ContentList.Children.Add(Wheel);
        }

        private void DisableLoader()
        {
            //ContentList.Children.Remove(Wheel);
        }
    }
}