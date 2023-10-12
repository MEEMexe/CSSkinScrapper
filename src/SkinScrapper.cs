using CSSkinScrapper.ScrapperImplemantations;
using System.Collections.Generic;

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
            //TODO: foreach in scrapperList when ExcelInterop is implemented
            return instance.scrapperList[0].GetPriceArray(skins);
        }
    }
}
