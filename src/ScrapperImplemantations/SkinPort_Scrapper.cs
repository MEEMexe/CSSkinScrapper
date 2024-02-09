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

        //TODO: share this between this and SkinPortImpl. -> start one async method at start and check if its completed if result is needed
        protected string completeMarket
        {
            get
            {
                if (m_completeMarket == "")
                {
                    using (var client = new HttpClient())
                    {
                        var response = client.GetAsync("https://api.skinport.com/v1/items?app_id=730&currency=EUR&tradable=0").GetAwaiter().GetResult();
                        if (!response.IsSuccessStatusCode)
                        {
                            //TODO: dont throw up
                            throw new Exception("Couldn't get the market from skinport.");
                        }
                        m_completeMarket = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    }
                }
                return m_completeMarket;
            }
        }

        private string m_completeMarket = "";

        internal SkinPort_Scrapper()
        {
            HttpResponseMessage response = GetResponse("");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            m_completeMarket = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
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
