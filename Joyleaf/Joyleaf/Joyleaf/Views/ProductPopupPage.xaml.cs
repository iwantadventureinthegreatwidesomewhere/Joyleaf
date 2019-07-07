using System;
using System.Collections.Generic;
using Joyleaf.Helpers;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class ProductPopupPage : PopupPage
    {
        public ProductPopupPage(Item item)
        {
            InitializeComponent();

            ProductName.Text = item.Name;
        }
    }
}
