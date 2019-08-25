using System;

namespace Joyleaf.Helpers
{
    public class Account
    {
        public readonly string name;
        public readonly string dateCreated;

        public Account(string name, string dateCreated)
        {
            this.name = name;
            this.dateCreated = dateCreated;
        }
    }
}
