using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSSkinScrapper.UI
{
    internal class TechnicalUserInterface
    {
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

        protected int IntUiSelector(int max)
        {
            string? input = Console.ReadLine();
            bool success = int.TryParse(input, out int value);

            if (!success | value < 0 | value > max)
            {
                Console.WriteLine("Enter one of the numbers above:");
                return IntUiSelector(max);
            }

            return value;
        }

        protected bool BoolUiSelector()
        {
            Console.WriteLine("Type [y] for yes or [n] for no.");
            string? input = Console.ReadLine();

            if (input != "y" && input != "n")
            {
                return BoolUiSelector();
            }
            else
            {
                if (input == "y")
                    return true;
                else
                    return false;
            }
        }
    }
}
