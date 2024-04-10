using CSSkinScrapper.FileInterop;
using CSSkinScrapper.ScrapperImplemantation;
using CSSkinScrapper.SkinType;
using CSSkinScrapper.UserInterface;

namespace CSSkinScrapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var scrapper = new SkinScrapper();
            using var excelSheet = new SpreadSheetWriter();
            var skinList = excelSheet.Init();

            if (skinList.Count != 0)
            {
                if (UserInterfaceWaiter.AskNewSkin())
                {
                    new NewSkinInterface().NewSkin(skinList);
                }
            }
            else
            {
                new NewSkinInterface().NewSkin(skinList);
            }

            var s = scrapper.StartScrapping(skinList);
            excelSheet.WritePriceArrays(s);
        }
    }
}
