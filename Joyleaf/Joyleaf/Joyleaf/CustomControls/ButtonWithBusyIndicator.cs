using System;
using Xamarin.Forms;

namespace Joyleaf.CustomControls
{
    public class ButtonWithBusyIndicator : RelativeLayout
    {
        public new event EventHandler Clicked;

        public ButtonWithBusyIndicator()
        {
            Button button = new Button {
                BackgroundColor = Color.White,
                CornerRadius = 23,
                FontAttributes = FontAttributes.Bold,
                FontFamily = (OnPlatform<string>)Application.Current.Resources["SF-Bold"],
                FontSize = 15,
                TextColor = Color.FromHex("#333333")
            };

            button.Clicked += InvokeClicked;

            Children.Add(button,
                Constraint.Constant(0), Constraint.Constant(0),
                Constraint.RelativeToParent((p) => { return p.Width; }),
                Constraint.RelativeToParent((p) => { return p.Height; }));


            Children.Add(new ActivityIndicator { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, HeightRequest = 30, WidthRequest = 30 },
                Constraint.RelativeToParent((p) => { return p.Width / 2 - 15; }),
                Constraint.RelativeToParent((p) => { return p.Height / 2 - 15; }));
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ButtonWithBusyIndicator), string.Empty, propertyChanged: OnTextChanged);
        public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(ButtonWithBusyIndicator), false, propertyChanged: OnIsBusyChanged);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        private void InvokeClicked(object sender, EventArgs e)
        {
            if (Clicked != null)
            {
                Clicked.Invoke(sender, e);
            }
        }

        private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ButtonWithBusyIndicator control = bindable as ButtonWithBusyIndicator;

            if (control == null)
            {
                return;
            }

            SetTextBasedOnBusy(control, control.IsBusy, newValue as string);
        }

        private static void OnIsBusyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ButtonWithBusyIndicator control = bindable as ButtonWithBusyIndicator;

            if (control == null)
            {
                return;
            }

            SetTextBasedOnBusy(control, (bool)newValue, control.Text);
        }

        private static void SetTextBasedOnBusy(ButtonWithBusyIndicator control, bool isBusy, string text)
        {
            ActivityIndicator activityIndicator = GetActivityIndicator(control);
            Button button = GetButton(control);

            if (activityIndicator == null || button == null)
            {
                return;
            }

            activityIndicator.IsVisible = activityIndicator.IsRunning = isBusy;
            button.Text = isBusy ? string.Empty : control.Text;
        }

        private static ActivityIndicator GetActivityIndicator(ButtonWithBusyIndicator control)
        {
            return control.Children[1] as ActivityIndicator;
        }

        private static Button GetButton(ButtonWithBusyIndicator control)
        {
            return control.Children[0] as Button;
        }
    }
}
