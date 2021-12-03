using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TwilioRegs
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var countriesFile = File.ReadAllText("countries.json");
            var jCountries = JArray.Parse(countriesFile);
            var web = new WebClient { Encoding = Encoding.UTF8 };
            foreach (var jCountry in jCountries)
            {
                var value = jCountry["value"].ToString().ToLower(); // DZ
                Console.WriteLine(value);
                var url = string.Format("https://www.twilio.com/guidelines/{0}/regulatory", value);
                var html = web.DownloadString(url);
                var intStartMarker = html.IndexOf("section pt-5\">");
                var intEndMarker = html.IndexOf("<section class=\"section pt-0");
                if (intStartMarker == -1 || intEndMarker == -1) throw new Exception("Failed to find markers");
                html = html.Substring(intStartMarker+ 14, intEndMarker - intStartMarker - 14);
                File.WriteAllText(string.Format("{0}.html",value),html);
            }
        }
    }
}
