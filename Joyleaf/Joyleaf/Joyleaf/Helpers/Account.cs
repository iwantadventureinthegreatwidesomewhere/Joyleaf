using System;

namespace Joyleaf.Helpers
{
    public class Account
    {
        public readonly string name;
        public readonly string createdOn;

        public Account(string name)
        {
            this.name = name;
            createdOn = DateTime.Now.ToString("MM/dd/yyyy");
        }
    }
}
