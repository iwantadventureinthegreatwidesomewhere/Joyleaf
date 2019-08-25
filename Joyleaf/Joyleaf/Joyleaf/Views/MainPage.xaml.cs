using CarouselView.FormsPlugin.Abstractions;
using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        private bool completedOnboarding;

        private readonly string[] PositiveEffects = { "Relaxed", "Hungry", "Euphoric", "Happy", "Creative", "Energetic", "Talkative", "Uplifted", "Tingly", "Sleepy", "Focused", "Giggly", "Aroused" };


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
                FontSize = 13,
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
                GetContentAsync();
            };

            LoadingErrorStack.GestureRecognizers.Add(RetryLoadingTap);

            ExploreRelativeLayout.Children.Add(LoadingErrorStack,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getLoadingErrorStackWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getLoadingErrorStackHeight(parent) / 2)));

            double getLoadingErrorStackWidth(RelativeLayout parent) => LoadingErrorStack.Measure(parent.Width, parent.Height).Request.Width;
            double getLoadingErrorStackHeight(RelativeLayout parent) => LoadingErrorStack.Measure(parent.Width, parent.Height).Request.Height;

            if (!Settings.HasCompletedOnboarding)
            {
                StackLayout OnboardingStack = new StackLayout
                {
                    BackgroundColor = Color.White,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                CarouselViewControl Carousel = new CarouselViewControl
                {
                    CurrentPageIndicatorTintColor = Color.Black,
                    HeightRequest = 500,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    IndicatorsShape = IndicatorsShape.Circle,
                    Orientation = CarouselViewOrientation.Horizontal,
                    ShowIndicators = true,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                StackLayout WelcomeStack = new StackLayout
                {
                    Padding = new Thickness(30, 0)
                };

                WelcomeStack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 27,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(17, 0, 17, 7),
                    Text = "Welcome to Joyleaf",
                    TextColor = Color.FromHex("#333333")
                });

                WelcomeStack.Children.Add(new Label
                {
                    FontSize = 17,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(17, 0, 17, 50),
                    Text = "Joyleaf is a strain finder app with over 2,000 unique strains displayed according to properties like species and effects.",
                    TextColor = Color.FromHex("#333333")
                });

                StackLayout SpeciesStack = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };

                StackLayout SativaStack = new StackLayout
                {
                    Margin = new Thickness(0, 0, 0, 25),
                    Orientation = StackOrientation.Horizontal
                };

                SativaStack.Children.Add(new Image
                {
                    HeightRequest = 40,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 0, 50, 0),
                    Source = "FeaturedSativa"
                });

                SativaStack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 17,
                    Text = "SATIVA",
                    TextColor = Color.FromHex("#ffa742"),
                    VerticalOptions = LayoutOptions.Center
                });

                SpeciesStack.Children.Add(SativaStack);

                StackLayout IndicaStack = new StackLayout
                {
                    Margin = new Thickness(0, 0, 0, 25),
                    Orientation = StackOrientation.Horizontal
                };

                IndicaStack.Children.Add(new Image
                {
                    HeightRequest = 28,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 0, 50, 0),
                    Source = "FeaturedIndica"
                });

                IndicaStack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 17,
                    Text = "INDICA",
                    TextColor = Color.FromHex("#774dff"),
                    VerticalOptions = LayoutOptions.Center
                });

                SpeciesStack.Children.Add(IndicaStack);

                StackLayout HybridStack = new StackLayout
                {
                    Margin = new Thickness(0, 0, 0, 25),
                    Orientation = StackOrientation.Horizontal
                };

                HybridStack.Children.Add(new Image
                {
                    HeightRequest = 32,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(4, 0, 54, 0),
                    Source = "FeaturedHybrid"
                });

                HybridStack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 17,
                    Text = "HYBRID",
                    TextColor = Color.FromHex("#00b368"),
                    VerticalOptions = LayoutOptions.Center
                });

                SpeciesStack.Children.Add(HybridStack);

                WelcomeStack.Children.Add(SpeciesStack);

                Button WelcomeButton = new Button
                {
                    BackgroundColor = Color.FromHex("#333333"),
                    CornerRadius = 18,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 15,
                    HeightRequest = 35,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(0, 0, 0, 50),
                    Text = "NEXT",
                    TextColor = Color.White,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    WidthRequest = 130
                };

                WelcomeButton.Clicked += delegate (Object sender, EventArgs e)
                {
                    Carousel.Position = 1;
                };

                WelcomeStack.Children.Add(WelcomeButton);

                StackLayout HighfiveStack = new StackLayout
                {
                    Padding = new Thickness(30, 0)
                };

                HighfiveStack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 27,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(17, 0, 17, 7),
                    Text = "Highfive",
                    TextColor = Color.FromHex("#333333")
                });

                HighfiveStack.Children.Add(new Label
                {
                    FontSize = 17,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(17, 0, 17, 50),
                    Text = "Use Highfive to get strains personalized to your interests within seconds.",
                    TextColor = Color.FromHex("#333333")
                });

                HighfiveStack.Children.Add(new Image
                {
                    HeightRequest = 100,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Source = "HighfiveOnboarding"
                });

                Button HighfiveButton = new Button
                {
                    BackgroundColor = Color.FromHex("#333333"),
                    CornerRadius = 18,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 15,
                    HeightRequest = 35,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(0, 0, 0, 50),
                    Text = "NEXT",
                    TextColor = Color.White,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    WidthRequest = 130
                };

                HighfiveButton.Clicked += delegate (Object sender, EventArgs e)
                {
                    Carousel.Position = 2;
                };

                HighfiveStack.Children.Add(HighfiveButton);

                StackLayout GetStartedStack = new StackLayout
                {
                    Padding = new Thickness(30, 0)
                };

                GetStartedStack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 27,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(17, 0, 17, 7),
                    Text = "Getting Started",
                    TextColor = Color.FromHex("#333333")
                });

                GetStartedStack.Children.Add(new Label
                {
                    FontSize = 17,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(17, 0, 17, 50),
                    Text = "To help you get started, select some of your cannabis interests from the following list.",
                    TextColor = Color.FromHex("#333333")
                });

                ArrayList interests = new ArrayList();

                Random random = new Random();
                string[] shuffled = PositiveEffects.OrderBy(x => random.Next()).Take(10).ToArray();

                FlexLayout TagLayout = new FlexLayout
                {
                    AlignContent = FlexAlignContent.Start,
                    AlignItems = FlexAlignItems.Start,
                    Direction = FlexDirection.Row,
                    JustifyContent = FlexJustify.SpaceBetween,
                    Margin = new Thickness(10, 0, 0, 0),
                    Wrap = FlexWrap.Wrap
                };

                foreach (string s in shuffled)
                {
                    Color color;

                    if (PositiveEffects.Contains(s))
                    {
                        color = Color.FromHex("#00b368");
                    }
                    else
                    {
                        color = Color.Transparent;
                    }

                    StackLayout TagStack = new StackLayout
                    {
                        Margin = new Thickness(0, 7, 0, 7),
                        Orientation = StackOrientation.Horizontal
                    };

                    TagStack.Children.Add(new Frame
                    {
                        BackgroundColor = Color.Transparent,
                        BorderColor = color,
                        Content = new Label
                        {
                            FontSize = 15,
                            Margin = new Thickness(15, 5),
                            Text = s,
                            TextColor = color
                        },
                        CornerRadius = 10,
                        Padding = 0,
                        HasShadow = false
                    });

                    TagStack.Children.Add(new BoxView
                    {
                        HeightRequest = 0,
                        WidthRequest = 5
                    });

                    TapGestureRecognizer TagTap = new TapGestureRecognizer();
                    TagTap.Tapped += (sender, e) =>
                    {
                        Frame frame = (Frame)((StackLayout)sender).Children.GetItem(0);

                        if(frame.BackgroundColor == Color.Transparent)
                        {
                            frame.BackgroundColor = frame.BorderColor;

                            Label label = (Label)frame.Content;
                            label.TextColor = Color.White;

                            interests.Add(label.Text);
                        }
                        else
                        {
                            frame.BackgroundColor = Color.Transparent;

                            Label label = (Label)frame.Content;
                            label.TextColor = frame.BorderColor;

                            interests.Remove(label.Text);
                        }
                    };

                    TagStack.GestureRecognizers.Add(TagTap);
                    TagLayout.Children.Add(TagStack);
                }

                GetStartedStack.Children.Add(TagLayout);

                Button GetStartedButton = new Button
                {
                    BackgroundColor = Color.FromHex("#333333"),
                    CornerRadius = 18,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 15,
                    HeightRequest = 35,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Margin = new Thickness(0, 0, 0, 50),
                    Text = "DONE",
                    TextColor = Color.White,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    WidthRequest = 130
                };

                GetStartedButton.Clicked += delegate (Object sender, EventArgs e)
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        if(interests.Count > 0)
                        {
                            Settings.HasCompletedOnboarding = true;
                            ExploreRelativeLayout.Children.Remove(OnboardingStack);

                            foreach(string str in interests)
                            {
                                Log.AddTopic(str);
                            }

                            CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();
                            FirebaseBackend.SendLogAsync(CancellationTokenSource);

                            VerifyAuthAsync();
                            GetContentAsync();
                        }
                        else
                        {
                            DisplayAlert("No interests selected", "Please select at least one interest from the list, then try again.", "OK");
                        }
                    }
                    else
                    {
                        DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
                    }
                };

                GetStartedStack.Children.Add(GetStartedButton);

                Carousel.ItemsSource = new List<StackLayout> { WelcomeStack, HighfiveStack, GetStartedStack };
                OnboardingStack.Children.Add(Carousel);

                ExploreRelativeLayout.Children.Add(OnboardingStack, null, null,
                    Constraint.RelativeToParent(parent => parent.Width),
                    Constraint.RelativeToParent(parent => parent.Height));
            }
            else
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    VerifyAuthAsync();
                    GetContentAsync();
                }
                else
                {
                    LoadingErrorStack.IsVisible = false;
                    ContentStack.Children.Clear();
                    ConnectionErrorStack.IsVisible = true;
                }
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

        private async Task GetContentAsync()
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
                GetContentAsync();
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
                    GetContentAsync();
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
