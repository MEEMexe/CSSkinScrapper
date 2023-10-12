using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSSkinScrapper.ScrapperImplemantations
{
    internal class SkinPort_Scrapper : ScrapperBase
    {
        private static string baseURL = "https://api.skinport.com/v1/items?app_id=730&currency=EUR&tradable=0";

        private string completeMarket;

        internal SkinPort_Scrapper() : base(baseURL)
        {
            HttpResponseMessage response = GetResponse("");

            if (!response.IsSuccessStatusCode)
            {
                throw new BadRequestException();
            }

            completeMarket = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        public override double GetPrice(Skin skin)
        {
            throw new NotImplementedException();
        }

        public override double[] GetPriceArray(List<Skin> skinList)
        {
            throw new NotImplementedException();
        }

        public override string GetUrl(Skin skin)
        {
            throw new NotImplementedException();
        }
    }
}
