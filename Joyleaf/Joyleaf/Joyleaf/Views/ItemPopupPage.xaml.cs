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

            Stack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 25,
                Margin = new Thickness(0, 0, 0, 3),
                Text = item.Name,
                TextColor = Color.FromHex("#333333")
            });

            StackLayout SpeciesStack = new StackLayout
            {
                Margin = new Thickness(0, 0, 0, 3),
                Orientation = StackOrientation.Horizontal
            };

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
                    FontSize = 15,
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
                    FontSize = 15,
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
                    FontSize = 15,
                    Text = "HYBRID",
                    TextColor = Color.FromHex("#00b368"),
                    VerticalOptions = LayoutOptions.Center
                });
            }

            Stack.Children.Add(SpeciesStack);

            StackLayout RatingStack = new StackLayout
            {
                Margin = new Thickness(0, 0, 0, 10),
                Orientation = StackOrientation.Horizontal
            };

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

            Stack.Children.Add(RatingStack);

            if (!String.IsNullOrEmpty(item.Desc))
            {
                Stack.Children.Add(new Label
                {
                    FontSize = 15,
                    Margin = new Thickness(0, 0, 0, 10),
                    Text = item.Desc,
                    TextColor = Color.FromHex("#333333")
                });
            }

            if(item.Flavors != null)
            {
                Stack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 17,
                    Text = "FLAVORS",
                    TextColor = Color.FromHex("#e349c2")
                });

                FlexLayout FlavorLayout = new FlexLayout
                {
                    AlignContent = FlexAlignContent.Start,
                    AlignItems = FlexAlignItems.Start,
                    Direction = FlexDirection.Row,
                    Margin = new Thickness(0, 0, 0, 10),
                    Wrap = FlexWrap.Wrap
                };

                foreach (KeyValuePair<string, string> entry in item.Flavors)
                {
                    FlavorLayout.Children.Add(new Frame
                    {
                        BackgroundColor = Color.Transparent,
                        BorderColor = Color.FromHex("#e349c2"),
                        Content = new Label
                        {
                            FontSize = 15,
                            Margin = new Thickness(15, 5),
                            Text = entry.Value,
                            TextColor = Color.FromHex("#e349c2")
                        },
                        CornerRadius = 10,
                        Margin = new Thickness(0, 5, 0, 5),
                        Padding = 0,
                        HasShadow = false
                    });

                    FlavorLayout.Children.Add(new BoxView
                    {
                        BackgroundColor = Color.Transparent,
                        HeightRequest = 0,
                        WidthRequest = 10
                    });
                }

                Stack.Children.Add(FlavorLayout);
            }

            if(item.Effects.Medical != null || item.Effects.Negative != null || item.Effects.Positive != null)
            {
                Stack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 17,
                    Text = "EFFECTS",
                    TextColor = Color.FromHex("#3269e6")
                });

                FlexLayout EffectLayout = new FlexLayout
                {
                    AlignContent = FlexAlignContent.Start,
                    AlignItems = FlexAlignItems.Start,
                    Direction = FlexDirection.Row,
                    Margin = new Thickness(0, 0, 0, 10),
                    Wrap = FlexWrap.Wrap
                };

                if (item.Effects.Positive != null)
                {
                    foreach (KeyValuePair<string, string> entry in item.Effects.Positive)
                    {
                        EffectLayout.Children.Add(new Frame
                        {
                            BackgroundColor = Color.Transparent,
                            BorderColor = Color.FromHex("#00b368"),
                            Content = new Label
                            {
                                FontSize = 15,
                                Margin = new Thickness(15, 5),
                                Text = entry.Value,
                                TextColor = Color.FromHex("#00b368")
                            },
                            CornerRadius = 10,
                            Margin = new Thickness(0, 5, 0, 5),
                            Padding = 0,
                            HasShadow = false
                        });

                        EffectLayout.Children.Add(new BoxView
                        {
                            BackgroundColor = Color.Transparent,
                            HeightRequest = 0,
                            WidthRequest = 10
                        });
                    }
                }

                if (item.Effects.Negative != null)
                {
                    foreach (KeyValuePair<string, string> entry in item.Effects.Negative)
                    {
                        EffectLayout.Children.Add(new Frame
                        {
                            BackgroundColor = Color.Transparent,
                            BorderColor = Color.FromHex("#EC5B55"),
                            Content = new Label
                            {
                                FontSize = 15,
                                Margin = new Thickness(15, 5),
                                Text = entry.Value,
                                TextColor = Color.FromHex("#EC5B55")
                            },
                            CornerRadius = 10,
                            Margin = new Thickness(0, 5, 0, 5),
                            Padding = 0,
                            HasShadow = false
                        });

                        EffectLayout.Children.Add(new BoxView
                        {
                            BackgroundColor = Color.Transparent,
                            HeightRequest = 0,
                            WidthRequest = 10
                        });
                    }
                }

                if (item.Effects.Medical != null)
                {
                    foreach (KeyValuePair<string, string> entry in item.Effects.Medical)
                    {
                        EffectLayout.Children.Add(new Frame
                        {
                            BackgroundColor = Color.Transparent,
                            BorderColor = Color.FromHex("#3269e6"),
                            Content = new Label
                            {
                                FontSize = 15,
                                Margin = new Thickness(15, 5),
                                Text = entry.Value + " Relief",
                                TextColor = Color.FromHex("#3269e6")
                            },
                            CornerRadius = 10,
                            Margin = new Thickness(0, 5, 0, 5),
                            Padding = 0,
                            HasShadow = false
                        });

                        EffectLayout.Children.Add(new BoxView
                        {
                            BackgroundColor = Color.Transparent,
                            HeightRequest = 0,
                            WidthRequest = 10
                        });
                    }
                }

                Stack.Children.Add(EffectLayout);
            }



        }
    }
}
