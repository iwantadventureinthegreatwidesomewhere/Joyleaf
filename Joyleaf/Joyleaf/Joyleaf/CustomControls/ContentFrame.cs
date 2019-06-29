using Joyleaf.Helpers;
using Xamarin.Forms;

namespace Joyleaf.CustomControls
{
    public class ContentFrame : Frame
    {
        public ContentFrame(Datum datum)
        {
            CornerRadius = 13;
            Margin = new Thickness(0, 0, 0, 5);

            StackLayout itemStack = new StackLayout();
            itemStack.Children.Add(new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 23,
                Margin = new Thickness(5, 0, 5, 10),
                Text = datum.Title,
                TextColor = Color.Black
            });

            if (!datum.Description.Equals(""))
            {
                itemStack.Children.Add(new Label
                {
                    FontSize = 15,
                    Margin = new Thickness(5, 0, 5, 10),
                    Text = datum.Description,
                    TextColor = Color.Gray
                });
            }

            for (int i = 0; i < datum.Items.Length; i++)
            {
                if (i != datum.Items.Length - 1)
                {
                    itemStack.Children.Add(new ContentItem(datum.Items[i], false));
                }
                else
                {
                    itemStack.Children.Add(new ContentItem(datum.Items[i], true));
                }
            }

            HeightRequest = itemStack.Height;
            Content = itemStack;
        }
    }
}