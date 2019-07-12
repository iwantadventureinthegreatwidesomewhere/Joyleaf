using Syncfusion.SfRating.XForms;
using Xamarin.Forms;

namespace Joyleaf.Helpers
{
    public class CategoryItem : Frame
    {
        public CategoryItem(string s)
        {
            string species = s;

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

            if (species.Equals("sativa"))
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
            else if (species.Equals("indica"))
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
            else if (species.Equals("hybrid"))
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
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 5),
                Text = Truncate("Lorem ipsum dolor sit amet, consectetur adipiscing", 23),
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand
            });

            SfRating rating = new SfRating
            {
                HorizontalOptions = LayoutOptions.Center,
                ItemCount = 5,
                ItemSize = 13,
                Margin = new Thickness(0, 0, 0, 5),
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

        private string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
    }
}
