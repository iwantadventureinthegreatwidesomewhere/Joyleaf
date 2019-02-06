using CoreGraphics;
using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using ReturnType = Joyleaf.CustomTypes.ReturnType;

[assembly: ExportRenderer (typeof(CustomEntry), typeof(CustomiOSEntry))]

namespace Joyleaf.iOS.CustomRenderers
{
    public class CustomiOSEntry : EntryRenderer
    {
        protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            UITextField entry = Control as UITextField;

            entry.Layer.CornerRadius = 23;

            entry.BackgroundColor = UIColor.FromRGB(244, 243, 250);

            entry.Layer.BorderColor = UIColor.FromRGB(244, 243, 250).CGColor;
            entry.Layer.BorderWidth = 1;

            entry.ClearButtonMode = UITextFieldViewMode.WhileEditing;

            entry.LeftView = new UIView(new CGRect(0, 0, 10, Control.Frame.Height));
            entry.LeftViewMode = UITextFieldViewMode.Always;

            entry.EnablesReturnKeyAutomatically = true;
            
            CustomEntry entryReturnKey = (CustomEntry) Element;

            if (Control != null)
            {
                if (entryReturnKey != null)
                {
                    SetReturnType(entryReturnKey);

                    entry.ShouldReturn += (UITextField tf) => 
                    {
                        entryReturnKey.InvokeCompleted();
                        return true;
                    };
                }
            }
        }
        
        private void SetReturnType(CustomEntry entryReturnKey)
        {
            ReturnType type = entryReturnKey.ReturnType;

            switch (type)
            {
                case ReturnType.Done:
                    Control.ReturnKeyType = UIReturnKeyType.Done;
                    break;
                case ReturnType.Go:
                    Control.ReturnKeyType = UIReturnKeyType.Go;
                    break;
                case ReturnType.Next:
                    Control.ReturnKeyType = UIReturnKeyType.Next;
                    break;
                case ReturnType.Send:
                    Control.ReturnKeyType = UIReturnKeyType.Send;
                    break;
                case ReturnType.Default:
                    Control.ReturnKeyType = UIReturnKeyType.Default;
                    break;
            } 
        }
    }
}