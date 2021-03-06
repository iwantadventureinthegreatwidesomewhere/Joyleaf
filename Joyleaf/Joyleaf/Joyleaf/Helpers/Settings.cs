﻿using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Joyleaf.Helpers
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

        public static string Content
        {
            get
            {
                return AppSettings.GetValueOrDefault("Content", "");
            }

            set
            {
                AppSettings.AddOrUpdateValue("Content", value);
            }
        }

        public static string Log
        {
            get
            {
                return AppSettings.GetValueOrDefault("Log", "");
            }

            set
            {
                AppSettings.AddOrUpdateValue("Log", value);
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

        public static int LastContentUpdateTimestamp
        {
            get
            {
                return AppSettings.GetValueOrDefault("LastContentUpdateTimestamp", -1);
            }

            set
            {
                AppSettings.AddOrUpdateValue("LastContentUpdateTimestamp", value);
            }
        }

        public static bool HasCompletedOnboarding
        {
            get
            {
                return AppSettings.GetValueOrDefault("HasCompletedOnboarding", true);
            }

            set
            {
                AppSettings.AddOrUpdateValue("HasCompletedOnboarding", value);
            }
        }

        public static void ResetSettings()
        {
            Content = "";
            FirebaseAuth = "";
            LastContentUpdateTimestamp = -1;
            Log = "";
        }
    }
}
