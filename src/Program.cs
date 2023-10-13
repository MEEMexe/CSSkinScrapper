using System;
using System.Collections.Generic;

namespace CSSkinScrapper
{
    internal class Program
    {
        static void TestNewScrapper()
        {
            var l = new List<Skin>
            {
                new Skin("In Living Color", Weapon.M4A4, false, 69, Conditions.MinimalWear),
                new Skin("In Living Color", Weapon.M4A4, true, 69, Conditions.MinimalWear),
            };

            SkinScrapper.GetPriceArray(l);
        }

        static void Main(string[] args)
        {
            TestNewScrapper();
            return;

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
                UserInterface.NewSkin(saveFile, jsonInterop);
                newSkin = true;
            }
            else
            {
                Console.WriteLine("Do you want to add new skins [n] or just scan for prices [s]?");
                if (Console.ReadLine() == "n")
                {
                    UserInterface.NewSkin(saveFile, jsonInterop);
                    newSkin = true;
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
        }
    }
}