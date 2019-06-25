using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Joyleaf.Helpers;
using Joyleaf.Views;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Joyleaf.Services
{
    public static class FirebaseBackend
    {
        public static void SignIn(string email, string password)
        {
            try
            {
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_DATABASE_API_KEY));
                FirebaseAuthLink auth = Task.Run(() => authProvider.SignInWithEmailAndPasswordAsync(email, password)).Result;

                SetAuth(auth);
                Application.Current.MainPage = new NavigationPage(new MainPageView());
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.Contains("EMAIL_NOT_FOUND") || e.InnerException.Message.Contains("INVALID_EMAIL") || e.InnerException.Message.Contains("INVALID_PASSWORD"))
                {
                    Application.Current.MainPage.DisplayAlert("Incorrect password for " + email, "The password you entered is incorrect. Please try again.", "Try Again");
                }
                else
                {
                    throw e;
                }
            }
        }

        public static void SignUp(Account account, string email, string password)
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_DATABASE_API_KEY));
            FirebaseAuthLink auth = Task.Run(() => authProvider.CreateUserWithEmailAndPasswordAsync(email, password)).Result;

            FirebaseClient firebase = new FirebaseClient(
                Constants.FIREBASE_DATABASE_URL,
                new FirebaseOptions{ AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken) }
            );

            Task.Run(() => firebase.Child("users").Child(auth.User.LocalId).PutAsync(account));

            SetAuth(auth);
            Application.Current.MainPage = new NavigationPage(new MainPageView());
        }

        public static async Task<Account> GetAccountAsync()
        {
            FirebaseClient firebase = new FirebaseClient(
                Constants.FIREBASE_DATABASE_URL,
                new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(GetAuth().FirebaseToken) }
            );

            Account account = await firebase.Child("users").Child(GetAuth().User.LocalId).OnceSingleAsync<Account>();
            return account;
        }

        public static void SendPasswordReset(string email)
        {
            if (!IsEmailAvailable(email))
            {
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_DATABASE_API_KEY));
                authProvider.SendPasswordResetEmailAsync(email);

                Application.Current.MainPage.DisplayAlert("Password reset email sent", "Follow the directions in the email to reset your password.", "OK");
                Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Cannot find account", "That email does not have an existing Joyleaf account.", "OK");
            }
        }

        public static bool IsEmailAvailable(string email)
        {
            try
            {
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_DATABASE_API_KEY));
                FirebaseAuthLink auth = Task.Run(() => authProvider.SignInWithEmailAndPasswordAsync(email, "~")).Result;
                return false;
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.Contains("EMAIL_NOT_FOUND"))
                {
                    return true;
                }

                if (e.InnerException.Message.Contains("INVALID_PASSWORD") || e.InnerException.Message.Contains("INVALID_EMAIL"))
                {
                    return false;
                }

                throw e;
            }
        }

        public static async Task<bool> IsCurrentAuthValidAsync()
        {
            try
            {
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_DATABASE_API_KEY));
                FirebaseAuthLink auth = new FirebaseAuthLink(authProvider, GetAuth());
                await authProvider.GetUserAsync(auth.FirebaseToken);
                return true;
            }
            catch (Exception e)
            {
                if(e.Message.Contains("Value cannot be null."))
                {
                    return false;
                }

                throw e;
            }
        }

        public static async Task RefreshAuthAsync()
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_DATABASE_API_KEY));
            FirebaseAuthLink authLink = new FirebaseAuthLink(authProvider, GetAuth());
            Task<FirebaseAuthLink> freshAuthLink = authLink.GetFreshAuthAsync();
            SetAuth(await freshAuthLink);
        }

        public static async Task<Content> LoadContentAsync()
        {
            if (IsContentExpired())
            {
                int UnixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://us-central1-joyleaf-c142c.cloudfunctions.net/app_content_loader?uid=" + GetAuth().User.LocalId),
                    Method = HttpMethod.Get
                };

                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);
                string json = await response.Content.ReadAsStringAsync();

                Settings.Content = json;
                Settings.LastContentUpdateTimestamp = UnixTimestamp;
                return Content.FromJson(json);
            }

            return Content.FromJson(Settings.Content);
        }

        public static bool IsContentExpired()
        {
            int UnixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            return UnixTimestamp - Settings.LastContentUpdateTimestamp >= 43200;
        }

        public static void SetAuth(FirebaseAuth auth)
        {
            string s = JsonConvert.SerializeObject(auth);
            Settings.FirebaseAuth = s;
        }

        public static FirebaseAuth GetAuth()
        {
            string json = Settings.FirebaseAuth;
            FirebaseAuth auth = JsonConvert.DeserializeObject<FirebaseAuth>(json);
            return auth;
        }
    }
}