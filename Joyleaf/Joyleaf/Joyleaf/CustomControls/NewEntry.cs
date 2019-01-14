using System;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using ReturnType = Joyleaf.CustomTypes.ReturnType;

namespace Joyleaf.CustomControls
{
    public class NewEntry: Entry
    {
        public new event EventHandler Completed;

        public new static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(
            nameof(ReturnType),
            typeof(ReturnType),
            typeof(NewEntry),
            ReturnType.Default,
            BindingMode.OneWay
        );

        public new ReturnType ReturnType{
            
            get { return (ReturnType)GetValue(ReturnTypeProperty); }
            set { SetValue(ReturnTypeProperty, value); }
        }

        public void InvokeCompleted(){
            if (this.Completed != null)
                this.Completed.Invoke(this, null);
        }

        Regex textRegex = new Regex(@"^[ -~]+$");
        public bool verifyText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;
            return textRegex.IsMatch(text);
        }
    }
}