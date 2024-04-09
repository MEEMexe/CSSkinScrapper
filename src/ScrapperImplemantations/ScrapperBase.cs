using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSSkinScrapper.Interop;

namespace CSSkinScrapper.ScrapperImplemantations
{
    public abstract class ScrapperBase
    {
        protected SkinStrings weaponStrings { get; } = new SkinStrings();
        protected abstract string baseUrl { get; }

        private HttpClient client;

        protected ScrapperBase()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public abstract string GetUrl(Skin skin);
        public abstract double GetPrice(string apiSkin);
        public abstract Task<double[]> GetPriceArray(List<Skin> skinList);

        protected HttpResponseMessage GetResponse(string skinpath)
        {
            return client.GetAsync(skinpath).GetAwaiter().GetResult();
        }
    }
}
