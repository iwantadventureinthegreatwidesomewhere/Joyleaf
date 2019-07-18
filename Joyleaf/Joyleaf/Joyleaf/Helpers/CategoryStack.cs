using Xamarin.Forms;

namespace Joyleaf.Helpers
{
    public class CategoryStack : StackLayout
    {
        public CategoryStack(Curated categoryData)
        {
            Orientation = StackOrientation.Vertical;
            Spacing = 15;

            Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 23,
                Margin = new Thickness(25, 0),
                Text = categoryData.Title,
                TextColor = Color.FromHex("#333333")
            });

            Children.Add(new Label
            {
                FontSize = 16,
                Margin = new Thickness(25, 0),
                Text = categoryData.Description,
                TextColor = Color.Gray
            });

           ScrollView scrollView = new ScrollView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                Orientation = ScrollOrientation.Horizontal
            };

            Children.Add(scrollView);

            StackLayout itemStack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(17, 0),
                Spacing = 17
            };

            scrollView.Content = itemStack;

            foreach(Item item in categoryData.Items)
            {
                itemStack.Children.Add(new CategoryItem(item));
            }







        }
    }
}
