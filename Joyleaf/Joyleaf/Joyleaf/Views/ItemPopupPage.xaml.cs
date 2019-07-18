using System;
using System.Collections.Generic;
using Joyleaf.Helpers;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Joyleaf.Views
{
    public partial class ItemPopupPage : PopupPage
    {
        public ItemPopupPage(Item item)
        {
            InitializeComponent();

            ProductName.Text = item.Name;
        }
    }
}
