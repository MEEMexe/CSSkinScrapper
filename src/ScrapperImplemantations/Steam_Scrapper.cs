using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSSkinScrapper.ScrapperImplemantations
{
    internal class Steam_Scrapper : ScrapperBase
    {
        protected override string baseUrl => "http://steamcommunity.com/market/priceoverview/?currency=3&appid=730&market_hash_name=";

        public override string GetUrl(Skin skin)
        {
            throw new NotImplementedException();
        }

        public override double[] GetPriceArray(List<Skin> skins)
        {
            int skinCount = skins.Count;
            double[] priceArray = new double[skinCount];

            for (int i = 0; i < skinCount; i++)
            {
                string form = " price:\t";
                string skinname = skins[i].name;
                //TODO ON STORES-BRANCH: get api name out of Skin class in abstract method -> different implemantations for different store
                double price = GetPrice(skins[i]);

                if (skinname.Length < 9)
                    form += "\t";

                priceArray[i] = price;

                Console.WriteLine(skinname + form + price);
            }

            return priceArray;
        }

        public override double GetPrice(Skin skin)
        {
            HttpResponseMessage response = GetResponse(skin.name);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
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
