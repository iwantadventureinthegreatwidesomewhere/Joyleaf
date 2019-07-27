using System;
using System.Collections.Generic;
using System.Linq;
using Joyleaf.Helpers;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class SearchPage : ContentPage
    {
        private readonly StackLayout SuggestedTopicsStack;

        private readonly string[] Flavors = { "Earthy", "Chemical", "Pine", "Spicy/Herbal", "Pungent", "Pepper", "Flowery", "Citrus", "Orange", "Sweet", "Skunk", "Grape", "Minty", "Woody", "Cheese", "Diesel", "Tropical", "Grapefruit", "Nutty", "Lemon", "Berry", "Blueberry", "Ammonia", "Apple", "Rose", "Butter", "Honey", "Tea", "Lime", "Lavender", "Strawberry", "Mint", "Chestnut", "Tree Fruit", "Pear", "Apricot", "Peach", "Blue Cheese", "Menthol", "Coffee", "Tar", "Mango", "Pineapple", "Sage", "Vanilla", "Plum", "Tobacco", "Violet" };
        private readonly string[] PositiveEffects = { "Relaxed", "Hungry", "Euphoric", "Happy", "Creative", "Energetic", "Talkative", "Uplifted", "Tingly", "Sleepy", "Focused", "Giggly", "Aroused" };
        private readonly string[] MedicalEffects = { "Treats Insomnia", "Treats Pain", "Treats Stress", "Treats Fatigue", "Treats Headaches" };

        public SearchPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            string[] subFlavors = Flavors.OrderBy(arg => Guid.NewGuid()).Take(3).ToArray();
            string[] subPositiveEffects = PositiveEffects.OrderBy(arg => Guid.NewGuid()).Take(3).ToArray();
            string[] subMedicalEffects = MedicalEffects.OrderBy(arg => Guid.NewGuid()).Take(3).ToArray();

            string[] merged = subFlavors.Concat(subPositiveEffects).Concat(subMedicalEffects).ToArray();

            Random rand = new Random();
            string[] shuffled = merged.OrderBy(x => rand.Next()).Take(7).ToArray();

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
                Margin = new Thickness(0, 10),
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

                SuggestedTopicsStack.Children.Add(new BoxView
                {
                    Color = Color.LightGray,
                    HeightRequest = 0.5,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                });

                SuggestedTopicsStack.Children.Add(new Label
                {
                    FontSize = 22,
                    Margin = new Thickness(0, 10),
                    Text = s.ToLower(),
                    TextColor = color
                });
            }

            ContentStack.Children.Add(SuggestedTopicsStack);
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
