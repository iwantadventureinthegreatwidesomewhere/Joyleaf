using Joyleaf.Helpers;
using Syncfusion.SfRating.XForms;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class ContentItemView : ContentPage
    {
        public ContentItemView(Item item)
        {
            InitializeComponent();

            double ratingScore = 3.52213;
            double numberOfRatings = 273;

            HeaderStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 25,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 3),
                Text = item.Name,
                TextColor = Color.FromHex("#333333")
            });

            HeaderStack.Children.Add(new Label
            {
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 10),
                Text = "By " + item.Brand,
                TextColor = Color.FromHex("#636363")
            });

            StackLayout RatingStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 20),
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Fill
            };

            string number = "" + ratingScore;
            var characteristic = number.Substring(0, number.IndexOf("."));
            var mantissa = number.Substring(number.IndexOf("."));

            var fs = new FormattedString();
            fs.Spans.Add(new Span { Text = characteristic, TextColor = Color.FromHex("#333333"), FontAttributes = FontAttributes.Bold, FontSize = 20 });
            fs.Spans.Add(new Span { Text = mantissa.Length > 2 ? mantissa.Substring(0, 3) : mantissa, TextColor = Color.FromHex("#333333"), FontAttributes = FontAttributes.Bold, FontSize = 15});

            Label formattedRatingScore = new Label
            {
                Margin = new Thickness(0, 0, 3, 0),
                FormattedText = fs,
                VerticalOptions = LayoutOptions.Center
            };

            RatingStack.Children.Add(formattedRatingScore);

            SfRating rating = new SfRating
            {
                ItemCount = 5,
                ItemSize = 17,
                Margin = new Thickness(0, 0, 3, 0),
                Precision = Precision.Exact,
                ReadOnly = true,
                Value = 3.5,
                VerticalOptions = LayoutOptions.Center
            };

            rating.RatingSettings.RatedFill = Color.Orange;
            rating.RatingSettings.RatedStroke = Color.Transparent;
            rating.RatingSettings.UnRatedFill = Color.LightGray;
            rating.RatingSettings.UnRatedStroke = Color.Transparent;

            RatingStack.Children.Add(rating);

            RatingStack.Children.Add(new Label
            {
                FontSize = 15,
                Text = "(" + numberOfRatings + ")",
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.Center
            });

            HeaderStack.Children.Add(RatingStack);

            HeaderStack.Children.Add(new  Button
            {
                BackgroundColor = Color.FromHex("#00c88c"),
                CornerRadius = 20,
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                HeightRequest = 40,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 5),
                Text = "BUY NOW",
                TextColor = Color.White,
                WidthRequest = 120
            });

            StackLayout AvailabilityStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Fill
            };

            if (item.Availability == true)
            {
                AvailabilityStack.Children.Add(new Image { Source = "Check", VerticalOptions = LayoutOptions.Center });

                AvailabilityStack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 13,
                    Text = "Available",
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Center
                });
            }
            else
            {
                AvailabilityStack.Children.Add(new Image { Source = "XMark", VerticalOptions = LayoutOptions.Center });

                AvailabilityStack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 13,
                    Text = "Out of stock",
                    TextColor = Color.Gray,
                    VerticalOptions = LayoutOptions.Center
                });
            }

            HeaderStack.Children.Add(AvailabilityStack);

            Grid DetailGrid = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                HeightRequest = 225
            };

            DetailGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) });
            DetailGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) });
            DetailGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Star) });
            DetailGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50, GridUnitType.Star) });

            StackLayout SpeciesStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical
            };

            SpeciesStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 10),
                Text = "Species",
                TextColor = Color.FromHex("#333333")
            });

            DetailGrid.Children.Add(SpeciesStack, 0, 0);

            StackLayout StrengthStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical
            };

            StrengthStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 10),
                Text = "Strength",
                TextColor = Color.FromHex("#333333")
            });

            DetailGrid.Children.Add(StrengthStack, 1, 0);

            StackLayout PotencyStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical
            };

            PotencyStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center,
                Text = "Potency",
                TextColor = Color.FromHex("#333333")
            });

            StackLayout ThcCbdStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal
            };

            StackLayout ThcStack = new StackLayout
            {
                Margin = new Thickness(0, 0, 15, 0),
                Orientation = StackOrientation.Vertical
            };

            ThcStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 16),
                Text = item.Thc,
                TextColor = Color.FromHex("#0281ff")
            });

            ThcStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center,
                Text = "THC",
                TextColor = Color.FromHex("#636363")
            });

            ThcCbdStack.Children.Add(ThcStack);

            StackLayout CbdStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical
            };

            CbdStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 16),
                Text = item.Cbd,
                TextColor = Color.FromHex("#0281ff")
            });

            CbdStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center,
                Text = "CBD",
                TextColor = Color.FromHex("#636363")
            });

            ThcCbdStack.Children.Add(CbdStack);

            PotencyStack.Children.Add(ThcCbdStack);

            DetailGrid.Children.Add(PotencyStack, 0, 1);

            StackLayout CategoryStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical
            };

            CategoryStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 10),
                Text = "Type",
                TextColor = Color.FromHex("#333333")
            });

            switch (item.Category)
            {
                case "Capsules":
                    CategoryStack.Children.Add(new Image { Source = "Capsules", HeightRequest = 30, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 0, 0, 10) });
                    break;
                case "Dried flowers":
                    CategoryStack.Children.Add(new Image { Source = "DriedFlowers", HeightRequest = 30, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 0, 0, 10) });
                    break;
                case "Oils":
                    CategoryStack.Children.Add(new Image { Source = "Oils", HeightRequest = 30, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 0, 0, 10) });
                    break;
                case "Oral sprays":
                    CategoryStack.Children.Add(new Image { Source = "OralSprays", HeightRequest = 30, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 0, 0, 10) });
                    break;
                case "Pre-rolled":
                    CategoryStack.Children.Add(new Image { Source = "PreRolled", HeightRequest = 30, HorizontalOptions = LayoutOptions.Center, Margin = new Thickness(0, 0, 0, 10) });
                    break;
            }

            CategoryStack.Children.Add(new Label{
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center,
                Text = item.Category,
                TextColor = Color.FromHex("#636363")
            });

            DetailGrid.Children.Add(CategoryStack, 1, 1);

            DetailStack.Children.Add(DetailGrid);
        }
    }
}