using System.Text.Json.Serialization;

namespace CSSkinScrapper
{
    public enum Conditions
    {
        FactoryNew,
        MinimalWear,
        FieldTested,
        WellWorn,
        BattleScarred
    }

    public class Skin
    {
        [JsonInclude]
        public string name;
        [JsonInclude]
        public bool statTrak;
        [JsonInclude]
        public double buyPrice;
        [JsonInclude]
        public Conditions condition;

        [JsonIgnore]
        public float currentPrice;

        public Skin(string name, bool statTrak, double buyPrice, Conditions condition)
        {
            this.statTrak = statTrak;
            this.buyPrice = buyPrice;
            this.name = name;
            this.condition = condition;
        }

        /* Method for checking if Skin exists
        public static bool NameCheck(string name)
        {
            bool success = true;

            try
            {
                SkinScrapper.GetPrice(name);
            }
            catch (BadRequestException)
            {
                if (name.Contains(' '))
                {
                    name = name.Replace(' ', '-');
                }
                else
                {
                    name = name.Replace('-', ' ');
                }
                checkCount++;
                if (checkCount >= 2)
                {
                    success = false;
                }
                else
                {
                    NameCheck(name);
                }
            }

            if (!success)
            {
                throw new SkinNotFoundException($"The skin: {name} does not exist.");
            }

            return name;
        }
        */
    }
}
