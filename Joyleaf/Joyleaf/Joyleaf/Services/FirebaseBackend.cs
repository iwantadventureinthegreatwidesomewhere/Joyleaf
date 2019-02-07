using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Joyleaf
{
    static  class FirebaseBackend
    {
        static public void SignUp(Account account, string password)
        {
            try
            {
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Helpers.Constants.FIREBASE_DATABASE_API_KEY));

                FirebaseClient firebase = new FirebaseClient(Helpers.Constants.FIREBASE_DATABASE_URL);

                FirebaseAuthLink auth = Task.Run(() =>
                {
                    var t = authProvider.CreateUserWithEmailAndPasswordAsync(account.email, password);
                    return t;
                }).Result;
                var x = firebase.Child("users").Child(auth.User.LocalId).PutAsync(account);
                SaveAuth(auth);
                var main = new MainPage();
                main.EnableLoader();
                App.Current.MainPage = new NavigationPage(main);
            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Whoops, looks like there is a problem on our end. Please try again later.", "OK");
            }
        }

        static public void SignIn(string email, string password){
            try{
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyDzp-mTwM_FacdwvNWk-6-M350NqDdXc94"));
                var auth = Task.Run(() =>
                {
                    var t = authProvider.SignInWithEmailAndPasswordAsync(email, password);
                    return t;
                }).Result;
                SaveAuth(auth);
                var main = new MainPage();
                main.EnableLoader();
                App.Current.MainPage = new NavigationPage(main);
            }
            catch(Exception e){
                App.Current.MainPage.DisplayAlert("Incorrect password for " + email, "The password you entered is incorrect. Please try again.", "Try Again");
            }
        }

        static public bool IsValidAuth(){
            try{
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyDzp-mTwM_FacdwvNWk-6-M350NqDdXc94"));
                var user = Task.Run(() =>
                {
                    var auth = new FirebaseAuthLink(authProvider, FirebaseBackend.DeserializeAuth());
                    return authProvider.GetUserAsync(auth.FirebaseToken);
                }).Result;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        static public bool ForgotPassword(string email){
            try{
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyDzp-mTwM_FacdwvNWk-6-M350NqDdXc94"));
                var auth = Task.Run(() =>
                {
                    var t = authProvider.SignInWithEmailAndPasswordAsync(email, "~");
                    return t;
                }).Result;
                return false;
            }
            catch(Exception e){
                if (e.InnerException.Message.Contains("INVALID_PASSWORD"))
                {
                    FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyDzp-mTwM_FacdwvNWk-6-M350NqDdXc94"));
                    authProvider.SendPasswordResetEmailAsync(email);
                    App.Current.MainPage.DisplayAlert("Reset your password", "We've sent you a password reset link to your email address.", "OK");
                    return true;
                }else if(e.InnerException.Message.Contains("EMAIL_NOT_FOUND")){
                    App.Current.MainPage.DisplayAlert("Cannot find account", "That email does not have an existing Joyleaf account.", "OK");
                    return false;
                }else{
                    App.Current.MainPage.DisplayAlert("Error", "Whoops, looks like there is a problem on our end. Please try again later.", "OK");
                    return false;
                }
            }
        }

        static public bool EmailAvailable(string email){
            try
            {
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyDzp-mTwM_FacdwvNWk-6-M350NqDdXc94"));
                var auth = Task.Run(() =>
                {
                    var t = authProvider.SignInWithEmailAndPasswordAsync(email, "~");
                    return t;
                }).Result;
                return false;
            }
            catch (Exception e){
                if (e.InnerException.Message.Contains("EMAIL_NOT_FOUND")){
                    return true;
                }else if (e.InnerException.Message.Contains("INVALID_PASSWORD")){
                    App.Current.MainPage.DisplayAlert("Email is taken", "That email belongs to an existing account. Try another.", "OK");
                    return false;
                }else{
                    App.Current.MainPage.DisplayAlert("Error", "Whoops, looks like there is a problem on our end. Please try again later.", "OK");
                    return false;
                }
            }
        }

        static public Account GetAccount(FirebaseAuthLink auth){
            try{
                FirebaseClient firebase = new FirebaseClient("https://joyleaf-c142c.firebaseio.com/",
                                                            new FirebaseOptions
                                                            {
                                                                AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken)
                                                            });
                var a = Task.Run(() =>
                {
                    var t = firebase.Child("users").Child(auth.User.LocalId).OnceSingleAsync<Account>();
                    return t;
                }).Result;
                return a;
           }
            catch(Exception e){
                return null;
            }
        }

        static public void SaveAuth(FirebaseAuth auth){
            string s = JsonConvert.SerializeObject(auth);
            Settings.FirebaseAuth = s;
        }

        static public FirebaseAuth DeserializeAuth(){
            string json = Settings.FirebaseAuth;
            FirebaseAuth auth = JsonConvert.DeserializeObject<FirebaseAuth>(json);
            return auth;
        }

        static public void LogOut(){
            Settings.FirebaseAuth = "";
        }
    }
}
