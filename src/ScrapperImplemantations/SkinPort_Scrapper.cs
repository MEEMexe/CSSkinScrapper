using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CSSkinScrapper.Interop;

namespace CSSkinScrapper.ScrapperImplemantations
{
    internal class SkinPort_Scrapper : ScrapperBase
    {
        protected override string baseUrl => "https://api.skinport.com/v1/items?app_id=730&currency=EUR&tradable=0";

        //TODO: share this between this and SkinPortImpl. -> start one async method at start and check if its completed if result is needed
        protected string completeMarket
        {
            get
            {
                if (m_completeMarket == "")
                {
                    using (var client = new HttpClient())
                    {
                        var response = client.GetAsync("https://api.skinport.com/v1/items?app_id=730&currency=EUR&tradable=0").GetAwaiter().GetResult();
                        if (!response.IsSuccessStatusCode)
                        {
                            //TODO: dont throw up
                            throw new Exception("Couldn't get the market from skinport.");
                        }
                        m_completeMarket = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    }
                }
                return m_completeMarket;
            }
        }

        private string m_completeMarket = "";

        public override double GetPrice(string skin)
        {
            //get weapon search
            var marketIndexBracet = completeMarket.IndexOf(skin) - 2;
            var closingBracet = completeMarket.IndexOf("}", marketIndexBracet);
            var singleWeaponEntry = completeMarket.Substring(marketIndexBracet, closingBracet - marketIndexBracet);

            //get median price
            var index = singleWeaponEntry.IndexOf("median_price");
            var subs = singleWeaponEntry.Substring(index + 14, 5);

            //parse
            subs = subs.Replace('.', ',');
            return double.Parse(subs);
        }

        public override Task<double[]> GetPriceArray(List<Skin> skinList)
        {
            var priceList = new List<double>();
            foreach (var skin in skinList)
            {
                priceList.Add(GetPrice(GetUrl(skin)));
            }
            return null; priceList.ToArray();
        }

        public override string GetUrl(Skin skin)
        {
            //build search
            string statTrak = "";
            if (skin.statTrak)
                statTrak = "StatTrak™ ";
            var search = $"market_hash_name\":\"";
            search += $"{statTrak}{weaponStrings.weapons[(int)skin.type]} | {skin.name} ({weaponStrings.conditions[(int)skin.condition]})\"";
            return search;
        }
    }
}
