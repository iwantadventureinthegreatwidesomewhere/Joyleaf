using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class SearchPage : ContentPage
    {
        private readonly string[] Flavors = { "Earthy", "Chemical", "Pine", "Spicy/Herbal", "Pungent", "Pepper", "Flowery", "Citrus", "Orange", "Sweet", "Skunk", "Grape", "Minty", "Woody", "Cheese", "Diesel", "Tropical", "Grapefruit", "Nutty", "Lemon", "Berry", "Blueberry", "Ammonia", "Apple", "Rose", "Butter", "Honey", "Tea", "Lime", "Lavender", "Strawberry", "Mint", "Chestnut", "Tree Fruit", "Pear", "Apricot", "Peach", "Blue Cheese", "Menthol", "Coffee", "Tar", "Mango", "Pineapple", "Sage", "Vanilla", "Plum", "Tobacco", "Violet" };
        private readonly string[] PositiveEffects = { "Relaxed", "Hungry", "Euphoric", "Happy", "Creative", "Energetic", "Talkative", "Uplifted", "Tingly", "Sleepy", "Focused", "Giggly", "Aroused" };
        private readonly string[] MedicalEffects = { "Insomnia", "Pain", "Stress", "Fatigue", "Headaches" };

        private readonly string[] WordsToRemove = { "a", "an", "the", "for", "and", "nor", "but", "or", "yet", "so", "at", "around", "by", "after", "along", "from", "of", "on", "to", "with", "without" };

        private readonly StackLayout SuggestedStack;

        private readonly StackLayout LoadingStack;
        private readonly StackLayout NoResultsStack;
        private readonly Label NoResultsText;
        private readonly StackLayout ConnectionErrorStack;
        private readonly StackLayout SearchErrorStack;

        private string CachePreviousSearch;

        private CancellationTokenSource CancellationTokenSource;

        public SearchPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            string[] flavors = Flavors.OrderBy(arg => Guid.NewGuid()).Take(3).ToArray();
            string[] positiveEffects = PositiveEffects.OrderBy(arg => Guid.NewGuid()).Take(3).ToArray();
            string[] medicalEffects = MedicalEffects.OrderBy(arg => Guid.NewGuid()).Take(3).ToArray();

            string[] merged = flavors.Concat(positiveEffects).Concat(medicalEffects).ToArray();

            Random random = new Random();
            string[] shuffled = merged.OrderBy(x => random.Next()).Take(6).ToArray();

            SuggestedStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(10,0),
                Orientation = StackOrientation.Vertical
            };

            SuggestedStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 22,
                Margin = new Thickness(0, 0, 0, 10),
                Text = "Suggested",
                TextColor = Color.FromHex("#333333")
            });

            foreach (string s in shuffled)
            {
                Color color;

                if (Flavors.Contains(s))
                {
                    color = Color.FromHex("#e349c2");
                }
                else if (PositiveEffects.Contains(s))
                {
                    color = Color.FromHex("#00b368");
                }
                else if (MedicalEffects.Contains(s))
                {
                    color = Color.FromHex("#3269e6");
                }
                else
                {
                    color = Color.Transparent;
                }

                SuggestedStack.Children.Add(new BoxView
                {
                    Color = Color.LightGray,
                    HeightRequest = 0.5,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                });

                StackLayout TopicStack = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Vertical
                };

                string text;

                if (MedicalEffects.Contains(s))
                {
                    text = "treats " + s.ToLower();
                }
                else
                {
                    text = s.ToLower();
                }

                TopicStack.Children.Add(new Label
                {
                    FontSize = 22,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 10),
                    Text = text,
                    TextColor = color
                });

                TapGestureRecognizer TopicTap = new TapGestureRecognizer();
                TopicTap.Tapped += (sender, e) =>
                {
                    CancellationTokenSource = new CancellationTokenSource();

                    SearchAsync(s);
                };

                TopicStack.GestureRecognizers.Add(TopicTap);

                SuggestedStack.Children.Add(TopicStack);
            }

            ContentStack.Children.Add(SuggestedStack);

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

            SearchRelativeLayout.Children.Add(LoadingStack,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getLoadingActivityIndicatorWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getLoadingActivityIndicatorHeight(parent) / 2)));

            double getLoadingActivityIndicatorWidth(RelativeLayout parent) => LoadingStack.Measure(parent.Width, parent.Height).Request.Width;
            double getLoadingActivityIndicatorHeight(RelativeLayout parent) => LoadingStack.Measure(parent.Width, parent.Height).Request.Height;

            NoResultsStack = new StackLayout
            {
                IsVisible = false,
            };

            NoResultsStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 35,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "No Results",
                TextColor = Color.FromHex("#333333")
            });

            NoResultsText = new Label
            {
                FontSize = 23,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.Gray
            };

            NoResultsStack.Children.Add(NoResultsText);

            SearchRelativeLayout.Children.Add(NoResultsStack,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getNoResultsTextWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getNoResultsTextHeight(parent) / 2)));

            double getNoResultsTextWidth(RelativeLayout parent) => NoResultsStack.Measure(parent.Width, parent.Height).Request.Width;
            double getNoResultsTextHeight(RelativeLayout parent) => NoResultsStack.Measure(parent.Width, parent.Height).Request.Height;

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

            SearchRelativeLayout.Children.Add(ConnectionErrorStack,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getConnectionErrorStackWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getConnectionErrorStackHeight(parent) / 2)));

            double getConnectionErrorStackWidth(RelativeLayout parent) => ConnectionErrorStack.Measure(parent.Width, parent.Height).Request.Width;
            double getConnectionErrorStackHeight(RelativeLayout parent) => ConnectionErrorStack.Measure(parent.Width, parent.Height).Request.Height;

            SearchErrorStack = new StackLayout
            {
                IsVisible = false
            };

            SearchErrorStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 27,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Search Error",
                TextColor = Color.FromHex("#333333")
            });

            SearchErrorStack.Children.Add(new Label
            {
                FontSize = 23,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Please tap to retry.",
                TextColor = Color.Gray
            });

            TapGestureRecognizer RetrySearchTap = new TapGestureRecognizer();
            RetrySearchTap.Tapped += (sender, e) =>
            {
                CancellationTokenSource = new CancellationTokenSource();

                SearchAsync(CachePreviousSearch);
            };

            SearchErrorStack.GestureRecognizers.Add(RetrySearchTap);

            SearchRelativeLayout.Children.Add(SearchErrorStack,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getSearchErrorTextWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getSearchErrorTextHeight(parent) / 2)));

            double getSearchErrorTextWidth(RelativeLayout parent) => SearchErrorStack.Measure(parent.Width, parent.Height).Request.Width;
            double getSearchErrorTextHeight(RelativeLayout parent) => SearchErrorStack.Measure(parent.Width, parent.Height).Request.Height;

            HandleConnectivityChanged();
        }

        private async Task SearchAsync(string s)
        {
            searchBar.IsEnabled = false;

            try
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    string[] words = System.Text.RegularExpressions.Regex.Split(s.Trim(), @"\s+");
                    ArrayList a = new ArrayList();

                    foreach (string word in words)
                    {
                        if (word.Length > 1 && !WordsToRemove.Contains(word.ToLower()))
                        {
                            a.Add(word);
                        }
                    }

                    string[] filtered = (string[])a.ToArray(typeof(string));

                    if (filtered.Any())
                    {
                        searchBar.Text = s;
                        CachePreviousSearch = s;

                        NoResultsStack.IsVisible = false;
                        SearchErrorStack.IsVisible = false;

                        ContentStack.Children.Clear();

                        LoadingStack.IsVisible = true;

                        await Task.Delay(250);

                        SearchResult result = await FirebaseBackend.SearchAsync(filtered, CancellationTokenSource);

                        if (result.Items.Any())
                        {
                            foreach (Item item in result.Items)
                            {
                                ContentStack.Children.Add(new SearchItem(item));
                            }
                        }
                        else
                        {
                            NoResultsText.Text = "for \"" + s + "\"";

                            NoResultsStack.IsVisible = true;
                        }

                        LoadingStack.IsVisible = false;

                        ClearButton.IsVisible = true;
                    }
                }

                searchBar.IsEnabled = true;
            }
            catch (OperationCanceledException)
            {
                LoadingStack.IsVisible = false;

                ContentStack.Children.Clear();
            }
            catch (Exception)
            {
                LoadingStack.IsVisible = false;

                searchBar.IsEnabled = true;
                ContentStack.Children.Clear();

                SearchErrorStack.IsVisible = true;
            }
        }

        private void ClearSearch(object sender, EventArgs e)
        {
            ClearButton.IsVisible = false;

            searchBar.Text = "";
            ContentStack.Children.Clear();

            NoResultsStack.IsVisible = false;
            SearchErrorStack.IsVisible = false;

            ContentStack.Children.Add(SuggestedStack);
        }

        private void SearchButtonPressed(object sender, EventArgs e)
        {
            CancellationTokenSource = new CancellationTokenSource();

            SearchAsync(searchBar.Text);
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public void HandleConnectivityChanged()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                ConnectionErrorStack.IsVisible = false;

                searchBar.IsEnabled = true;

                ContentStack.Children.Add(SuggestedStack);
            }
            else
            {
                if(CancellationTokenSource != null)
                {
                    CancellationTokenSource.Cancel();
                }

                LoadingStack.IsVisible = false;

                ClearButton.IsVisible = false;

                searchBar.IsEnabled = false;
                searchBar.Text = "";
                ContentStack.Children.Clear();

                NoResultsStack.IsVisible = false;
                SearchErrorStack.IsVisible = false;

                ConnectionErrorStack.IsVisible = true;
            }
        }
    }
}
