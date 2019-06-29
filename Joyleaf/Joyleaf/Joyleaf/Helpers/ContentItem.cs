using Joyleaf.Views;
using Syncfusion.SfRating.XForms;
using System;
using Xamarin.Forms;

namespace Joyleaf.Helpers
{
    public class ContentItem : Grid
    {
        public readonly Item item;
        public readonly ContentItemView contentItemView;

        public ContentItem(Item item, bool islastToLoad)
        {
            this.item = item;
            contentItemView = new ContentItemView(item);

            HeightRequest = 70;
            Margin = new Thickness(0, 0, 0, 3);

            TapGestureRecognizer TapGesture = new TapGestureRecognizer();
            TapGesture.Tapped += PushItemViewAsync;
            GestureRecognizers.Add(TapGesture);

            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(99, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            if (!islastToLoad)
            {
                Children.Add(new BoxView
                {
                    BackgroundColor = Color.LightGray,
                    HeightRequest = 1,
                    HorizontalOptions = LayoutOptions.Fill
                }, 1, 1);
            }

            StackLayout symbolStack = new StackLayout { HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };

            switch (item.Category)
            {
                case "Capsules":
                    symbolStack.Children.Add(new Image { Source = "Capsules" });
                    break;
                case "Dried flowers":
                    symbolStack.Children.Add(new Image { Source = "DriedFlowers" });
                    break;
                case "Oils":
                    symbolStack.Children.Add(new Image { Source = "Oils" });
                    break;
                case "Oral sprays":
                    symbolStack.Children.Add(new Image { Source = "OralSprays" });
                    break;
                case "Pre-rolled":
                    symbolStack.Children.Add(new Image { Source = "PreRolled" });
                    break;
            }

            Children.Add(symbolStack, 0, 0);

            StackLayout detailStack = new StackLayout { HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Center };

            detailStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                Text = Truncate(item.Name, 15),
                TextColor = Color.FromHex("#333333")
            });

            detailStack.Children.Add(new Label
            {
                FontSize = 13,
                Text = Truncate("By " + item.Brand, 20),
                TextColor = Color.Gray
            });

            SfRating rating = new SfRating
            {
                ItemCount = 5,
                ItemSize = 13,
                Precision = Precision.Half,
                ReadOnly = true,
                Value = 3.5
            };

            rating.RatingSettings.RatedFill = Color.Orange;
            rating.RatingSettings.RatedStroke = Color.Transparent;
            rating.RatingSettings.UnRatedFill = Color.LightGray;
            rating.RatingSettings.UnRatedStroke = Color.Transparent;

            detailStack.Children.Add(rating);
            Children.Add(detailStack, 1, 0);

            StackLayout moreStack = new StackLayout { HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Center };

            Button seeMore = new Button
            {
                BackgroundColor = Color.FromHex("#ececec"),
                CornerRadius = 15,
                FontSize = 12,
                HeightRequest = 30,
                Text = "MORE",
                TextColor = Color.Gray,
                WidthRequest = 60
            };

            seeMore.Clicked += PushItemViewAsync;

            moreStack.Children.Add(seeMore);
            Children.Add(moreStack, 2, 0);
        }

        private async void PushItemViewAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(contentItemView);
        }

        private string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
    }
}