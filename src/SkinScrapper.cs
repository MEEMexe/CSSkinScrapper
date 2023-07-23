using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CSSkinScrapper
{
    internal class SkinScrapper
    {
        private static string baseUrl = "http://steamcommunity.com/market/priceoverview/?currency=3&appid=730&market_hash_name=";
        private static HttpClient client = new HttpClient();

        public static string[,] GetPriceArray(List<string> skinNames, List<string> skinApiNames)
        {
            int skinCount = skinNames.Count;
            string[,] priceArray = new string[skinCount, 1];

            for (int i = 0; i < skinCount; i++)
            {
                string form = " price:\t";
                string skinname = skinNames[i];
                string price = GetPrice(skinApiNames[i]).GetAwaiter().GetResult();

                if (skinname.Length < 9)
                    form += "\t";

                priceArray[i, 0] = price;

                Console.WriteLine(skinname + form + price);
            }

            return priceArray;
        }

        public static async Task<string> GetPrice(string skinpath)
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
            price = price.Replace("-", "0");

            return price;
        }
    }
}
