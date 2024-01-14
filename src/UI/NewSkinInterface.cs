using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CSSkinScrapper.UI
{
    internal class NewSkinInterface : TechnicalUserInterface
    {
        public void NewSkin(IList<Skin> skinList)
        {
            skinList.Add(NewSkinUI());
            Console.WriteLine("Do you want to add another skin?");
            if (BoolUiSelector())
                NewSkin(skinList);
        }

        private Skin NewSkinUI()
        {
            Console.Clear();

            Console.WriteLine("For wich weapon did you get a new skin?");
            Weapon weapon = WeaponSelector();
            Console.Clear();

            Console.WriteLine("\nEnter the name of your skin:");
            string name = NameSelector(weapon);
            Console.Clear();

            Console.WriteLine($"In wich condition did you get the {name} ?");
            Conditions condition = ConditionSelector();
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

            Console.WriteLine($"Is the '{skin}' for {skin.buyPrice}€ correct?");
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

        private Conditions ConditionSelector()
        {
            Console.WriteLine("0 - FactoryNew");
            Console.WriteLine("1 - MinimalWear");
            Console.WriteLine("2 - FieldTested");
            Console.WriteLine("3 - WellWorn");
            Console.WriteLine("4 - BattleScarred");

            return (Conditions)IntUiSelector(4);
        }

        private string NameSelector(Weapon weapon)
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
            string toSearch = $"{SkinStrings.defaultWeapons[(int)weapon]} | {skinName} (Field-Tested)";
            bool success = completeMarket.Contains(toSearch);

            if (!success)
            {
                Console.Clear();
                Console.WriteLine($"This skin dosn't exist. Weapon selected: {weapon}. Try again:");
                return NameSelector(weapon);
            }

            return skinName;
        }

        #region WeaponTypeSelection
        private Weapon WeaponSelector()
        {
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

            int offset = 0;
            int categoryCount = 0;

            //TODO: this whole thing is awfully writen
            switch (category)
            {
                case 0:
                    offset = 0;
                    categoryCount = PrintWeapons(offset, 10);
                    break;
                case 1:
                    offset = 10;
                    categoryCount = PrintWeapons(offset, 14);
                    break;
                case 2:
                    offset = 14;
                    categoryCount = PrintWeapons(offset, 21);
                    break;
                case 3:
                    offset = 21;
                    categoryCount = PrintWeapons(offset, 28);
                    break;
                case 4:
                    offset = 28;
                    categoryCount = PrintWeapons(offset, 30);
                    break;
                case 5:
                    offset = 30;
                    categoryCount = PrintWeapons(offset, 34);
                    break;
            }

            int choosenWeapon = IntUiSelector(categoryCount);

            return (Weapon)(offset + choosenWeapon);
        }

        private int PrintWeapons(int startIndex, int endIndex)
        {
            int count = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                Console.WriteLine(count + " - " + SkinStrings.defaultWeapons[i]);
                count++;
            }

            return endIndex - startIndex - 1;
        }
        #endregion
    }
}
