using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Joyleaf
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            { 
                return CrossSettings.Current; 
            }
        }

        public static string FirebaseAuth
        {
            get
            { 
                return AppSettings.GetValueOrDefault("FirebaseAuth", ""); 
            }

            set
            {
                AppSettings.AddOrUpdateValue("FirebaseAuth", value);
            }
        }
    }
}
