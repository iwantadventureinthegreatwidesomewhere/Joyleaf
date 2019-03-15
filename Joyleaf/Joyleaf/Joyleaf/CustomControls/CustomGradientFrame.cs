using Xamarin.Forms;

namespace Joyleaf.CustomControls
{
    public class CustomGradientFrame : Frame
    {
        public bool IsWhite
        {
            get;
            set;
        }

        public Color StartColor
        {
            get;
            set;
        }

        public Color EndColor
        {
            get;
            set;
        }
    }
}
