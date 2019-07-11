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
            HeightRequest = 90;
            WidthRequest = 140;

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
                    Margin = new Thickness(0, 4, 0, 0),
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
                    Source = "Hybrid2"
                });
            }

            Content = detailStack;
        }
    }
}
