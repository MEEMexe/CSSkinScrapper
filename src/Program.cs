using System;
using System.Collections.Generic;

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
            bool newSkin = false;

            //Load user settings
            JSONInterop jsonInterop = new JSONInterop();
            SaveFile saveFile = jsonInterop.Load().GetAwaiter().GetResult();

            //new skins?
            if (saveFile.skinCount == 0)
            {
                NewSkin();
            }
            else
            {
                Console.WriteLine("Do you want to add new skins [n] or just scan for prices [s]?");
                if (Console.ReadLine() == "n")
                {
                    NewSkin();
                }
            }

            //get newest prices
            Console.WriteLine("\nGetting Prices:");
            double[] prices = SkinScrapper.GetPriceArray(saveFile.skinList);

            //Write to excel
            Console.WriteLine("\nWriting to Excelsheet...");
            ExcelInterop exelInterop = new ExcelInterop(saveFile);
            if (newSkin)
                exelInterop.SkinForm(false);
            exelInterop.WritePrices(prices);

            void NewSkin()
            {
                var ui = new UserInterface();
                ui.NewSkin(saveFile.skinList);
                newSkin = true;
                Console.WriteLine("\nSaving json file.");
                jsonInterop.Save(saveFile).GetAwaiter().GetResult();
            }
        }
    }
}