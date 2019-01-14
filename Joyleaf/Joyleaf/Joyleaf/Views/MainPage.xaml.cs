using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Firebase.Auth;
using Joyleaf.CustomTypes;
using System.Collections;
using Plugin.Connectivity;
using System.Threading.Tasks;

namespace Joyleaf
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
	{
        //firebase
        FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyDzp-mTwM_FacdwvNWk-6-M350NqDdXc94"));
        FirebaseAuth auth = FirebaseBackend.DeserializeAuth();
        FirebaseAuthLink authLink;
        //wheel
        ActivityIndicator Wheel;

        public MainPage(){
            //fetch account
            authLink = new FirebaseAuthLink(authProvider, auth);
            var account = FirebaseBackend.GetAccount(authLink);

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
            MasterBarText.Text = "Hello, " + account.firstName;
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

        public async void RefreshContent(){
            await Task.Run(() =>
            {
                //start test
                var list = Services.WebTester.GetImageURL("https://www.saq.com/webapp/wcs/stores/servlet/SearchDisplay?pageSize=20&searchTerm=*&catalogId=50000&showOnly=product&beginIndex=0&langId=-2&storeId=20002&categoryIdentifier=06&orderBy=1");
                foreach (var e in list)
                {
                    var samplePicker = new Hashtable();
                    samplePicker.Add(0, "1g - $5.99");
                    var item = new StoreItem("Fresh New Awesome Product", e, samplePicker);
                    Device.BeginInvokeOnMainThread(() => { ItemList.Children.Add(item); });
                }
                //end test



                Device.BeginInvokeOnMainThread(() => { DisableLoader(); });
            });
        }

        public void EnableLoader(){
            ItemList.Children.Add(Wheel);
        }

        public void DisableLoader(){
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