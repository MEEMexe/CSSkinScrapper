using CSSkinScrapper.ScrapperImplemantation;
using CSSkinScrapper.SkinType;

namespace CSSkinScrapper.UserInterface
{
    public class NewSkinInterface : TechnicalInterface
    {
        public void NewSkin(IList<Skin> skinList)
        {
            skinList.Add(new Skin("Nightwish", "AK-47", false, 78.96, "Minimal Wear"));
            skinList.Add(new Skin("In Living Color", "M4A4", false, 16.57, "Minimal Wear"));
            skinList.Add(new Skin("Decimator", "M4A1-S", false, 22.08, "Field-Tested"));
            skinList.Add(new Skin("Fever Dream", "AWP", false, 5.47, "Minimal Wear"));
            skinList.Add(new Skin("Monster Mashup", "USP-S", false, 9.01, "Minimal Wear"));
            skinList.Add(new Skin("Neo-Noir", "Glock-18", false, 24.78, "Minimal Wear"));
            skinList.Add(new Skin("Disco Tech", "MAC-10", false, 6.64, "Minimal Wear"));
            skinList.Add(new Skin("Neon Ply", "MP7", false, 2.68, "Factory New"));
            skinList.Add(new Skin("Momentum", "UMP-45", false, 2.50, "Field-Tested"));
            skinList.Add(new Skin("Vogue", "Glock-18", false, 2.50, "Field-Tested"));
            //skinList.Add(NewSkinUI());
            //Console.WriteLine("Do you want to add another skin?");
            //if (BoolUiSelector())
            //    NewSkin(skinList);
            //Console.Clear();
        }
        
        private Skin NewSkinUI()
        {
            Console.Clear();

            Console.WriteLine("For wich weapon did you get a new skin?");
            string weapon = WeaponSelector();
            Console.Clear();

            Console.WriteLine("\nEnter the name of your skin:");
            string name = NameSelector(weapon);
            Console.Clear();

            Console.WriteLine($"In wich condition did you get the {name} ?");
            string condition = ConditionSelector();
            Console.Clear();

            Console.WriteLine($"Did you get the {condition} {name} in StatTrak?");
            bool statTrak = BoolUiSelector();
            Console.Clear();

            string msg = $"How much did the {condition} {name}";
            if (statTrak)
                msg += " in StatTrak";
            Console.WriteLine($"{msg} cost?");
            double price = BuyPriceInput();
            Console.Clear();

            var skin = new Skin(name, weapon, statTrak, price, condition);

            Console.WriteLine($"Is the '{skin.ToString()}' for {skin.buyPrice}€ correct?");
            if (!BoolUiSelector())
            {
                //TODO: maybe don't restart from scratch and ask what information is wrong
                Console.WriteLine("This will restart the SkinSelection for the current Skin from scratch.");
                Console.WriteLine("Are you sure you want to do this?");
                if (BoolUiSelector())
                    return NewSkinUI();
            }
            Console.Clear();
            return skin;
        }

        private double BuyPriceInput()
        {
            string? price = Console.ReadLine();
            bool success = double.TryParse(price, out double value);
            if (!success)
            {
                Console.WriteLine("Input a numerical value with [,] as comma.");
                return BuyPriceInput();
            }
            return value;
        }

        private string ConditionSelector()
        {
            int conditionCount = SkinStrings.conditions.Length;

            for (int i = 0; i < conditionCount; i++)
            {
                Console.WriteLine(i + " - " + SkinStrings.conditions[i]);
            }

            return SkinStrings.conditions[IntUiSelector(conditionCount - 1)];
        }

        private string NameSelector(string weapon)
        {
            string? skinName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(skinName))
            {
                Console.WriteLine("You have to input something.");
                return NameSelector(weapon);
            }

            Console.Clear();
            Console.WriteLine("Checking if the skin exists...");
            Console.WriteLine("This might take a while for the first skin...");

            int i = skinName.Length - 1;
            if (skinName[i] == ' ')
                skinName = skinName.Remove(i);

            //get random instance of this name
            bool success;
            {
                var toSearch = new Skin(skinName, weapon, false, 69, SkinStrings.conditions[3]);
                success = Steam_Scrapper.SkinExists(toSearch);
            }

            if (!success)
            {
                Console.Clear();
                Console.WriteLine($"This skin dosn't exist. Weapon selected: {weapon}. Try again:");
                return NameSelector(weapon);
            }

            return skinName;
        }

        private string WeaponSelector()
        {
            //could be simplified -> writing catergory name in 0 of each category array -> iterate through first array
            Console.WriteLine("\nFor wich weapon category:");
            Console.WriteLine("0 - Pistols");
            Console.WriteLine("1 - Shotguns");
            Console.WriteLine("2 - SMGs");
            Console.WriteLine("3 - Rifles");
            Console.WriteLine("4 - LMGs");
            Console.WriteLine("5 - Sniper Rifles");
            int category = IntUiSelector(5);
            Console.Clear();

            Console.WriteLine("For wich weapon did you get a new skin?");
            Console.WriteLine("\nFor wich weapon specific:");

            int weaponCount = 0;
            foreach (string weapon in SkinStrings.weapons[category])
            {
                Console.WriteLine(weaponCount + " - " + weapon);
                weaponCount++;
            }

            int choosenWeapon = IntUiSelector(weaponCount);

            return SkinStrings.weapons[category][choosenWeapon];
        }
    }
}
