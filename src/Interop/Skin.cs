using System;
using System.Text.Json.Serialization;

namespace CSSkinScrapper.Interop
{
    public class Skin
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
        public float currentPrice;  //TODO: currently not used -> remove if so

        public Skin(string name, Weapon type, bool statTrak, double buyPrice, Conditions condition)
        {
            this.statTrak = statTrak;
            this.buyPrice = buyPrice;
            this.name = name;
            this.condition = condition;
            this.type = type;
        }

        new public string ToString()
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
