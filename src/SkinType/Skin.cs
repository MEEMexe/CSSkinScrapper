namespace CSSkinScrapper.SkinType
{
    public class Skin
    {
        public string name;
        public bool statTrak;
        public string type;
        public string condition;
        public double buyPrice;

        public Skin(string completeSkin, double _buyprice)
        {
            int start = 0;
            bool _statTrak = false;
            if (completeSkin.Contains("StatTrak"))
            {
                _statTrak = true;
                start = completeSkin.IndexOf("  ") + 2;
            }

            int next = completeSkin.IndexOf("  ", start);
            string _type = completeSkin.Substring(start, next - start);

            next += 2;
            int next2 = completeSkin.IndexOf("  ", next);
            string _name = completeSkin.Substring(next, next2 - next);

            string _condition = completeSkin.Substring(next2 + 2);

            name = _name;
            type = _type;
            condition = _condition;
            buyPrice = _buyprice;
            statTrak = _statTrak;
        }

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
                complete += "StatTrak  ";
            complete += type + "  " + name + "  " + condition;
            return complete;
        }
    }
}
