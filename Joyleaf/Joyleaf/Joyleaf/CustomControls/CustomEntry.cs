using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

using ReturnType = Joyleaf.CustomTypes.ReturnType;

namespace Joyleaf.CustomControls
{
    public class CustomEntry: Entry
    {
        public new event EventHandler Completed;

        public new static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(
            nameof(ReturnType),
            typeof(ReturnType),
            typeof(CustomEntry),
            ReturnType.Default,
            BindingMode.OneWay
        );

        public new ReturnType ReturnType
        {
            get
            {
                return (ReturnType) GetValue(ReturnTypeProperty);
            }

            set
            { 
                SetValue(ReturnTypeProperty, value); 
            }
        }

        public void InvokeCompleted()
        {
            if(Completed != null)
            {
                Completed.Invoke(this, null);
            }
        }


        public bool VerifyText(string pattern)
        {
            Regex textRegex = new Regex(pattern);

            if(string.IsNullOrWhiteSpace(Text))
            {
                return false;
            }

            return textRegex.IsMatch(Text);
        }
    }
}