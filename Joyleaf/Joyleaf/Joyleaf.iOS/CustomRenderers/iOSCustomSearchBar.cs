using System;
using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(iOSCustomSearchBar))]

namespace Joyleaf.iOS.CustomRenderers
{
    public class iOSCustomSearchBar : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> args)
        {
            base.OnElementChanged(args);

            if (Control == null)
            {
                return;
            }

            UISearchBar searchBar = (UISearchBar)this.Control;

            searchBar.BackgroundImage = new UIImage();

            Foundation.NSString _searchField = new Foundation.NSString("searchField");
            var insideSearchBar = (UITextField)searchBar.ValueForKey(_searchField);

            insideSearchBar.BackgroundColor = UIColor.FromRGB(240, 240, 245);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Text")
            {
                Control.ShowsCancelButton = false;
            }
        }
    }
}
