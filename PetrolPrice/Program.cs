using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net.Http;

namespace PetrolPrice
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var trying = 0; trying < 1;) {
                string[,] opcje = new string[,] { { "pl", " - Poland" }, { "nl", " - Netherlands" }, { "gb", " - United Kingdom" }, { "de", " - Germany" }, { "end", " - Close the aplication" } };
                Console.WriteLine("Choose the country from which you want information:");
                for (var i = 0; i < opcje.GetLength(0); i++)
                {
                    Console.WriteLine("     " + opcje[i, 0] + opcje[i, 1]);
                }
                Console.WriteLine();
                Console.Write("You are choosing: ");
                string country = Console.ReadLine();
                var fails = 0;
                for (int i = 0; i < opcje.GetLength(0); i++)
                {
                    if(country == opcje[opcje.GetLength(0)-1, 0])
                    {
                        trying++;
                        break;
                    }
                    if (opcje[i, 0] == country)
                    {
                        GetHtmlAsync(country);
                        Console.ReadLine();
                    }
                    else
                    {
                        fails++;
                    }
                    if (fails >= opcje.GetLength(0))
                    {
                        Console.WriteLine();
                        Console.WriteLine("You entered the wrong country");
                        Console.WriteLine();
                    }
                }
            }
        }

        private static async void GetHtmlAsync(string country)
        {
            var Year = DateTime.Now.Year;
            var Month = DateTime.Now.Month;
            var Day = DateTime.Now.Day;

            var url = $"https://{country}.fuelo.net/prices/date/{Year}-{Month}-{Day}?lang=en";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var PetrolsHtml = htmlDocument.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("id", "")
                .Equals("pricesTable")).ToList();


            var PetrolPriceList = PetrolsHtml[0].Descendants("thead").ToList()[0].Descendants("tr").ToList()[1].Descendants("th").ToList();

            char[] separators = new char[] {' '};
            var PetrolTypes = PetrolsHtml[0].Descendants("thead").ToList()[0].Descendants("tr").ToList()[0].Descendants("th").ToList();


            Console.WriteLine("");
            Console.WriteLine($"Average fuels prices for {country}:");
            for (var i = 1; i > 0 && i<PetrolPriceList.Count; i++)
            {
                Console.WriteLine("     " + PetrolTypes[i].InnerText.TrimEnd() + ": " + PetrolPriceList[i].InnerText.Split(separators, StringSplitOptions.RemoveEmptyEntries)[1]);
            }
            Console.WriteLine();
        }
    }
}
