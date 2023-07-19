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
            JSONInterop jsonInterop = new JSONInterop();
            SaveFile saveFile = jsonInterop.Load().GetAwaiter().GetResult();

            //new skins?
            if (saveFile.skinCount == 0)
            {
                UserInterface.NewSkin(ref saveFile, ref jsonInterop);
            }
            else
            {
                Console.WriteLine("Do you want to add new skins [n] or just scan for prices [s]?");
                if (Console.ReadLine() == "n")
                {
                    UserInterface.NewSkin(ref saveFile, ref jsonInterop);
                }
            }

            //get newest prices
            Console.WriteLine("\nGetting Prices:");
            string[,] prices = SkinScrapper.GetPriceArray(saveFile.skinNames, saveFile.skinApiNames);

            //Write to excel
            Console.WriteLine("\nWriting to Excelsheet...");
            ExcelInterop exelInterop = new ExcelInterop(saveFile.filePath);

            exelInterop.WriteForm(saveFile);
            exelInterop.WriteData(prices);
        }
    }
}