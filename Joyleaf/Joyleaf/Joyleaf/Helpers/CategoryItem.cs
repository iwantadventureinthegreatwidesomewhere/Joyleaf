using System;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfRating.XForms;
using Xamarin.Forms;

namespace Joyleaf.Helpers
{
    public class CategoryItem : Frame
    {
        private readonly Item item;

        public CategoryItem(Item item)
        {
            this.item = item;

            CornerRadius = 20;
            Padding = 10;
            HasShadow = false;
            HeightRequest = 100;
            WidthRequest = 133;

            StackLayout detailStack = new StackLayout{
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            if (item.Race == Race.Sativa)
            {
                BackgroundColor = Color.FromHex("f3ba6d");
                detailStack.Children.Add(new Image
                {
                    HeightRequest = 25,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 0, 0, 5),
                    Source = "Sativa2"
                });
            }
            else if (item.Race == Race.Indica)
            {
                BackgroundColor = Color.FromHex("5D98F7");
                detailStack.Children.Add(new Image
                {
                    HeightRequest = 17,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 4, 0, 9),
                    Source = "Indica2"
                });
            }
            else if (item.Race == Race.Hybrid)
            {
                BackgroundColor = Color.FromHex("#6fc294");
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
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                Text = Truncate(item.Name, 20),
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand
            });

            SfRating rating = new SfRating
            {
                HorizontalOptions = LayoutOptions.Center,
                ItemCount = 5,
                ItemSize = 13,
                Margin = new Thickness(0, 0, 0, 3),
                Precision = Precision.Exact,
                ReadOnly = true,
                Value = 3,
                VerticalOptions = LayoutOptions.EndAndExpand
            };

            rating.RatingSettings.RatedFill = Color.White;
            rating.RatingSettings.RatedStroke = Color.Transparent;
            rating.RatingSettings.UnRatedFill = Color.LightGray;
            rating.RatingSettings.UnRatedStroke = Color.Transparent;

            detailStack.Children.Add(rating);



            Content = detailStack;
        }

        private void PushItemViewAsync(object sender, EventArgs e)
        {
            //PopupNavigation.Instance.PushAsync(productPopupPage);
        }

        private string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
    }
}