using System;
using System.Text.Json.Serialization;

namespace CSSkinScrapper
{
    public class Skin : IFormattable
    {
        [JsonInclude]
        public string name;
        [JsonInclude]
        public Weapon type;
        [JsonInclude]
        public bool statTrak;
        [JsonInclude]
        public double buyPrice;
        [JsonInclude]
        public Conditions condition;

        [JsonIgnore]
        public float currentPrice;

        public Skin(string name, Weapon type, bool statTrak, double buyPrice, Conditions condition)
        {
            this.statTrak = statTrak;
            this.buyPrice = buyPrice;
            this.name = name;
            this.condition = condition;
            this.type = type;
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            string target = string.Empty;
            if (statTrak)
                target += "StatTrak ";
            target += SkinStrings.defaultWeapons[(int)type];
            target += " " + name + " " + condition;

            return target;
        }
    }
}
