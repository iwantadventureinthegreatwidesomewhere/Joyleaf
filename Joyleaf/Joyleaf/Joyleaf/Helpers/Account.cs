namespace Joyleaf
{
    public class Account
    {
        public readonly string firstName;
        public readonly string lastName;
        public readonly string email;
        public readonly string location;

        public Account(string firstName, string lastName, string email, string location)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.location = location;
        }
    }
}
