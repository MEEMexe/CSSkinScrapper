using System.Net.Http;
using System;
using System.Collections.Generic;

namespace CSSkinScrapper.ScrapperImplemantations
{
    internal abstract class ScrapperBase
    {
        private HttpClient client;

        protected ScrapperBase(string baseUrl)
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public abstract string GetUrl(Skin skin);
        public abstract double GetPrice(Skin skin);
        public abstract double[] GetPriceArray(List<Skin> skinList);

        protected HttpResponseMessage GetResponse(string skinpath)
        {
            return client.GetAsync(client.BaseAddress + skinpath).GetAwaiter().GetResult();
        }
    }

    public class BadRequestException : Exception { }
    public class SkinNotFoundException : Exception
    {
        public SkinNotFoundException(string message) : base(message) { }
    }
}
