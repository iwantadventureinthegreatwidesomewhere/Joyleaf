using System;
using Joyleaf.Views;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfRating.XForms;
using Xamarin.Forms;

namespace Joyleaf.Helpers
{
    public class CategoryItem : Frame, ItemInterface
    {
        private readonly Item item;
        private SfRating rating;

        public CategoryItem(Item item)
        {
            this.item = item;

            CornerRadius = 20;
            Padding = 10;
            HasShadow = false;
            HeightRequest = 110;
            WidthRequest = 133;

            StackLayout detailStack = new StackLayout{
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            if (item.Info.Race == Race.Sativa)
            {
                BackgroundColor = Color.FromHex("#ffa742");
                detailStack.Children.Add(new Image
                {
                    HeightRequest = 25,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 0, 0, 5),
                    Source = "Sativa2"
                });
            }
            else if (item.Info.Race == Race.Indica)
            {
                BackgroundColor = Color.FromHex("#774dff");
                detailStack.Children.Add(new Image
                {
                    HeightRequest = 17,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 4, 0, 9),
                    Source = "Indica2"
                });
            }
            else if (item.Info.Race == Race.Hybrid)
            {
                BackgroundColor = Color.FromHex("#00b368");
                detailStack.Children.Add(new Image
                {
                    HeightRequest = 25,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 0, 0, 5),
                    Source = "Hybrid2"
                });
            }

            detailStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 17,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 3),
                Text = Truncate(item.Info.Name, 17),
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand
            });

            rating = new SfRating
            {
                HorizontalOptions = LayoutOptions.Center,
                ItemCount = 5,
                ItemSize = 13,
                Margin = new Thickness(0, 0, 0, 3),
                Precision = Precision.Exact,
                ReadOnly = true,
                Value = item.Reviews.AverageRating,
                VerticalOptions = LayoutOptions.EndAndExpand
            };

            rating.RatingSettings.RatedFill = Color.White;
            rating.RatingSettings.RatedStroke = Color.White;
            rating.RatingSettings.RatedStrokeWidth = 1;
            rating.RatingSettings.UnRatedFill = Color.Transparent;
            rating.RatingSettings.UnRatedStroke = Color.White;
            rating.RatingSettings.UnRatedStrokeWidth = 1;

            detailStack.Children.Add(rating);

            TapGestureRecognizer TapGesture = new TapGestureRecognizer();
            TapGesture.Tapped += PushItemViewAsync;
            GestureRecognizers.Add(TapGesture);

            Content = detailStack;
        }

        private void PushItemViewAsync(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new ItemPopupPage(item, this));
        }

        private string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        void ItemInterface.updateRating(double averageRating)
        {
            rating.Value = averageRating;
        }
    }
}
