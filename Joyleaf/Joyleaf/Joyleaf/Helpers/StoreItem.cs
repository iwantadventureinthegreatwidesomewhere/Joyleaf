using System;
using Xamarin.Forms;
using Joyleaf.CustomControls;
using Joyleaf.Views;
using System.Collections;

namespace Joyleaf.CustomTypes
{
    public class StoreItem: Frame
    {

        public StoreItem(string name, string imageURL, Hashtable selection){
            //frame
            this.BorderColor = Color.FromHex("#ebebeb");
            this.HasShadow = false;
            this.CornerRadius = 15;
            this.HeightRequest = 120;
            this.Margin = new Thickness(5, 0);
        }

        async private void NewPage (Page page){
            await Navigation.PushAsync(page);
        }
    }
}
