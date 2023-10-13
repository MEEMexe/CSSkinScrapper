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

        public static bool SkinExists(Skin skin, bool recursive = true)
        {
            //TODO: this is awful
            var api = instance.scrapperList[0].GetUrl(skin);
            try
            {
                instance.scrapperList[0].GetPrice(api);
                return true;
            }
            catch
            {
                if (recursive)
                {
                    var skinName = skin.name;

                    if (skinName.Contains(" "))
                    {
                        while (skinName.Contains(" "))
                        {
                            skinName = skinName.Replace(" ", "-");
                        }

                    }
                    else if (skinName.Contains("-"))
                    {
                        while (skinName.Contains("-"))
                        {
                            skinName = skinName.Replace("-", " ");
                        }
                    }

                    skin.name = skinName;

                    return SkinExists(skin, false);
                }
                else
                {
                    throw new Exception($"The Skin {skin.name} does not exist.");
                }
            }
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
