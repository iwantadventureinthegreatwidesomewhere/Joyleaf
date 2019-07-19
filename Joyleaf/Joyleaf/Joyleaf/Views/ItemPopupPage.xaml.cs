using System;
using System.Collections.Generic;
using Joyleaf.Helpers;
using Rg.Plugins.Popup.Pages;
using Syncfusion.SfRating.XForms;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class ItemPopupPage : PopupPage
    {
        public ItemPopupPage(Item item)
        {
            double ratingScore = 3.52213;
            double numberOfRatings = 273;

            InitializeComponent();

            ItemName.Text = item.Name;
            ItemDesc.Text = item.Desc;

            if (item.Race == Race.Sativa)
            {
                SpeciesStack.Children.Add(new Image
                {
                    HeightRequest = 30,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 0, 3, 0),
                    Source = "Sativa"
                });

                SpeciesStack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 16,
                    Text = "SATIVA",
                    TextColor = Color.FromHex("#ffa742"),
                    VerticalOptions = LayoutOptions.Center
                });
            }
            else if (item.Race == Race.Indica)
            {
                SpeciesStack.Children.Add(new Image
                {
                    HeightRequest = 30,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 0, 3, 0),
                    Source = "Indica"
                });

                SpeciesStack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 16,
                    Text = "INDICA",
                    TextColor = Color.FromHex("#774dff"),
                    VerticalOptions = LayoutOptions.Center
                });
            }
            else if (item.Race == Race.Hybrid)
            {
                SpeciesStack.Children.Add(new Image
                {
                    HeightRequest = 30,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 0, 3, 0),
                    Source = "Hybrid"
                });

                SpeciesStack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 16,
                    Text = "HYBRID",
                    TextColor = Color.FromHex("#00b368"),
                    VerticalOptions = LayoutOptions.Center
                });
            }

            string number = "" + ratingScore;
            var characteristic = number.Substring(0, number.IndexOf("."));
            var mantissa = number.Substring(number.IndexOf("."));

            var fs = new FormattedString();
            fs.Spans.Add(new Span { Text = characteristic, TextColor = Color.FromHex("#333333"), FontAttributes = FontAttributes.Bold, FontSize = 20 });
            fs.Spans.Add(new Span { Text = mantissa.Length > 2 ? mantissa.Substring(0, 3) : mantissa, TextColor = Color.FromHex("#333333"), FontAttributes = FontAttributes.Bold, FontSize = 15 });

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

            rating.RatingSettings.RatedFill = Color.FromHex("#ffa742");
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
        }
    }
}
