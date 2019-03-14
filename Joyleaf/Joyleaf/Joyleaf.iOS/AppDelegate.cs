using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace Joyleaf.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
            statusBar.BackgroundColor = UIColor.FromRGB(255, 255, 255);

            UINavigationBar.Appearance.TintColor = UIColor.FromRGB(0, 200, 140);

            UITabBar.Appearance.TintColor = UIColor.FromRGB(51, 51, 51);

            return base.FinishedLaunching(app, options);
        }
    }
}