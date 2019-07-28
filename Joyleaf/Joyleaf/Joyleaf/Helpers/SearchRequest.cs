using System;
namespace Joyleaf.Helpers
{
    public class SearchRequest
    {
        public string[] words;

        public SearchRequest(string[] words)
        {
            this.words = words;
        }
    }
}
