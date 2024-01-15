using System;

namespace CSSkinScrapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ScopeToTriggerGC();

            //Wait for Excel Process to finalize
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        static void ScopeToTriggerGC()
        {
            //Load user settings
            var jsonInterop = new JSONInterop();
            var saveFile = jsonInterop.Load();

            //new skins?
            bool newSkin = UserInterface.AskNewSkin(saveFile);

            //save savefile if new skins got added
            if (newSkin)
                jsonInterop.Save(saveFile);

            //get newest prices
            Console.WriteLine("Getting Prices:\n");
            double[] prices = SkinScrapper.GetPriceArray(saveFile.skinList);

            //Write to excel
            Console.WriteLine("\nWriting to Excelsheet...");
            var exelInterop = new ExcelInterop(saveFile);
            exelInterop.WritePrices(prices, newSkin);
        }
    }
}