using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CarouselView.FormsPlugin.Abstractions;
using Joyleaf.CustomControls;
using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class HighfivePage : GradientPage
    {
        private StackLayout WelcomeStack;
        private Button FindStrainsButton;

        private readonly StackLayout LoadingStack;

        private CancellationTokenSource CancellationTokenSource;

        public HighfivePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            LoadingStack = new StackLayout
            {
                IsVisible = false
            };

            LoadingStack.Children.Add(new ActivityIndicator
            {
                Color = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                IsRunning = true,
                Margin = new Thickness(0, 0, 0, 3)
            });

            LoadingStack.Children.Add(new Label
            {
                FontFamily = (OnPlatform<string>)Application.Current.Resources["SF-Regular"],
                FontSize = 13,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "LOADING",
                TextColor = Color.White
            });

            HighfiveRelativeLayout.Children.Add(LoadingStack,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getLoadingActivityIndicatorWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getLoadingActivityIndicatorHeight(parent) / 2)));

            double getLoadingActivityIndicatorWidth(RelativeLayout parent) => LoadingStack.Measure(parent.Width, parent.Height).Request.Width;
            double getLoadingActivityIndicatorHeight(RelativeLayout parent) => LoadingStack.Measure(parent.Width, parent.Height).Request.Height;

            WelcomeStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(30, 70),
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            WelcomeStack.Children.Add(new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 0, 0, 50),
                Source = "Hand"
            });

            WelcomeStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontFamily = (OnPlatform<string>)Application.Current.Resources["SF-Bold"],
                FontSize = 33,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 0, 0, 25),
                Text = "Highfive",
                TextColor = Color.White
            });

            WelcomeStack.Children.Add(new Label
            {
                FontFamily = (OnPlatform<string>)Application.Current.Resources["SF-Regular"],
                FontSize = 17,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 15),
                Text = "Highfive searches our database of over 2,000 unique strains to instantly find you five best suited to your interests.",
                TextColor = Color.White,
            });

            WelcomeStack.Children.Add(new Label
            {
                FontFamily = (OnPlatform<string>)Application.Current.Resources["SF-Regular"],
                FontSize = 17,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "The more you use Joyleaf, the more personalized the strains!",
                TextColor = Color.White,
            });

            FindStrainsButton = new Button
            {
                BackgroundColor = Color.White,
                CornerRadius = 23,
                FontAttributes = FontAttributes.Bold,
                FontFamily = (OnPlatform<string>)Application.Current.Resources["SF-Bold"],
                FontSize = 15,
                HeightRequest = 45,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "GET STRAINS NOW",
                TextColor = Color.FromHex("#333333"),
                VerticalOptions = LayoutOptions.EndAndExpand
            };

            FindStrainsButton.Clicked += FindStrainsButtonClicked;

            WelcomeStack.Children.Add(FindStrainsButton);
            HighfiveStack.Children.Add(WelcomeStack);
        }

        private async Task HighfiveAsync()
        {
            HighfiveStack.Children.Remove(WelcomeStack);
            LoadingStack.IsVisible = true;

            try
            {
                HighfiveResult highfiveResult = await FirebaseBackend.HighfiveAsync(CancellationTokenSource);

                StackLayout ResultStack = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                ResultStack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontFamily = (OnPlatform<string>)Application.Current.Resources["SF-Bold"],
                    FontSize = 27,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(17, 0, 17, 7),
                    Text = "Congrats!",
                    TextColor = Color.White
                });

                ResultStack.Children.Add(new Label
                {
                    FontFamily = (OnPlatform<string>)Application.Current.Resources["SF-Regular"],
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(17, 0, 17, 15),
                    Text = "Here’s five strains we found just for you.",
                    TextColor = Color.White
                });

                CarouselViewControl Carousel = new CarouselViewControl
                {
                    CurrentPageIndicatorTintColor = Color.White,
                    HeightRequest = 425,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    IndicatorsShape = IndicatorsShape.Circle,
                    Orientation = CarouselViewOrientation.Horizontal,
                    ShowIndicators = true
                };

                foreach (Result result in highfiveResult.Result)
                {
                    if (Carousel.ItemsSource != null)
                    {
                        List<object> list = Carousel.ItemsSource.GetList();
                        list.Add(new HighfiveItem(result));
                        Carousel.ItemsSource = list;
                    }
                    else
                    {
                        Carousel.ItemsSource = new List<HighfiveItem> { new HighfiveItem(result) };
                    }
                }

                RelativeLayout RelativeLayout = new RelativeLayout
                {
                    HeightRequest = 425
                };

                RelativeLayout.Children.Add(Carousel,
                    Constraint.RelativeToParent(parent => 0),
                    Constraint.RelativeToParent(parent => 0),
                    Constraint.RelativeToParent(parent => parent.Width),
                    Constraint.Constant(425));

                Image ScrollMoreBig = new Image
                {
                    HeightRequest = 84,
                    Source = "ScrollMoreBig",
                    WidthRequest = 43
                };

                RelativeLayout.Children.Add(ScrollMoreBig,
                    Constraint.RelativeToParent(parent => parent.Width - 43),
                    Constraint.RelativeToParent(parent => (((parent.Height - 125) / 2) + 34) - 43));

                ResultStack.Children.Add(RelativeLayout);

                LoadingStack.IsVisible = false;
                HighfiveStack.Children.Add(ResultStack);
            }
            catch (OperationCanceledException)
            {
                LoadingStack.IsVisible = false;
                HighfiveStack.Children.Add(WelcomeStack);
            }
            catch (Exception)
            {
                LoadingStack.IsVisible = false;
                HighfiveStack.Children.Add(WelcomeStack);
                await DisplayAlert("Error", "Whoops, looks like there's a problem. Please try again later.", "OK");
            }
        }

        private void FindStrainsButtonClicked(object sender, EventArgs e)         {             if (CrossConnectivity.Current.IsConnected)
            {
                CancellationTokenSource = new CancellationTokenSource();
                HighfiveAsync();
            }
            else
            {
                DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }         }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public void HandleConnectivityChanged()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                if (CancellationTokenSource != null)
                {
                    CancellationTokenSource.Cancel();
                }

                HighfiveStack.Children.Clear();
                HighfiveStack.Children.Add(WelcomeStack);
                DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }
        }
    }
}
