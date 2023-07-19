using Microsoft.Office.Interop.Excel;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CSSkinScrapper
{
    internal class Program
    {
        static string baseUrl =
            "http://steamcommunity.com/market/priceoverview/?currency=3&appid=730&market_hash_name=";

        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            int skinCount = Skins.skinCount;
            string[,] priceArray = new string[skinCount, 1];

            for (int i = 0; i < skinCount; i++)
            {
                string form = " price:\t";
                string skinname = Skins.skins[i, 0];
                string price = GetPrice(Skins.skins[i, 1]).GetAwaiter().GetResult();

                if (skinname.Length < 9)
                    form += "\t";

                priceArray[i, 0] = price;

                Console.WriteLine(skinname + form + price);
            }

            ExcelInterop.WriteExcel(priceArray);

            //Wait for Excel Process to finalize
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        static async Task<string> GetPrice(string skinpath)
        {
            string requestUrl = baseUrl + skinpath;

            HttpResponseMessage response = await client.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            string responseString = await response.Content.ReadAsStringAsync();

            int i = responseString.IndexOf("lowest_price");
            string price = responseString.Substring(i + 15, 4);

            return price;
        }
    }
}