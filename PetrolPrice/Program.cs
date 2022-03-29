using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PetrolPrice
{
    class Program
    {
        static void Main(string[] args)
        {
            GetHtmlAsync();
            Console.ReadLine();
        }

        private static async void GetHtmlAsync()
        {
            var Year = DateTime.Now.Year;
            var Month = DateTime.Now.Month;
            var Day = DateTime.Now.Day;

            var url = $"https://pl.fuelo.net/prices/date/{Year}-{Month}-{Day}?lang=en";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var PetrolsHtml = htmlDocument.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("pricesTable")).ToList();


            var PetrolPriceList = PetrolsHtml[0].Descendants("thead").ToList()[0].Descendants("tr").ToList()[1].Descendants("th").ToList();

            char[] separators = new char[] {' '};
            string[] PetrolTypes = new String[] {"", "A95: ", "Diesel: ", "LPG: ", "CNG: ", "A98: ", "Diesel+: ", "A98+: " };

            for (var i = 1; i > 0 && i<8; i++)
            {
                Console.WriteLine(PetrolTypes[i] + PetrolPriceList[i].InnerText.Split(separators, StringSplitOptions.RemoveEmptyEntries)[1]);
            }
        }
    }
}
