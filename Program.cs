using System;

namespace CSSkinScrapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SkinScrapper.GetPriceArray(Skins.skins);

            //Wait for Excel Process to finalize
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}