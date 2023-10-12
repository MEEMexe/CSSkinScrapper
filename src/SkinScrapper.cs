using System;
using System.Net.Http;
using System.Collections.Generic;

namespace CSSkinScrapper
{
    public class BadRequestException : Exception { }
    public class SkinNotFoundException : Exception
    {
        public SkinNotFoundException(string message) : base(message) { }
    }

    internal static class HttpClientExtension
    {
        public static HttpResponseMessage GetResponseMessage(this HttpClient client, string skinpath)
        {
            return client.GetAsync(client.BaseAddress + skinpath).GetAwaiter().GetResult();
        }
    }

    internal class SkinScrapper
    {
        private static HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("http://steamcommunity.com/market/priceoverview/?currency=3&appid=730&market_hash_name=")
        };

        public static double[] GetPriceArray(List<string> skinNames, List<string> skinApiNames)
        {
            int skinCount = skinNames.Count;
            double[] priceArray = new double[skinCount];

            for (int i = 0; i < skinCount; i++)
            {
                string form = " price:\t";
                string skinname = skinNames[i];
                double price = GetPrice(skinApiNames[i]);

                if (skinname.Length < 9)
                    form += "\t";

                priceArray[i] = price;

                Console.WriteLine(skinname + form + price);
            }

            return priceArray;
        }

        public static double GetPrice(string skinpath)
        {
            HttpResponseMessage response = client.GetResponseMessage(skinpath);

            if (!response.IsSuccessStatusCode)
            {
                throw new BadRequestException();
            }

            string responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            int i = responseString.IndexOf("lowest_price");
            string priceString = responseString.Substring(i + 15, 4);
            priceString = priceString.Replace("-", "0");

            double price = double.Parse(priceString) / 1.15 - 0.01;
            price = Math.Round(price, 2);

            return price;
        }
    }
}
