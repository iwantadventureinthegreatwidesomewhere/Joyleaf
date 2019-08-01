using Joyleaf.Helpers;
using Joyleaf.Services;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class SearchPage : ContentPage
    {
        private readonly StackLayout SuggestedStack;

        private readonly string[] Flavors = { "Earthy", "Chemical", "Pine", "Spicy/Herbal", "Pungent", "Pepper", "Flowery", "Citrus", "Orange", "Sweet", "Skunk", "Grape", "Minty", "Woody", "Cheese", "Diesel", "Tropical", "Grapefruit", "Nutty", "Lemon", "Berry", "Blueberry", "Ammonia", "Apple", "Rose", "Butter", "Honey", "Tea", "Lime", "Lavender", "Strawberry", "Mint", "Chestnut", "Tree Fruit", "Pear", "Apricot", "Peach", "Blue Cheese", "Menthol", "Coffee", "Tar", "Mango", "Pineapple", "Sage", "Vanilla", "Plum", "Tobacco", "Violet" };
        private readonly string[] PositiveEffects = { "Relaxed", "Hungry", "Euphoric", "Happy", "Creative", "Energetic", "Talkative", "Uplifted", "Tingly", "Sleepy", "Focused", "Giggly", "Aroused" };
        private readonly string[] MedicalEffects = { "Insomnia", "Pain", "Stress", "Fatigue", "Headaches" };

        private readonly string[] WordsToRemove = { "a", "an", "the", "for", "and", "nor", "but", "or", "yet", "so", "at", "around", "by", "after", "along", "from", "of", "on", "to", "with", "without" };

        private readonly ActivityIndicator LoadingActivityIndicator;
        private readonly StackLayout LoadingErrorStack;
        private readonly StackLayout NoResultsStack;
        private readonly Label NoResultsText;

        private string CachedPreviousSearch;

        public SearchPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            string[] subFlavors = Flavors.OrderBy(arg => Guid.NewGuid()).Take(3).ToArray();
            string[] subPositiveEffects = PositiveEffects.OrderBy(arg => Guid.NewGuid()).Take(3).ToArray();
            string[] subMedicalEffects = MedicalEffects.OrderBy(arg => Guid.NewGuid()).Take(3).ToArray();

            string[] merged = subFlavors.Concat(subPositiveEffects).Concat(subMedicalEffects).ToArray();

            Random rand = new Random();
            string[] shuffled = merged.OrderBy(x => rand.Next()).Take(6).ToArray();

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

            foreach (string str in shuffled)
            {
                Color color;

                if (Flavors.Contains(str))
                {
                    color = Color.FromHex("#e349c2");
                }
                else if (PositiveEffects.Contains(str))
                {
                    color = Color.FromHex("#00b368");
                }
                else if (MedicalEffects.Contains(str))
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

                StackLayout SuggestedLabelStack = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Vertical
                };

                string text;

                if (MedicalEffects.Contains(str))
                {
                    text = "treats " + str.ToLower();
                }
                else
                {
                    text = str.ToLower();
                }

                SuggestedLabelStack.Children.Add(new Label
                {
                    FontSize = 22,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 10),
                    Text = text,
                    TextColor = color
                });

                TapGestureRecognizer SuggestedLabelTapGesture = new TapGestureRecognizer();
                SuggestedLabelTapGesture.Tapped += (s, e) =>
                {
                    SearchAsync(str);
                };

                SuggestedLabelStack.GestureRecognizers.Add(SuggestedLabelTapGesture);

                SuggestedStack.Children.Add(SuggestedLabelStack);
            }

            ContentStack.Children.Add(SuggestedStack);

            LoadingActivityIndicator = new ActivityIndicator
            {
                Color = Color.Gray,
                IsEnabled = false,
                IsRunning = true,
                IsVisible = false
            };

            SearchRelativeLayout.Children.Add(LoadingActivityIndicator,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getLoadingWheelWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getLoadingWheelHeight(parent) / 2)));

            double getLoadingWheelWidth(RelativeLayout parent) => LoadingActivityIndicator.Measure(parent.Width, parent.Height).Request.Width;
            double getLoadingWheelHeight(RelativeLayout parent) => LoadingActivityIndicator.Measure(parent.Width, parent.Height).Request.Height;

            LoadingErrorStack = new StackLayout
            {
                IsEnabled = false,
                IsVisible = false
            };

            LoadingErrorStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 27,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Search Error",
                TextColor = Color.FromHex("#333333")
            });

            LoadingErrorStack.Children.Add(new Label
            {
                FontSize = 23,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Please tap to retry.",
                TextColor = Color.Gray
            });

            TapGestureRecognizer LoadingRetryTapGesture = new TapGestureRecognizer();
            LoadingRetryTapGesture.Tapped += (s, e) =>
            {
                SearchAsync(CachedPreviousSearch);
            };

            LoadingErrorStack.GestureRecognizers.Add(LoadingRetryTapGesture);

            SearchRelativeLayout.Children.Add(LoadingErrorStack,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getLoadingErrorTextWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getLoadingErrorTextHeight(parent) / 2)));

            double getLoadingErrorTextWidth(RelativeLayout parent) => LoadingErrorStack.Measure(parent.Width, parent.Height).Request.Width;
            double getLoadingErrorTextHeight(RelativeLayout parent) => LoadingErrorStack.Measure(parent.Width, parent.Height).Request.Height;

            NoResultsStack = new StackLayout
            {
                IsEnabled = false,
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
        }

        private async Task SearchAsync(string s)
        {
            searchBar.IsEnabled = false;

            if (!string.IsNullOrWhiteSpace(s))
            {
                string[] words = System.Text.RegularExpressions.Regex.Split(s.Trim(), @"\s+");

                ArrayList t = new ArrayList();

                foreach (string word in words)
                {
                    if (word.Length > 1 && !WordsToRemove.Contains(word.ToLower()))
                    {
                        t.Add(word);
                    }
                }

                string[] filteredWords = (string[])t.ToArray(typeof(string));

                if (filteredWords.Count() > 0)
                {
                    searchBar.Text = s;
                    CachedPreviousSearch = s;

                    LoadingErrorStack.IsEnabled = false;
                    LoadingErrorStack.IsVisible = false;

                    NoResultsStack.IsEnabled = false;
                    NoResultsStack.IsVisible = false;

                    ContentStack.Children.Clear();

                    LoadingActivityIndicator.IsEnabled = true;
                    LoadingActivityIndicator.IsVisible = true;

                    await Task.Delay(250);

                    try
                    {
                        SearchResult searchResult = await FirebaseBackend.SearchAsync(filteredWords);

                        if (searchResult.Items.Count() > 0)
                        {
                            foreach (Item item in searchResult.Items)
                            {
                                ContentStack.Children.Add(new SearchItem(item));
                            }
                        }
                        else
                        {
                            NoResultsText.Text = "for \"" + s + "\"";

                            NoResultsStack.IsEnabled = true;
                            NoResultsStack.IsVisible = true;
                        }
                    
                        LoadingActivityIndicator.IsVisible = false;
                        LoadingActivityIndicator.IsEnabled = false;
                        
                        ClearButton.IsVisible = true;
                    }
                    catch (Exception)
                    {
                        LoadingActivityIndicator.IsEnabled = false;
                        LoadingActivityIndicator.IsVisible = false;

                        ContentStack.Children.Clear();

                        LoadingErrorStack.IsEnabled = true;
                        LoadingErrorStack.IsVisible = true;
                    }
                }
            }

            searchBar.IsEnabled = true;
        }

        private void ClearSearch(object sender, EventArgs e)
        {
            ClearButton.IsVisible = false;

            ContentStack.Children.Clear();
            searchBar.Text = "";

            LoadingErrorStack.IsEnabled = false;
            LoadingErrorStack.IsVisible = false;

            NoResultsStack.IsEnabled = false;
            NoResultsStack.IsVisible = false;

            ContentStack.Children.Add(SuggestedStack);
        }

        private void SearchButtonPressed(object sender, EventArgs e)
        {
            SearchAsync(searchBar.Text);
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
