using System;
using System.Threading.Tasks;
using Joyleaf.CustomControls;
using Joyleaf.Services;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class HighfivePage : GradientPage
    {
        private readonly ButtonWithBusyIndicator FindStrainsButton;

        public HighfivePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();

            StackLayout WelcomeStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 25)
            };

            WelcomeStack.Children.Add(new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 0, 0, 50),
                Source = "Hand"
            });

            WelcomeStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 27,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(0, 0, 0, 30),
                Text = "Welcome to Highfive",
                TextColor = Color.White
            });

            WelcomeStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 17,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 25),
                Text = "Highfive searches our 2,000 unique strains to instantly find strains best suited to your interests.",
                TextColor = Color.White,
            });

            WelcomeStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 17,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 50),
                Text = "The more you use Joyleaf, the more tailored the results!",
                TextColor = Color.White,
            });

            FindStrainsButton = new ButtonWithBusyIndicator
            {
                HeightRequest = 45,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(25, 0),
                Text = "FIND STRAINS NOW",
            };

            FindStrainsButton.Clicked += FindStrainsButtonClicked;

            WelcomeStack.Children.Add(FindStrainsButton);

            HighfiveStack.Children.Add(WelcomeStack);
        }

        private async void FindStrainsButtonClicked(object sender, EventArgs e)         {             FindStrainsButton.IsBusy = true;

            await Task.Delay(250);              if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    FirebaseBackend.Highfive();
                    FindStrainsButton.IsBusy = false;
                }
                catch (Exception)
                {
                    FindStrainsButton.IsBusy = false;
                    await DisplayAlert("Error", "Whoops, looks like there's a problem on our end. Please try again later.", "OK");
                }
            }
            else
            {
                FindStrainsButton.IsBusy = false;
                await DisplayAlert("Connection error", "Please check your network connection, then try again.", "OK");
            }         }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
