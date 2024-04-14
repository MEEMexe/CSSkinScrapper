using CSSkinScrapper.SkinType;

namespace CSSkinScrapper.ScrapperImplemantation
{
    internal class SkinPort_Scrapper : IScrapper
    {
        private string completeMarket = "";
        private Task marketTask;

        public SkinPort_Scrapper()
        {
            marketTask = GetCompleteMarket();
        }

        public async Task<double[]> GetPriceArray(List<Skin> skinList)
        {
            await marketTask;
            var prices = new List<double>();

            for (int i = 0; i < skinList.Count; i++)
            {
                prices.Add(GetPrice(skinList[i]));
            }

            return prices.ToArray();
        }

        private double GetPrice(Skin skin)
        {
            var skinString = GetUrl(skin);

            //get weapon search
            var marketIndexBracet = completeMarket.IndexOf(skinString) - 2;
            var closingBracet = completeMarket.IndexOf("}", marketIndexBracet);
            var singleWeaponEntry = completeMarket.Substring(marketIndexBracet, closingBracet - marketIndexBracet);

            //get median price
            var index = singleWeaponEntry.IndexOf("median_price");
            var subs = singleWeaponEntry.Substring(index + 14, 5);

            //parse
            subs = subs.Replace('.', ',');

            //console print form
            string form = "\t";
            if (!skin.statTrak)
                form += "\t";

            Console.WriteLine("SkinPort:\t" + skin.ToString() + form + subs);

            return double.Parse(subs);
        }

        private string GetUrl(Skin skin)
        {
            //build search
            string statTrak = "";
            if (skin.statTrak)
                statTrak = "StatTrak™ ";
            var search = $"market_hash_name\":\"";
            search += $"{statTrak}{skin.type} | {skin.name} ({skin.condition})\"";
            return search;
        }

        private async Task GetCompleteMarket()
        {
            var client = new HttpClient();
            var marketResponse = await client.GetAsync("https://api.skinport.com/v1/items?app_id=730&currency=EUR&tradable=0");

            if (!marketResponse.IsSuccessStatusCode)
            {
                //TODO: dont throw up
                throw new Exception("Couldn't get the market from skinport.");
            }
            completeMarket = await marketResponse.Content.ReadAsStringAsync();
        }
    }
}
