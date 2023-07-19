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
            //SaveFile saveFile = JSONInterop.Load();

            //get newest prices
            Console.WriteLine("Getting Prices:");
            string[] prices = SkinScrapper.GetPriceArray(Skins.skins);

            //Write to excel
            Console.WriteLine("\nWriting to Excelsheet...");
            new ExcelInterop().WriteExcel(prices);
        }
    }
}