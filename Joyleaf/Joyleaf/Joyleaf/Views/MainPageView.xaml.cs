using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Firebase.Auth;
using Joyleaf.CustomTypes;
using System;
using System.Collections;
using Plugin.Connectivity;
using System.Threading.Tasks;

namespace Joyleaf
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageView : MasterDetailPage
    {
        //firebase
        FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyDzp-mTwM_FacdwvNWk-6-M350NqDdXc94"));
        FirebaseAuth auth = FirebaseBackend.GetAuth();
        FirebaseAuthLink authLink;
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

            //initialize master bar text
            var MasterBarText = new Label();
            //MasterBarText.Text = "Hello, " + account.firstName;
            MasterBarText.TextColor = Color.White;
            MasterBarText.FontAttributes = FontAttributes.Bold;



            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            //subscribe to event that handles connection changes
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    ItemList.Children.Clear();
                    EnableLoader();
                    RefreshContent();
                }
                else
                {
                    ItemList.Children.Clear();
                    ItemList.Children.Add(ConnectionErrorText);
                }
            };

            ItemList.Spacing = 3;

            MasterBar.Children.Add(MasterBarText);

            //initialize gestures for opening and closing the master page
            var OpenMaster = new TapGestureRecognizer();
            OpenMaster.Tapped += (sender, e) =>
                this.IsPresented = true;
            var CloseMaster = new TapGestureRecognizer();
            CloseMaster.Tapped += (sender, e) =>
                this.IsPresented = false;

            Hamburger.GestureRecognizers.Add(OpenMaster);

            MasterGrey.GestureRecognizers.Add(CloseMaster);









        }

        public async void RefreshContent()
        {
            await Task.Run(() =>
            {



                Device.BeginInvokeOnMainThread(() => { DisableLoader(); });
            });
        }

        public void EnableLoader()
        {
            ItemList.Children.Add(Wheel);
        }

        public void DisableLoader()
        {
            ItemList.Children.Remove(Wheel);
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