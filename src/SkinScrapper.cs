using CSSkinScrapper.ScrapperImplemantations;
using System.Collections.Generic;
using System;
using CSSkinScrapper.Interop;
using System.Net.Http;

namespace CSSkinScrapper
{
    internal class SkinScrapper
    {
        private static SkinScrapper instance { get; } = new SkinScrapper();

        private List<ScrapperBase> scrapperList = new List<ScrapperBase>();

        private SkinScrapper()
        {
            scrapperList.Add(new Steam_Scrapper());
        }

        public static double[] GetPriceArray(List<Skin> skins)
        {
            return instance.scrapperList[0].GetPriceArray(skins).Result;

            instance.GetPriceArrays(skins);
        }

        private static HttpClient? userClient;
        public static bool SkinExists(Skin skin)
        {
            if (userClient is null)
                userClient = new HttpClient();

            string req = instance.scrapperList[0].GetUrl(skin);
            var resp = userClient.GetAsync(req).Result;

            if (!resp.IsSuccessStatusCode)
                return false;
            else
                return true;
        }

        private (double[] steam, double[]skinport) GetPriceArrays(List<Skin> skins)
        {
            foreach (var scrapper in scrapperList)
            {
                
            }

            return (null, null);
        }
    }
}
