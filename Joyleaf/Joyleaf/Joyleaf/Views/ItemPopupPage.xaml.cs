using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Joyleaf.Helpers;
using Joyleaf.Services;
using Rg.Plugins.Popup.Pages;
using Syncfusion.SfRating.XForms;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class ItemPopupPage : PopupPage
    {
        private Item item;
        private ItemInterface mainPageFrame;
        private SfRating headerRating;
        private Label headerNumberOfRatings;
        private Label sectionRating;
        private Label sectionNumberOfRatings;
        private Button postReviewButton;
        private Editor writeReviewEditor;
        private SfRating writeReviewRating;

        public ItemPopupPage(Item item, ItemInterface mainPageFrame)
        {
            InitializeComponent();

            this.item = item;
            this.mainPageFrame = mainPageFrame;

            UpdateRatingAsync();

            Stack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 25,
                Margin = new Thickness(0, 30, 0, 5),
                Text = item.Info.Name,
                TextColor = Color.FromHex("#333333")
            });

            StackLayout SpeciesStack = new StackLayout
            {
                Margin = new Thickness(0, 0, 0, 5),
                Orientation = StackOrientation.Horizontal
            };

            if (item.Info.Race == Race.Sativa)
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
            else if (item.Info.Race == Race.Indica)
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
            else if (item.Info.Race == Race.Hybrid)
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
                Margin = new Thickness(0, 0, 0, 7),
                Orientation = StackOrientation.Horizontal
            };

            headerRating = new SfRating
            {
                ItemCount = 5,
                ItemSize = 17,
                Margin = new Thickness(0, 0, 3, 3),
                Precision = Precision.Exact,
                ReadOnly = true,
                VerticalOptions = LayoutOptions.Center
            };

            headerRating.RatingSettings.RatedFill = Color.FromHex("#ffa742");
            headerRating.RatingSettings.RatedStroke = Color.Transparent;
            headerRating.RatingSettings.UnRatedFill = Color.LightGray;
            headerRating.RatingSettings.UnRatedStroke = Color.Transparent;

            RatingStack.Children.Add(headerRating);

            headerNumberOfRatings = new Label
            {
                FontSize = 15,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.Center
            };

            RatingStack.Children.Add(headerNumberOfRatings);

            Stack.Children.Add(RatingStack);

            if (!String.IsNullOrEmpty(item.Info.Desc))
            {
                Stack.Children.Add(new Label
                {
                    FontSize = 15,
                    Margin = new Thickness(0, 0, 0, 10),
                    Text = item.Info.Desc,
                    TextColor = Color.FromHex("#333333")
                });
            }

            if (item.Info.Flavors != null)
            {
                Stack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 17,
                    Text = "FLAVORS",
                    TextColor = Color.FromHex("#e349c2")
                });

                FlexLayout FlavorsLayout = new FlexLayout
                {
                    AlignContent = FlexAlignContent.Start,
                    AlignItems = FlexAlignItems.Start,
                    Direction = FlexDirection.Row,
                    Margin = new Thickness(0, 0, 0, 10),
                    Wrap = FlexWrap.Wrap
                };

                foreach (KeyValuePair<string, string> entry in item.Info.Flavors)
                {
                    StackLayout TagStack = new StackLayout
                    {
                        Margin = new Thickness(0, 5, 0, 5),
                        Orientation = StackOrientation.Horizontal
                    };

                    TagStack.Children.Add(new Frame
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
                        Padding = 0,
                        HasShadow = false
                    });

                    TagStack.Children.Add(new BoxView
                    {
                        HeightRequest = 0,
                        WidthRequest = 5
                    });

                    FlavorsLayout.Children.Add(TagStack);
                }

                Stack.Children.Add(FlavorsLayout);
            }

            if (item.Info.Effects.Medical != null || item.Info.Effects.Negative != null || item.Info.Effects.Positive != null)
            {
                Stack.Children.Add(new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 17,
                    Text = "EFFECTS",
                    TextColor = Color.FromHex("#3269e6")
                });

                FlexLayout EffectsLayout = new FlexLayout
                {
                    AlignContent = FlexAlignContent.Start,
                    AlignItems = FlexAlignItems.Start,
                    Direction = FlexDirection.Row,
                    Margin = new Thickness(0, 0, 0, 15),
                    Wrap = FlexWrap.Wrap
                };

                if (item.Info.Effects.Positive != null)
                {
                    foreach (KeyValuePair<string, string> entry in item.Info.Effects.Positive)
                    {
                        StackLayout TagStack = new StackLayout
                        {
                            Margin = new Thickness(0, 5, 0, 5),
                            Orientation = StackOrientation.Horizontal
                        };

                        TagStack.Children.Add(new Frame
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
                            Padding = 0,
                            HasShadow = false
                        });

                        TagStack.Children.Add(new BoxView
                        {
                            HeightRequest = 0,
                            WidthRequest = 5
                        });

                        EffectsLayout.Children.Add(TagStack);
                    }
                }

                if (item.Info.Effects.Negative != null)
                {
                    foreach (KeyValuePair<string, string> entry in item.Info.Effects.Negative)
                    {
                        StackLayout TagStack = new StackLayout
                        {
                            Margin = new Thickness(0, 5, 0, 5),
                            Orientation = StackOrientation.Horizontal
                        };

                        TagStack.Children.Add(new Frame
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
                            Padding = 0,
                            HasShadow = false
                        });

                        TagStack.Children.Add(new BoxView
                        {
                            HeightRequest = 0,
                            WidthRequest = 5
                        });

                        EffectsLayout.Children.Add(TagStack);
                    }
                }

                if (item.Info.Effects.Medical != null)
                {
                    foreach (KeyValuePair<string, string> entry in item.Info.Effects.Medical)
                    {
                        if (entry.Value != "Headache")
                        {
                            StackLayout TagStack = new StackLayout
                            {
                                Margin = new Thickness(0, 5, 0, 5),
                                Orientation = StackOrientation.Horizontal
                            };

                            TagStack.Children.Add(new Frame
                            {
                                BackgroundColor = Color.Transparent,
                                BorderColor = Color.FromHex("#3269e6"),
                                Content = new Label
                                {
                                    FontSize = 15,
                                    Margin = new Thickness(15, 5),
                                    Text = "Treats " + entry.Value,
                                    TextColor = Color.FromHex("#3269e6")
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

                            EffectsLayout.Children.Add(TagStack);
                        }
                    }
                }

                Stack.Children.Add(EffectsLayout);
            }

            Stack.Children.Add(new BoxView
            {
                Color = Color.LightGray,
                HeightRequest = 0.5,
                Margin = new Thickness(0, 0, 0, 15)
            });

            Stack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 23,
                Text = "Ratings & Reviews",
                TextColor = Color.FromHex("#333333"),
            });

            StackLayout scoreStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0, 0, 5),
                Orientation = StackOrientation.Horizontal
            };

            sectionRating = new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 50,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.Center
            };

            scoreStack.Children.Add(sectionRating);

            StackLayout scoreFooterStack = new StackLayout
            {
                Margin = 10,
                Orientation = StackOrientation.Vertical,
                Spacing = 5,
                VerticalOptions = LayoutOptions.Center
            };

            scoreFooterStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 17,
                HorizontalOptions = LayoutOptions.Start,
                Text = "out of 5",
                TextColor = Color.Gray
            });

            sectionNumberOfRatings = new Label
            {
                FontSize = 13,
                Margin = new Thickness(0, 0, 0, 3),
                TextColor = Color.Gray,
            };

            scoreFooterStack.Children.Add(sectionNumberOfRatings);

            scoreStack.Children.Add(scoreFooterStack);

            Stack.Children.Add(scoreStack);

            Frame writeReview = new Frame
            {
                BorderColor = Color.LightGray,
                CornerRadius = 15,
                HasShadow = false,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = 0
            };

            StackLayout writeReviewStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            StackLayout headerReviewStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(15, 12, 15, 7),
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center
            };

            writeReviewRating = new SfRating
            {
                HorizontalOptions = LayoutOptions.Start,
                ItemCount = 5,
                ItemSize = 17,
                Precision = Precision.Half,
                ReadOnly = false,
                Value = 0,
                VerticalOptions = LayoutOptions.Center
            };

            writeReviewRating.RatingSettings.RatedFill = Color.FromHex("#ffa742");
            writeReviewRating.RatingSettings.RatedStroke = Color.Transparent;
            writeReviewRating.RatingSettings.UnRatedFill = Color.LightGray;
            writeReviewRating.RatingSettings.UnRatedStroke = Color.Transparent;

            headerReviewStack.Children.Add(writeReviewRating);

            postReviewButton = new Button
            {
                BackgroundColor = Color.FromHex("#00C88C"),
                CornerRadius = 10,
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                HeightRequest = 30,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Text = "Post Review",
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 110
            };

            postReviewButton.Clicked += PostReviewClickedAsync;

            headerReviewStack.Children.Add(postReviewButton);

            writeReviewStack.Children.Add(headerReviewStack);

            writeReviewStack.Children.Add(new BoxView
            {
                Color = Color.LightGray,
                HeightRequest = 1,
            });

            writeReviewEditor = new Editor
            {
                FontSize = 15,
                HeightRequest = 75,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(7, 0, 7, 7),
                Text = "Write a review. Help others learn more about this strain.",
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            writeReviewEditor.Focused += EditorFocused;
            writeReviewEditor.Unfocused += EditorUnfocused;

            writeReviewStack.Children.Add(writeReviewEditor);

            writeReview.Content = writeReviewStack;

            Stack.Children.Add(writeReview);



            






        }

        public async Task UpdateRatingAsync()
        {
            Reviews reviews = await FirebaseBackend.GetRatingAsync(item.Info.Id);

            //caching review for strain

            mainPageFrame.updateRating(reviews.AverageRating);

            headerRating.Value = reviews.AverageRating;
            headerNumberOfRatings.Text = "(" + reviews.NumberOfReviews + ")";

            string rating = "" + (reviews.AverageRating + 0.001);
            string ratingSubstring = rating.Substring(0, 3);
            sectionRating.Text = ratingSubstring;

            sectionNumberOfRatings.Text = "" + reviews.NumberOfReviews + " Ratings";

            /*foreach (KeyValuePair<string, Rating> entry in reviews.Ratings)
            {

            }*/
        }

        private async void PostReviewClickedAsync(object sender, EventArgs e)
        {
            if (writeReviewRating.Value > 0 && writeReviewEditor.TextColor != Color.Gray && !string.IsNullOrEmpty(writeReviewEditor.Text))
            {
                await FirebaseBackend.PostReviewAsync(item.Info.Id, writeReviewRating.Value, writeReviewEditor.Text);

                await Application.Current.MainPage.DisplayAlert("Rating submitted!", "Thank you for taking a moment to rate this strain. The Joyleaf community appreciates your support.", "OK");

                await UpdateRatingAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Incomplete rating", "Please provide a rating greater than zero and use the text space to write a brief review.", "OK");
            }
        }

        private void EditorFocused(object sender, FocusEventArgs e)
        {
            if (writeReviewEditor.Text.Equals("Write a review. Help others learn more about this strain."))
            {
                writeReviewEditor.Text = "";
                writeReviewEditor.TextColor = Color.FromHex("#333333");
            }
        }

        private void EditorUnfocused(object sender, FocusEventArgs e)
        {
            if (writeReviewEditor.Text.Equals(""))
            {
                writeReviewEditor.Text = "Write a review. Help others learn more about this strain.";
                writeReviewEditor.TextColor = Color.Gray;
            }
        }
    }
}
