using CSSkinScrapper.SkinType;

namespace CSSkinScrapper.ScrapperImplemantation
{
    public interface IScrapper
    {
        public Task<double[]> GetPriceArray(List<Skin> skins);
    }

    public class SkinScrapper
    {
        private readonly List<IScrapper> actualScrappers = new List<IScrapper>();

        public SkinScrapper()
        {
            actualScrappers.Add(new Steam_Scrapper());
        }

        public double[][] StartScrapping(List<Skin> skins)
        {
            return AsyncScrapping(skins).Result;
        }

        private async Task<double[][]> AsyncScrapping(List<Skin> skins)
        {
            var tasks = new List<Task<double[]>>();

            foreach (var scrapper in actualScrappers)
            {
                tasks.Add(scrapper.GetPriceArray(skins));
            }

            return await Task.WhenAll(tasks);
        }
    }
}
