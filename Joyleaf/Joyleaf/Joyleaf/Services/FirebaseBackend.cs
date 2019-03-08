using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Joyleaf.Helpers;
using Joyleaf.Views;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Joyleaf.Services
{
    static class FirebaseBackend
    {
        public static void DeleteAuth()
        {
            Settings.FirebaseAuth = "";
        }

        public static Account GetAccount(FirebaseAuthLink auth)
        {
            FirebaseClient firebase = new FirebaseClient(
                Constants.FIREBASE_DATABASE_URL,
                new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken) }
            );

            Account account = Task.Run(() =>
            {
                Task<Account> t = firebase.Child("users").Child(auth.User.LocalId).OnceSingleAsync<Account>();
                return t;
            }).Result;

            return account;
        }

        public static FirebaseAuth GetAuth()
        {
            string json = Settings.FirebaseAuth;
            FirebaseAuth auth = JsonConvert.DeserializeObject<FirebaseAuth>(json);
            return auth;
        }

        public static bool IsEmailAvailable(string email)
        {
            try
            {
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_DATABASE_API_KEY));

                FirebaseAuthLink auth = Task.Run(() =>
                {
                    Task<FirebaseAuthLink> t = authProvider.SignInWithEmailAndPasswordAsync(email, "~");
                    return t;
                }).Result;

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

        public static async Task<bool> IsSavedAuthValidAsync()
        {
            try
            {
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_DATABASE_API_KEY));

                FirebaseAuthLink auth = new FirebaseAuthLink(authProvider, GetAuth());

                User user = await authProvider.GetUserAsync(auth.FirebaseToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task RefreshAuthAsync()
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_DATABASE_API_KEY));

            FirebaseAuthLink authLink = new FirebaseAuthLink(authProvider, GetAuth());

            Task<FirebaseAuthLink> freshAuthLink = authLink.GetFreshAuthAsync();

            SetAuth(await freshAuthLink);
        }

        public static void SendPasswordReset(string email)
        {
            if (!IsEmailAvailable(email))
            {
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_DATABASE_API_KEY));

                authProvider.SendPasswordResetEmailAsync(email);

                Application.Current.MainPage.DisplayAlert("Password reset email sent", "Follow the directions in the email to reset your password.", "OK");
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Cannot find account", "That email does not have an existing Joyleaf account.", "OK");
            }
        }

        public static void SetAuth(FirebaseAuth auth)
        {
            string s = JsonConvert.SerializeObject(auth);
            Settings.FirebaseAuth = s;
        }

        public static void SignIn(string email, string password)
        {
            try
            {
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_DATABASE_API_KEY));

                FirebaseAuthLink auth = Task.Run(() =>
                {
                    Task<FirebaseAuthLink> t = authProvider.SignInWithEmailAndPasswordAsync(email, password);
                    return t;
                }).Result;

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

            FirebaseAuthLink auth = Task.Run(() =>
            {
                Task<FirebaseAuthLink> t = authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
                return t;
            }).Result;

            FirebaseClient firebase = new FirebaseClient(
                Constants.FIREBASE_DATABASE_URL,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken)
                });

            firebase.Child("users").Child(auth.User.LocalId).PutAsync(account);

            SetAuth(auth);

            Application.Current.MainPage = new NavigationPage(new MainPageView());
        }
    }
}