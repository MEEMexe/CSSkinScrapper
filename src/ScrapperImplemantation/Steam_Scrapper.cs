using CSSkinScrapper.SkinType;
using System.Net;

namespace CSSkinScrapper.ScrapperImplemantation
{
    internal class Steam_Scrapper : IScrapper
    {
        private static Steam_Scrapper instance;
        private static readonly string baseUrl = "http://steamcommunity.com/market/priceoverview/?currency=3&appid=730&market_hash_name=";
        private static readonly string weaponApi = "%20%7C%20";
        private static readonly string conditionApi = "%20%28";
        private static readonly string statTrakApi = "StatTrak%E2%84%A2%20";

        private HttpClient webClient = new();

        public static bool SkinExists(Skin toTest)
        {
            var skinUrl = instance.GetUrl(toTest);
            HttpResponseMessage response = instance.webClient.GetAsync(skinUrl).Result;
            return response.IsSuccessStatusCode;
        }

        public Steam_Scrapper() { instance = this; }

        public async Task<double[]> GetPriceArray(List<Skin> skins)
        {
            var tasks = new List<Task<double>>();

            for (int i = 0; i < skins.Count; i++)
            {
                tasks.Add(GetPrice(skins[i]));
            }

            return await Task.WhenAll(tasks);
        }

        private async Task<double> GetPrice(Skin skin)
        {
            var skinUrl = GetUrl(skin);

            HttpResponseMessage response = await webClient.GetAsync(skinUrl);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    Console.WriteLine("To many steam requests. Waiting for 2 seconds.");
                    await Task.Delay(2 * 1000);
                    return await GetPrice(skin);
                }
                else
                {
                    //TODO: don't throw up
                    throw new Exception($"Steam-API-Call failed. Skin-URL: {skinUrl}");
                }
            }

            string responseString = await response.Content.ReadAsStringAsync();
            
            int i = responseString.IndexOf("median_price");     //sometimes a weapon dosn't have a lowest/highest property on steam for whatever reason
            if (i < 0)                                          //apperently weapons now sometimes only have a lowest price 
                i = responseString.IndexOf("lowest_price");

            if (i < 0)
                throw new Exception("Failed to parse out price.");

            string priceString = responseString.Substring(i + 15, 4);
            priceString = priceString.Replace("-", "0");

            double price = double.Parse(priceString) / 1.15 - 0.01; //market fee subtraction
            price = Math.Round(price, 2);

            //console print form
            string form = "\t";
            if (!skin.statTrak)
                form += "\t";

            Console.WriteLine("Steam:\t\t" + skin.ToString() + form + price);

            return price;
        }

        private string GetUrl(Skin skin)
        {
            string apiSkin = baseUrl;

            if (skin.statTrak)
            {
                apiSkin += statTrakApi;
            }

            apiSkin += skin.type + weaponApi;
            apiSkin += skin.name;
            apiSkin += conditionApi + skin.condition + "%29";

            CleanWhiteSpaces(ref apiSkin);

            return apiSkin;
        }

        private static string CleanWhiteSpaces(ref string toClean)
        {
            while (toClean.Contains(" "))
            {
                toClean = toClean.Replace(" ", "%20");
            }

            return toClean;
        }
    }
}