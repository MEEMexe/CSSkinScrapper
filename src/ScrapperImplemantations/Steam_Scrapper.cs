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

        private static string weaponApi = "%20%7C%20";
        private static string conditionApi = "%20%28";
        private static string statTrakApi = "StatTrak%E2%84%A2%20";

        internal Steam_Scrapper()
        {
            for (int i = 0; i < weaponStrings.weapons.Length; i++)
            {
                CleanWhiteSpaces(ref weaponStrings.weapons[i]);
            }

            for (int i = 0; i < weaponStrings.conditions.Length; i++)
            {
                CleanWhiteSpaces(ref weaponStrings.weapons[i]);
            }
        }

        public override string GetUrl(Skin skin)
        {
            string apiSkin = string.Empty;

            if (skin.statTrak)
            {
                apiSkin += statTrakApi;
            }

            apiSkin += weaponStrings.weapons[(int)skin.type] + weaponApi;
            apiSkin += skin.name;
            apiSkin += conditionApi + weaponStrings.conditions[(int)skin.condition] + "%29";

            CleanWhiteSpaces(ref apiSkin);

            return apiSkin;
        }

        public override double[] GetPriceArray(List<Skin> skins)
        {
            int skinCount = skins.Count;
            double[] priceArray = new double[skinCount];

            for (int i = 0; i < skinCount; i++)
            {
                string form = " \tprice:\t";
                string skinname = skins[i].name;
                //TODO ON STORES-BRANCH: get api name out of Skin class in abstract method -> different implemantations for different store
                double price = GetPrice(GetUrl(skins[i]));

                if (skinname.Length < 9)
                    form += "\t";

                priceArray[i] = price;

                if (skins[i].statTrak)
                    skinname = skinname.Insert(0, "StatTrak ");

                Console.WriteLine(skinname + form + price);
            }

            return priceArray;
        }

        public override double GetPrice(string apiSkin)
        {
            HttpResponseMessage response = GetResponse(apiSkin);

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

        private static string CleanWhiteSpaces(ref string toClean)
        {
            while (toClean.Contains(" "))
            {
                toClean = toClean.Replace(" ", "%20");
            }

            return toClean;
        }
    }
}
