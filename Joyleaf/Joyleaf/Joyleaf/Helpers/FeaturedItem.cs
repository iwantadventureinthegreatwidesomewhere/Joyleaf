using System;
using System.Collections.Generic;
using System.Linq;
using Joyleaf.CustomControls;
using Joyleaf.Views;
using Rg.Plugins.Popup.Services;
using Syncfusion.SfRating.XForms;
using Xamarin.Forms;

namespace Joyleaf.Helpers
{
    public class FeaturedItem : ShadowFrame
    {
        private readonly Item item;

        public FeaturedItem(Item item)
        {
            double ratingScore = 3.5233;

            this.item = item;

            CornerRadius = 30;
            Margin = new Thickness(17);

            StackLayout stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Fill,
                Orientation = StackOrientation.Vertical,
                Padding = 10,
                VerticalOptions = LayoutOptions.Fill
            };

            StackLayout header = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            header.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 17,
                HorizontalOptions = LayoutOptions.Start,
                Text = "FEATURED STRAIN",
                TextColor = Color.Gray
            });

            if (item.Race == Race.Sativa)
            {
                header.Children.Add(new Image
                {
                    HeightRequest = 45,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    Source = "FeaturedSativa"
                });
            }
            else if (item.Race == Race.Indica)
            {
                header.Children.Add(new Image
                {
                    HeightRequest = 35,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    Margin = new Thickness(0, 0, 0, 10),
                    Source = "FeaturedIndica"
                });
            }
            else if (item.Race == Race.Hybrid)
            {
                header.Children.Add(new Image
                {
                    HeightRequest = 40,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    Margin = new Thickness(0, 0, 0, 5),
                    Source = "FeaturedHybrid"
                });
            }

            stack.Children.Add(header);

            stack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 23,
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 10, 0, 5),
                Text = Truncate(item.Name, 20),
                TextColor = Color.FromHex("#333333")
            });

            SfRating rating = new SfRating
            {
                ItemCount = 5,
                ItemSize = 17,
                Precision = Precision.Exact,
                ReadOnly = true,
                Value = ratingScore,
                VerticalOptions = LayoutOptions.Center
            };

            rating.RatingSettings.RatedFill = Color.FromHex("#ffa742");
            rating.RatingSettings.RatedStroke = Color.Transparent;
            rating.RatingSettings.UnRatedFill = Color.LightGray;
            rating.RatingSettings.UnRatedStroke = Color.Transparent;

            stack.Children.Add(rating);

            if (!String.IsNullOrEmpty(item.Desc))
            {
                stack.Children.Add(new Label
                {
                    FontSize = 15,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, 10, 0, 0),
                    Text = Truncate(item.Desc, 200),
                    TextColor = Color.Gray
                });
            }

            if (item.Flavors != null || item.Effects.Positive != null)
            {
                FlexLayout flexLayout = new FlexLayout
                {
                    AlignContent = FlexAlignContent.Start,
                    AlignItems = FlexAlignItems.Start,
                    Direction = FlexDirection.Row,
                    Margin = new Thickness(0, 10, 0, 5),
                    Wrap = FlexWrap.Wrap
                };

                var flavors = item.Flavors;
                var effects = item.Effects.Positive;

                flavors.ToList().ForEach(x => effects.Add(flavors.Count+x.Key, x.Value));

                Random rand = new Random();
                var shuffled = effects.OrderBy(x => rand.Next()).ToDictionary(x => x.Key, x => x.Value);

                int count = 0;

                foreach (KeyValuePair<string, string> entry in shuffled)
                {
                    count++;

                    if (count <= 7)
                    {
                        Color color;

                        if (item.Flavors.ContainsValue(entry.Value))
                        {
                            color = Color.FromHex("#e349c2");
                        }
                        else
                        {
                            color = Color.FromHex("#00b368");
                        }

                        StackLayout TagStack = new StackLayout
                        {
                            Margin = new Thickness(0, 5, 0, 5),
                            Orientation = StackOrientation.Horizontal
                        };

                        TagStack.Children.Add(new Frame
                        {
                            BackgroundColor = Color.Transparent,
                            BorderColor = color,
                            Content = new Label
                            {
                                FontSize = 15,
                                Margin = new Thickness(15, 5),
                                Text = entry.Value,
                                TextColor = color
                            },
                            CornerRadius = 10,
                            Padding = 0,
                            HasShadow = false
                        });

                        TagStack.Children.Add(new BoxView
                        {
                            HeightRequest = 0,
                            WidthRequest = 5
                        });

                        flexLayout.Children.Add(TagStack);
                    }
                }

                stack.Children.Add(flexLayout);
            }

            TapGestureRecognizer TapGesture = new TapGestureRecognizer();
            TapGesture.Tapped += PushItemViewAsync;
            GestureRecognizers.Add(TapGesture);

            Content = stack;
        }

        private void PushItemViewAsync(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new ItemPopupPage(item));
        }

        private string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
    }
}