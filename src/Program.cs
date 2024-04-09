using CSSkinScrapper.FileInterop;

namespace CSSkinScrapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            using var e = new SpreadSheetWriter();
        }
    }
}
