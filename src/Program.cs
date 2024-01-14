using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CSSkinScrapper.UI;

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
            JSONInterop jsonInterop = new();
            SaveFile saveFile = jsonInterop.Load().GetAwaiter().GetResult();

            //new skins?
            bool newSkin = UserInterface.AskNewSkin(saveFile);

            //save savefile if new skins got added
            if (newSkin)
                jsonInterop.Save(saveFile).GetAwaiter().GetResult();

            //get newest prices
            Console.WriteLine("\nGetting Prices:");
            double[] prices = SkinScrapper.GetPriceArray(saveFile.skinList);

            //Write to excel
            Console.WriteLine("\nWriting to Excelsheet...");
            ExcelInterop exelInterop = new ExcelInterop(saveFile);
            //TODO: just pass this bool into one excelinterop method
            if (newSkin)
                exelInterop.SkinForm(false);
            exelInterop.WritePrices(prices);
        }
    }
}