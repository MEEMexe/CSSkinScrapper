﻿using System.Net.Http;
using System;
using System.Collections.Generic;

namespace CSSkinScrapper.ScrapperImplemantations
{
    public abstract class ScrapperBase
    {
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
        public abstract double GetPrice(Skin skin);
        public abstract double[] GetPriceArray(List<Skin> skinList);

        protected HttpResponseMessage GetResponse(string skinpath)
        {
            return client.GetAsync(client.BaseAddress + skinpath).GetAwaiter().GetResult();
        }
    }
}
