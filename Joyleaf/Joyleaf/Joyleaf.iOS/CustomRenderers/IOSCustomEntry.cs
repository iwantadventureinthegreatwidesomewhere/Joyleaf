using CoreGraphics;
using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using ReturnType = Joyleaf.CustomTypes.ReturnType;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(IOSCustomEntry))]

namespace Joyleaf.iOS.CustomRenderers
{
    public class IOSCustomEntry : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }

            UITextField entry = Control as UITextField;

            entry.AutocorrectionType = UITextAutocorrectionType.No;
            entry.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 20);
            entry.BorderStyle = UITextBorderStyle.None;
            entry.ClearButtonMode = UITextFieldViewMode.WhileEditing;
            entry.Layer.CornerRadius = 23;
            entry.Layer.MasksToBounds = true;

            entry.LeftView = new UIView(new CGRect(0, 0, 16, Control.Frame.Height));
            entry.LeftViewMode = UITextFieldViewMode.Always;

            entry.EnablesReturnKeyAutomatically = true;

            CustomEntry entryReturnKey = (CustomEntry)Element;

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