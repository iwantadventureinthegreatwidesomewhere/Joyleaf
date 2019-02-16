using Firebase.Auth;
using Joyleaf.Helpers;
using Joyleaf.Services;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Joyleaf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MainPageView : ContentPage
    {
        private FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_DATABASE_API_KEY));

        private FirebaseAuth auth = FirebaseBackend.GetAuth();

        private FirebaseAuthLink authLink;

        //wheel
        ActivityIndicator Wheel;

        public MainPageView()
        {
            //fetch account
            Account account;
            authLink = new FirebaseAuthLink(authProvider, auth);
            try
            {
                account = FirebaseBackend.GetAccount(authLink);
            }
            catch (Exception)
            {
                account = null;
            }


            //initialize wheel
            Wheel = new ActivityIndicator();
            Wheel.Color = Color.FromHex("#00b1b0");
            Wheel.HorizontalOptions = LayoutOptions.Center;
            Wheel.IsRunning = true;
            Wheel.Margin = new Thickness(0, 30);

            //initialize connection error text
            var ConnectionErrorText = new Label();
            ConnectionErrorText.FontSize = 15;
            ConnectionErrorText.Text = "Please check your network connection.";
            ConnectionErrorText.TextColor = Color.Gray;
            ConnectionErrorText.HorizontalOptions = LayoutOptions.CenterAndExpand;
            ConnectionErrorText.Margin = new Thickness(0, 70);




            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            //subscribe to event that handles connection changes
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    ContentList.Children.Clear();
                    EnableLoader();
                    RefreshContent();
                }
                else
                {
                    ContentList.Children.Clear();
                    ContentList.Children.Add(ConnectionErrorText);
                }
            };

            ContentList.Spacing = 3;










        }

        public async void RefreshContent()
        {
            ContentList.Children.Add(new StoreItem());
            ContentList.Children.Add(new StoreItem());
            ContentList.Children.Add(new StoreItem());
            ContentList.Children.Add(new StoreItem());
            await Task.Run(() =>
            {



                Device.BeginInvokeOnMainThread(() => { DisableLoader(); });
            });
        }

        public void EnableLoader()
        {
            ContentList.Children.Add(Wheel);
        }

        public void DisableLoader()
        {
            ContentList.Children.Remove(Wheel);
        }

        protected override void OnAppearing()
        {
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    RefreshContent();
                }
            }
        }
    }
}