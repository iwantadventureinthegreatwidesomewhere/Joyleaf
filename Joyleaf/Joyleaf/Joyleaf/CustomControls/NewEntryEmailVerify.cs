using System.Text.RegularExpressions;

namespace Joyleaf.CustomControls
{
    public class NewEntryEmailVerify : NewEntry
    {
        Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        new public bool verifyEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            return emailRegex.IsMatch(email);
        }
    }
}

