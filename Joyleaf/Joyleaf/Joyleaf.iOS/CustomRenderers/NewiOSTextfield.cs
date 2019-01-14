using CoreGraphics;
using Joyleaf.CustomControls;
using Joyleaf.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ReturnType = Joyleaf.CustomTypes.ReturnType;

[assembly: ExportRenderer (typeof(NewEntry), typeof(NewiOSTextfield))]
namespace Joyleaf.iOS.CustomRenderers
{
    public class NewiOSTextfield : EntryRenderer
    {
        protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                return;
            }
            
            //adds clear button to UITextfields
            
            var entry = this.Control as UITextField;

            entry.ClearButtonMode = UITextFieldViewMode.WhileEditing;
            Control.LeftView = new UIView(new CGRect(0, 0, 10, Control.Frame.Height));
            Control.LeftViewMode = UITextFieldViewMode.Always;
            entry.EnablesReturnKeyAutomatically = true;
            
            //modifies return key for UITextfields
            
            NewEntry entryReturnKey = (NewEntry)this.Element;

            if (this.Control != null){
                if(entryReturnKey != null){
                    SetReturnType(entryReturnKey);
                    Control.ShouldReturn += (UITextField tf) => {
                        entryReturnKey.InvokeCompleted();
                        return true;
                    };
                }
            }
        }
        
        private void SetReturnType(NewEntry entryReturnKey){
            
            ReturnType type = entryReturnKey.ReturnType;

            switch (type){
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
