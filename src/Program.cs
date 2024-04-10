using CSSkinScrapper.FileInterop;
using CSSkinScrapper.ScrapperImplemantation;
using CSSkinScrapper.SkinType;

namespace CSSkinScrapper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var scrapper = new SkinScrapper();

            var list = new List<Skin>();
            list.Add(new Skin() { type = "AK-47", condition = "Factory New", name = "Nightwish", statTrak = true });
            list.Add(new Skin() { type = "Desert Eagle", condition = "Factory New", name = "Printstream", statTrak = true });

            var s = scrapper.StartScrapping(list);
            Console.ReadKey();
        }
    }
}
