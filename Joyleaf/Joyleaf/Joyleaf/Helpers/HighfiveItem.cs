using System;
using Xamarin.Forms;

namespace Joyleaf.Helpers
{
    public class HighfiveItem : Frame
    {
        private Result result;

        public HighfiveItem(Result result, int position)
        {
            this.result = result;

            BackgroundColor = Color.Transparent;
            HasShadow = false;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            Padding = new Thickness(17);
            

            StackLayout stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            stack.Children.Add(new Frame
            {
                BackgroundColor = Color.FromHex("#ffa742"),
                BorderColor = Color.FromHex("#ffa742"),
                Content = new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 23,
                    Margin = new Thickness(25, 7),
                    Text = "" + ((int)(result.MatchPercent * 100)) + "% match!",
                    TextColor = Color.White
                },
                CornerRadius = 20,
                Padding = 0,
                HasShadow = false,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 0, 0, 20)
            });

            stack.Children.Add(new HighfiveStrain(result.Item, position));

            Content = stack;
        }
    }
}
