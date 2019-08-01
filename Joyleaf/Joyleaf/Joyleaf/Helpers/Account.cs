using System;

namespace Joyleaf.Helpers
{
    public class Account
    {
        public readonly string name;
        public readonly string dateCreated;

        public Account(string name)
        {
            this.name = name;
            dateCreated = DateTime.Now.ToString("MM/dd/yyyy");
        }
    }
}
