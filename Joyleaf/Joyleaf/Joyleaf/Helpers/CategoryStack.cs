using Xamarin.Forms;

namespace Joyleaf.Helpers
{
    public class CategoryStack : StackLayout
    {
        public CategoryStack(Curated categoryData)
        {
            Orientation = StackOrientation.Vertical;

            Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 23,
                Margin = new Thickness(25, 0, 25, 3),
                Text = categoryData.Title,
                TextColor = Color.FromHex("#333333")
            });

            Children.Add(new Label
            {
                FontSize = 17,
                Margin = new Thickness(25, 0, 25, 15),
                Text = categoryData.Description,
                TextColor = Color.Gray
            });

            ScrollView scrollView = new ScrollView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                Orientation = ScrollOrientation.Horizontal
            };

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
                itemStack.Children.Add(new CategoryItem(item, categoryData.Title));
            }

            Image ScrollMore = new Image
            {
                HeightRequest = 63,
                Source = "ScrollMore",
                WidthRequest = 32
            };

            RelativeLayout scrollLayout = new RelativeLayout
            {
                HeightRequest = 130
            };

            scrollLayout.Children.Add(scrollView,
                Constraint.RelativeToParent(parent => 0),
                Constraint.RelativeToParent(parent => 0),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.Constant(130));

            scrollLayout.Children.Add(ScrollMore,
                Constraint.RelativeToParent(parent => parent.Width - 32),
                Constraint.RelativeToParent(parent => parent.Height/2 - 32));

            Children.Add(scrollLayout);
        }
    }
}
