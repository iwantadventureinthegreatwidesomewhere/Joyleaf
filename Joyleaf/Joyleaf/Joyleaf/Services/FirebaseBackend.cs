using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Joyleaf.Helpers;
using Joyleaf.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_API_KEY));
                FirebaseAuthLink authLink = Task.Run(() => authProvider.SignInWithEmailAndPasswordAsync(email, password)).Result;

                SetAuth(authLink);
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.Contains("EMAIL_NOT_FOUND") || e.InnerException.Message.Contains("INVALID_EMAIL") || e.InnerException.Message.Contains("INVALID_PASSWORD"))
                {
                    Application.Current.MainPage.DisplayAlert("Incorrect password for " + email, "The password you entered is incorrect. Please try again.", "OK");
                }
                else
                {
                    throw e;
                }
            }
        }

        public static void SignUp(Account account, string email, string password)
        {
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_API_KEY));
            FirebaseAuthLink authLink = Task.Run(() => authProvider.CreateUserWithEmailAndPasswordAsync(email, password)).Result;

            FirebaseClient firebaseClient = new FirebaseClient(
                Constants.FIREBASE_DATABASE_URL,
                new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(authLink.FirebaseToken) }
            );

            Task.Run(() => firebaseClient.Child("users").Child(authLink.User.LocalId).PutAsync(account));

            SetAuth(authLink);
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        public static void SendPasswordReset(string email, bool checkEmail)
        {
            if (checkEmail)
            {
                if (!IsEmailAvailable(email))
                {
                    FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_API_KEY));

                    authProvider.SendPasswordResetEmailAsync(email);

                    Application.Current.MainPage.DisplayAlert("Password reset email sent", "Follow the directions in the email to reset your password.", "OK");
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Cannot find account", "That email does not have an existing Joyleaf account.", "OK");
                }
            }
            else
            {
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_API_KEY));

                authProvider.SendPasswordResetEmailAsync(email);

                Application.Current.MainPage.DisplayAlert("Password reset email sent", "Follow the directions in the email to reset your password.", "OK");
            }
        }

        public static Account GetAccount()
        {
            FirebaseClient firebase = new FirebaseClient(
                Constants.FIREBASE_DATABASE_URL,
                new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(GetAuth().FirebaseToken) }
            );

            return Task.Run(() => firebase.Child("users").Child(GetAuth().User.LocalId).OnceSingleAsync<Account>()).Result;
        }

        public static bool IsEmailAvailable(string email)
        {
            try
            {
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_API_KEY));

                string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string test = new string(Enumerable.Repeat(chars, 100).Select(s => s[new Random().Next(s.Length)]).ToArray());
                Console.WriteLine(test);
                FirebaseAuthLink authLink = Task.Run(() => authProvider.SignInWithEmailAndPasswordAsync(email, test)).Result;

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
                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_API_KEY));
                FirebaseAuthLink authLink = new FirebaseAuthLink(authProvider, GetAuth());

                await authProvider.GetUserAsync(authLink.FirebaseToken);

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
            FirebaseAuthProvider authProvider = new FirebaseAuthProvider(new FirebaseConfig(Constants.FIREBASE_API_KEY));
            FirebaseAuthLink authLink = new FirebaseAuthLink(authProvider, GetAuth());

            Task<FirebaseAuthLink> freshAuthLink = authLink.GetFreshAuthAsync();

            SetAuth(await freshAuthLink);
        }

        public static async Task<Content> LoadContentAsync()
        {
            if (IsContentExpired())
            {
                int unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

                HttpClient client = new HttpClient();

                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://us-central1-joyleaf-c142c.cloudfunctions.net/get_content?uid=" + GetAuth().User.LocalId),
                    Method = HttpMethod.Get
                };

                HttpResponseMessage response = await client.SendAsync(request);
                string json = await response.Content.ReadAsStringAsync();

                Settings.Content = json;
                Settings.LastContentUpdateTimestamp = unixTimestamp;
                return Content.FromJson(json);
            }

            return Content.FromJson(Settings.Content);
        }

        public static bool IsContentExpired()
        {
            int unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            return unixTimestamp - Settings.LastContentUpdateTimestamp >= 10800;
        }

        public static void ResetContentTimer()
        {
            Settings.LastContentUpdateTimestamp = -1;
        }

        public static async Task PostReviewAsync(long id, double rating, string review)
        {
            HttpClient client = new HttpClient();

            Dictionary<string, string> values = new Dictionary<string, string>
            {
                { "id", "" + id },
                { "rating", "" + rating },
                { "review", review }
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = await client.PostAsync("https://us-central1-joyleaf-c142c.cloudfunctions.net/post_review?uid=" + GetAuth().User.LocalId, content);
            await response.Content.ReadAsStringAsync();
        }

        public static async Task<Reviews> GetRatingAsync(long id)
        {
            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://us-central1-joyleaf-c142c.cloudfunctions.net/get_reviews?strain_id=" + id + "&uid=" + GetAuth().User.LocalId),
                Method = HttpMethod.Get
            };
            
            HttpResponseMessage response = await client.SendAsync(request);
            string json = await response.Content.ReadAsStringAsync();

            return Reviews.FromJson(json);
        }

        public static void CacheRating(long id, Reviews reviews)
        {
            Content content = Content.FromJson(Settings.Content);

            foreach (Curated curated in content.Curated)
            {
                foreach(Item item in curated.Items)
                {
                    if(item.Info.Id == id)
                    {
                        item.Reviews = reviews;
                    }
                }
            }

            foreach (Item item in content.Featured)
            {
                if (item.Info.Id == id)
                {
                    item.Reviews = reviews;
                }
            }

            Settings.Content = content.ToJson();
        }

        public static async Task<SearchResult> SearchAsync(string[] words)
        {
            HttpClient client = new HttpClient();

            StringContent content = new StringContent(JsonConvert.SerializeObject(new SearchRequest(words)));

            HttpResponseMessage response = await client.PostAsync("https://us-central1-joyleaf-c142c.cloudfunctions.net/search?uid=" + GetAuth().User.LocalId, content);
            string json = await response.Content.ReadAsStringAsync();

            return SearchResult.FromJson(json);
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
