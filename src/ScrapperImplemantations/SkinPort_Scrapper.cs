using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CSSkinScrapper.Interop;

namespace CSSkinScrapper.ScrapperImplemantations
{
    internal class SkinPort_Scrapper : ScrapperBase
    {
        protected override string baseUrl => "https://api.skinport.com/v1/items?app_id=730&currency=EUR&tradable=0";

        private string completeMarket;

        internal SkinPort_Scrapper()
        {
            HttpResponseMessage response = GetResponse("");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            completeMarket = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        public override double GetPrice(string skin)
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
