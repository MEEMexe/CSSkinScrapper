using System;

namespace CSSkinScrapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CompleteTask();

            //Wait for Excel Process to finalize
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        static void CompleteTask()
        {
            //Load user settings
            JSONInterop jsonInterop = new JSONInterop();
            SaveFile saveFile = jsonInterop.Load().GetAwaiter().GetResult();

            //get newest prices
            Console.WriteLine("Getting Prices:");
            string[,] prices = SkinScrapper.GetPriceArray(saveFile.skinNames, saveFile.skinApiNames);

            //Write to excel
            Console.WriteLine("\nWriting to Excelsheet...");
            new ExcelInterop(saveFile.filePath).WriteExcel(prices);

            //
        }

        public static void NewSkin(ref SaveFile saveFile)
        {
            
        }
    }
}