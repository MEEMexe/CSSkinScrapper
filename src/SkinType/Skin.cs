namespace CSSkinScrapper.SkinType
{
    public class Skin
    {
        public string name;
        public bool statTrak;
        public string type;
        public string condition;
        public double buyPrice;

        public Skin(string _name, string _type, bool _statTrak, double _buyprice, string _condition)
        {
            name = _name;
            type = _type;
            condition = _condition;
            buyPrice = _buyprice;
            statTrak = _statTrak;
        }

        public new string ToString()
        {
            string complete = string.Empty;
            if (statTrak)
                complete += "StatTrak ";
            complete += type + " " + name + " " + condition;
            return complete;
        }
    }
}
