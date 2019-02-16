namespace Joyleaf.Helpers
{
    public class Account
    {
        public readonly string firstName;
        public readonly string lastName;
        public readonly string location;

        public Account(string firstName, string lastName, string location)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.location = location;
        }
    }
}
