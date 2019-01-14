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

            //grid
            var grid = new Grid();
            ColumnDefinition c0 = new ColumnDefinition();
            c0.Width = new GridLength(45, GridUnitType.Star);
            grid.ColumnDefinitions.Add(c0);
            ColumnDefinition c1 = new ColumnDefinition();
            c1.Width = new GridLength(55, GridUnitType.Star);
            grid.ColumnDefinitions.Add(c1);

            //left layout
            var left = new StackLayout();
            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += (object sender, EventArgs e) => {
                NewPage(new ItemPage());
            };
            left.GestureRecognizers.Add(tap);
            var image = new Image();
            image.Source = imageURL;
            image.HorizontalOptions = LayoutOptions.CenterAndExpand;
            image.VerticalOptions = LayoutOptions.CenterAndExpand;
            left.Children.Add(image);
            grid.Children.Add(left, 0, 0);

            //right grid
            var rightGrid = new Grid();
            var r0 = new RowDefinition();
            r0.Height = new GridLength(30, GridUnitType.Star);
            rightGrid.RowDefinitions.Add(r0);
            var r1 = new RowDefinition();
            r1.Height = new GridLength(37, GridUnitType.Star);
            rightGrid.RowDefinitions.Add(r1);
            var r2 = new RowDefinition();
            r2.Height = new GridLength(33, GridUnitType.Star);
            rightGrid.RowDefinitions.Add(r2);
            grid.Children.Add(rightGrid, 1, 0);

            //title
            var titleGrid = new Grid();
            var t0 = new ColumnDefinition();
            t0.Width = new GridLength(5, GridUnitType.Star);
            titleGrid.ColumnDefinitions.Add(t0);
            var t1 = new ColumnDefinition();
            t1.Width = new GridLength(95, GridUnitType.Star);
            titleGrid.ColumnDefinitions.Add(t1);

            var title = new NewStoreTitle();
            title.Text = name;
            title.LineBreakMode = LineBreakMode.TailTruncation;
            title.FontSize = 13;
            title.FontAttributes = FontAttributes.Bold;
            title.TextColor = Color.FromHex("#303030");
            titleGrid.Children.Add(title, 1, 0);

            rightGrid.Children.Add(titleGrid, 0, 0);

            //picker
            var picker = new NewStorePicker();
            picker.TextColor = Color.FromHex("#00b1b0");
            picker.FontSize = 13;
            picker.FontAttributes = FontAttributes.Bold;

            for (int i = 0; i < selection.Count; i++)
            {
                picker.Items.Add(selection[i].ToString());
            }
            picker.SelectedIndex = 0;

            var pickerGrid = new Grid();
            pickerGrid.VerticalOptions = LayoutOptions.Start;
            pickerGrid.WidthRequest = 140;
            pickerGrid.HeightRequest = 30;
            var p0 = new ColumnDefinition();
            p0.Width = new GridLength(15, GridUnitType.Star);
            pickerGrid.ColumnDefinitions.Add(p0);
            var p1 = new ColumnDefinition();
            p1.Width = new GridLength(85, GridUnitType.Star);
            pickerGrid.ColumnDefinitions.Add(p1);
            TapGestureRecognizer tapPicker = new TapGestureRecognizer();
            tapPicker.Tapped += (object sender, EventArgs e) => {
                picker.Focus();
            };
            pickerGrid.GestureRecognizers.Add(tapPicker);

            var down = new Image { Source = "down" };
            down.HorizontalOptions = LayoutOptions.End;
            down.VerticalOptions = LayoutOptions.Center;
            pickerGrid.Children.Add(down, 0, 0);

            pickerGrid.Children.Add(picker, 1, 0);
            rightGrid.Children.Add(pickerGrid, 0, 1);

            //button
            var btnGrid = new Grid();
            var b0 = new ColumnDefinition();
            b0.Width = new GridLength(5, GridUnitType.Star);
            btnGrid.ColumnDefinitions.Add(b0);
            var b1 = new ColumnDefinition();
            b1.Width = new GridLength(95, GridUnitType.Star);
            btnGrid.ColumnDefinitions.Add(b1);

            var addToCart = new Button();
            addToCart.HorizontalOptions = LayoutOptions.Start;
            addToCart.Text = "ADD TO CART";
            addToCart.FontAttributes = FontAttributes.Bold;
            addToCart.FontSize = 10;
            addToCart.TextColor = Color.FromHex("#00b1b0");
            addToCart.WidthRequest = 100;
            addToCart.HeightRequest = 30;
            addToCart.CornerRadius = 18;
            addToCart.BorderColor = Color.FromHex("#00b1b0");
            addToCart.BorderWidth = 1.5;
            addToCart.Clicked += (object sender, EventArgs e) => {

            };

            btnGrid.Children.Add(addToCart, 1, 0);
            rightGrid.Children.Add(btnGrid, 0, 2);

            this.Content = grid;
        }

        async private void NewPage (Page page){
            await Navigation.PushAsync(page);
        }
    }
}
