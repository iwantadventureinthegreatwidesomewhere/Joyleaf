using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ButtonWithBusyIndicator FindStrainsButton;

        public HighfivePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            WelcomeStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(40, 20),
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
                FontSize = 33,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 0, 0, 30),
                Text = "Highfive",
                TextColor = Color.White
            });

            WelcomeStack.Children.Add(new Label
            {
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 15),
                Text = "Highfive searches our database of over 2,000 unique strains to instantly find you strains best suited to your interests.",
                TextColor = Color.White,
            });

            WelcomeStack.Children.Add(new Label
            {
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "The more you use Joyleaf, the more personalized the results!",
                TextColor = Color.White,
            });

            FindStrainsButton = new ButtonWithBusyIndicator
            {
                HeightRequest = 45,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "GET STRAINS NOW",
                VerticalOptions = LayoutOptions.EndAndExpand
            };

            FindStrainsButton.Clicked += FindStrainsButtonClicked;

            WelcomeStack.Children.Add(FindStrainsButton);

            HighfiveStack.Children.Add(WelcomeStack);
        }

        private async void FindStrainsButtonClicked(object sender, EventArgs e)         {             FindStrainsButton.IsBusy = true;

            await Task.Delay(250);              if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    HighfiveResult highfiveResult = await FirebaseBackend.HighfiveAsync();

                    CarouselViewControl Carousel = new CarouselViewControl
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        HeightRequest = 600,

                        ShowIndicators = true,
                        IndicatorsShape = IndicatorsShape.Circle,
                        CurrentPageIndicatorTintColor = Color.White,

                        Orientation = CarouselViewOrientation.Horizontal
                    };

                    int position = 0;

                    foreach (Result result in highfiveResult.Result)
                    {
                        position++;

                        if(Carousel.ItemsSource != null)
                        {
                            List<object> list = Carousel.ItemsSource.GetList();
                            list.Add(new HighfiveItem(result, position));
                            Carousel.ItemsSource = list;
                        }
                        else
                        {
                            Carousel.ItemsSource = new List<HighfiveItem> { new HighfiveItem(result, position) };
                        }
                    }

                    HighfiveStack.Children.Remove(WelcomeStack);
                    HighfiveStack.Children.Add(Carousel);
                    
                    FindStrainsButton.IsBusy = false;
                }
                catch (Exception p)
                {
                    Console.WriteLine(p.Message);
                    Console.WriteLine(p.StackTrace);
                    FindStrainsButton.IsBusy = false;
                    await DisplayAlert("Error", "Whoops, looks like there's a problem on our end. Please try again later.", "OK");
                }
            }
            else
            {
                FindStrainsButton.IsBusy = false;
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }         }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
