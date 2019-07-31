using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class SearchPage : ContentPage
    {
        private readonly StackLayout SuggestedTopicsStack;

        private readonly string[] Flavors = { "Earthy", "Chemical", "Pine", "Spicy/Herbal", "Pungent", "Pepper", "Flowery", "Citrus", "Orange", "Sweet", "Skunk", "Grape", "Minty", "Woody", "Cheese", "Diesel", "Tropical", "Grapefruit", "Nutty", "Lemon", "Berry", "Blueberry", "Ammonia", "Apple", "Rose", "Butter", "Honey", "Tea", "Lime", "Lavender", "Strawberry", "Mint", "Chestnut", "Tree Fruit", "Pear", "Apricot", "Peach", "Blue Cheese", "Menthol", "Coffee", "Tar", "Mango", "Pineapple", "Sage", "Vanilla", "Plum", "Tobacco", "Violet" };
        private readonly string[] PositiveEffects = { "Relaxed", "Hungry", "Euphoric", "Happy", "Creative", "Energetic", "Talkative", "Uplifted", "Tingly", "Sleepy", "Focused", "Giggly", "Aroused" };
        private readonly string[] MedicalEffects = { "Insomnia", "Pain", "Stress", "Fatigue", "Headaches" };

        private readonly string[] wordsToRemove = { "a", "an", "the", "for", "and", "nor", "but", "or", "yet", "so", "at", "around", "by", "after", "along", "from", "of", "on", "to", "with", "without" };

        private readonly ActivityIndicator LoadingWheel;
        private readonly StackLayout LoadingErrorText;
        private readonly StackLayout NoResultsText;
        private Label NoResultsDetail;

        private string cachedSearchString;

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

            SuggestedTopicsStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(10,0),
                Orientation = StackOrientation.Vertical
            };

            SuggestedTopicsStack.Children.Add(new Label
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

                SuggestedTopicsStack.Children.Add(new BoxView
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

                SuggestedTopicsStack.Children.Add(SuggestedLabelStack);
            }

            ContentStack.Children.Add(SuggestedTopicsStack);

            //######################################################

            LoadingWheel = new ActivityIndicator
            {
                Color = Color.Gray,
                IsEnabled = false,
                IsRunning = true,
                IsVisible = false
            };

            SearchRelativeLayout.Children.Add(LoadingWheel,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getLoadingWheelWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getLoadingWheelHeight(parent) / 2)));

            double getLoadingWheelWidth(RelativeLayout parent) => LoadingWheel.Measure(parent.Width, parent.Height).Request.Width;
            double getLoadingWheelHeight(RelativeLayout parent) => LoadingWheel.Measure(parent.Width, parent.Height).Request.Height;

            //######################################################

            LoadingErrorText = new StackLayout
            {
                IsEnabled = false,
                IsVisible = false
            };

            LoadingErrorText.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 27,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Search Error",
                TextColor = Color.FromHex("#333333")
            });

            LoadingErrorText.Children.Add(new Label
            {
                FontSize = 23,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "Please tap to retry.",
                TextColor = Color.Gray
            });

            TapGestureRecognizer LoadingRetryTapGesture = new TapGestureRecognizer();
            LoadingRetryTapGesture.Tapped += (s, e) =>
            {
                SearchAsync(cachedSearchString);
            };

            LoadingErrorText.GestureRecognizers.Add(LoadingRetryTapGesture);

            SearchRelativeLayout.Children.Add(LoadingErrorText,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getLoadingErrorTextWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getLoadingErrorTextHeight(parent) / 2)));

            double getLoadingErrorTextWidth(RelativeLayout parent) => LoadingErrorText.Measure(parent.Width, parent.Height).Request.Width;
            double getLoadingErrorTextHeight(RelativeLayout parent) => LoadingErrorText.Measure(parent.Width, parent.Height).Request.Height;

            //######################################################

            NoResultsText = new StackLayout
            {
                IsEnabled = false,
                IsVisible = false,
            };

            NoResultsText.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 35,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = "No Results",
                TextColor = Color.FromHex("#333333")
            });

            NoResultsDetail = new Label
            {
                FontSize = 23,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.Gray
            };

            NoResultsText.Children.Add(NoResultsDetail);

            SearchRelativeLayout.Children.Add(NoResultsText,
                Constraint.RelativeToParent(parent => (parent.Width / 2) - (getNoResultsTextWidth(parent) / 2)),
                Constraint.RelativeToParent(parent => (parent.Height / 2) - (getNoResultsTextHeight(parent) / 2)));

            double getNoResultsTextWidth(RelativeLayout parent) => NoResultsText.Measure(parent.Width, parent.Height).Request.Width;
            double getNoResultsTextHeight(RelativeLayout parent) => NoResultsText.Measure(parent.Width, parent.Height).Request.Height;

            //######################################################

            CrossConnectivity.Current.ConnectivityChanged += HandleConnectivityChanged;
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
                    if (word.Length > 1 && !wordsToRemove.Contains(word.ToLower()))
                    {
                        t.Add(word);
                    }
                }

                string[] filteredWords = (string[])t.ToArray(typeof(string));

                if (filteredWords.Count() > 0)
                {
                    searchBar.Text = s;
                    cachedSearchString = s;

                    LoadingErrorText.IsEnabled = false;
                    LoadingErrorText.IsVisible = false;

                    NoResultsText.IsEnabled = false;
                    NoResultsText.IsVisible = false;

                    scrollView.IsEnabled = false;

                    ContentStack.Children.Clear();

                    LoadingWheel.IsEnabled = true;
                    LoadingWheel.IsVisible = true;

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
                            NoResultsDetail.Text = "for \"" + s + "\"";

                            NoResultsText.IsEnabled = true;
                            NoResultsText.IsVisible = true;
                        }
                    
                        LoadingWheel.IsVisible = false;
                        LoadingWheel.IsEnabled = false;
                        
                        ClearButton.IsVisible = true;

                        scrollView.IsEnabled = true;
                    }
                    catch (Exception)
                    {
                        LoadingWheel.IsEnabled = false;
                        LoadingWheel.IsVisible = false;

                        scrollView.IsEnabled = false;

                        ContentStack.Children.Clear();

                        LoadingErrorText.IsEnabled = true;
                        LoadingErrorText.IsVisible = true;
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

            LoadingErrorText.IsEnabled = false;
            LoadingErrorText.IsVisible = false;

            NoResultsText.IsEnabled = false;
            NoResultsText.IsVisible = false;

            ContentStack.Children.Add(SuggestedTopicsStack);
        }

        private void SearchButtonPressed(object sender, EventArgs e)
        {
            SearchAsync(searchBar.Text);
        }

        private async void HandleConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await Navigation.PopAsync();
            }
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
