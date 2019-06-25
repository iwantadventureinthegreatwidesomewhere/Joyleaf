using Xamarin.Forms;

namespace Joyleaf.Helpers
{
    public class ContentItem : Grid
    {
        public Item item;

        public ContentItem(Item item, bool islastToLoad)
        {
            this.item = item;

            HeightRequest = 70;
            Margin = new Thickness(0, 0, 0, 3);

            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(99, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            if (!islastToLoad)
            {
                Children.Add(new BoxView
                {
                    BackgroundColor = Color.LightGray,
                    HeightRequest = 1,
                    HorizontalOptions = LayoutOptions.Fill
                }, 1, 1);
            }

            StackLayout symbolStack = new StackLayout { HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };

            switch (item.Category)
            {
                case "Capsules":
                    symbolStack.Children.Add(new Image { Source = "Capsules" });
                    break;
                case "Dried flowers":
                    symbolStack.Children.Add(new Image { Source = "DriedFlowers" });
                    break;
                case "Oils":
                    symbolStack.Children.Add(new Image { Source = "Oils" });
                    break;
                case "Oral sprays":
                    symbolStack.Children.Add(new Image { Source = "OralSprays" });
                    break;
                case "Pre-rolled":
                    symbolStack.Children.Add(new Image { Source = "PreRolled" });
                    break;
            }

            Children.Add(symbolStack, 0, 0);

            StackLayout detailStack = new StackLayout { HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Center };

            detailStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                Text = item.Name
            });

            detailStack.Children.Add(new Label
            {
                FontSize = 13,
                Text = "By " + item.Brand,
                TextColor = Color.Gray
            });

            Children.Add(detailStack, 1, 0);








            StackLayout moreStack = new StackLayout();


        }
    }
}