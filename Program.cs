using System;

namespace CSSkinScrapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Load user settings
            JSONInterop jsonInterop = new JSONInterop();
            SaveFile saveFile = jsonInterop.Load().GetAwaiter().GetResult();

            //new skins?
            Console.WriteLine("Do you want to add new skins [n] or just scan for prices [s]?");
            if (Console.ReadLine() == "n")
                UserInterface.NewSkin(ref saveFile, ref jsonInterop);    

            //get newest prices
            Console.WriteLine("\nGetting Prices:");
            string[,] prices = SkinScrapper.GetPriceArray(saveFile.skinNames, saveFile.skinApiNames);

            //Write to excel
            Console.WriteLine("\nWriting to Excelsheet...");
            new ExcelInterop(saveFile.filePath).WriteExcel(prices);

            //Wait for Excel Process to finalize
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}