using System;
using System.Linq;
using System.IO;
using System.Net;
using Xamarin.Forms;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace Joyleaf.Services
{
    public class WebTester
    {
        static public List<string> GetImageURL(string url)
        {
            var htmlWeb = new HtmlWeb();

            var output = htmlWeb.Load(url);

            List<string> urls = new List<string>();


            var images = output.DocumentNode.SelectSingleNode(".//*[@id='resultatRecherche']").Descendants("img")                                     .Where(x => x.ParentNode.ParentNode.GetAttributeValue("class", "").Equals("img"));             foreach (var image in images)             {                 urls.Add("https:" + image.Attributes["src"].Value);             } 




            return urls;
        }
    }
}