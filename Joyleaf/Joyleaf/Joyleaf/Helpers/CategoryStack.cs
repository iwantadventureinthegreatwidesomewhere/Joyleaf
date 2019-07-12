using Xamarin.Forms;

namespace Joyleaf.Helpers
{
    public class CategoryStack : StackLayout
    {
        public CategoryStack()
        {
            Orientation = StackOrientation.Vertical;
            Spacing = 15;

            Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 23,
                Margin = new Thickness(25, 0),
                Text = "Title",
                TextColor = Color.FromHex("#333333")
            });

            Children.Add(new Label
            {
                FontSize = 15,
                Margin = new Thickness(25, 0),
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.Sufaucus nibh et justo cursus id rutrum lorem imperdiet.",
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

            itemStack.Children.Add(new CategoryItem("sativa"));
            itemStack.Children.Add(new CategoryItem("indica"));
            itemStack.Children.Add(new CategoryItem("hybrid"));







        }
    }
}
