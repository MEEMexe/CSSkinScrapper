using CSSkinScrapper.ScrapperImplemantations;
using System.Collections.Generic;
using System;

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
            return instance.scrapperList[0].GetPriceArray(skins);

            instance.GetPriceArrays(skins);
        }

        private double[] GetPriceArrays(List<Skin> skins)
        {
            foreach (var scrapper in scrapperList)
            {
                
            }

            return null;
        }
    }
}
