using Joyleaf.CustomControls;
using Xamarin.Forms;

namespace Joyleaf.Helpers
{
    public class ExploreBox : CustomFrame
    {
        public ExploreBox()
        {
            CornerRadius = 13;
            HeightRequest = 300;
            Margin = new Thickness(0, 0, 0, 5);

            StackLayout stack = new StackLayout();
            Content = stack;
        }
    }
}