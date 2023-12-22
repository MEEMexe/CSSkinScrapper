﻿using System;
using System.Collections.Generic;

namespace CSSkinScrapper
{
    internal class UserInterface
    {
        private static SkinStrings skinStrings = new SkinStrings();

        public void NewSkin(IList<Skin> skinList)
        {
            skinList.Add(NewSkinUI());
            Console.WriteLine("Do you want to add another skin?");
            if (BoolUiSelector())
                NewSkin(skinList);
        }

        private int IntUiSelector(int max)
        {
            var input = Console.ReadLine();
            bool success = int.TryParse(input, out int value);

            if (!success | value < 0 | value > max)
            {
                Console.WriteLine("Enter one of the numbers above:");
                return IntUiSelector(max);
            }

            return value;
        }

        private bool BoolUiSelector()
        {
            Console.WriteLine("Type [y] for yes or [n] for no.");
            var input = Console.ReadLine();

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

            Skin skin = new Skin(name, weapon, statTrak, price, condition);

            Console.WriteLine($"Is the '{skin}' correct?)");
            if (!BoolUiSelector())
            {
                Console.WriteLine("This will restart the SkinSelection from scratch.");
                Console.WriteLine("Are you sure you want to do this?");
                if (BoolUiSelector())
                    return NewSkinUI();
            }
            Console.Clear();
            return skin;
        }

        private double BuyPriceInput()
        {
            string price = Console.ReadLine();
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
            string skinName = Console.ReadLine();
            //TODO: verify -> when SkinPort market is done maybe create instance of it in constructor and put .Exists(Skin) there
            //      skinames can't have a whitespace at the end!
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
            int cat = IntUiSelector(5);
            Console.Clear();

            Console.WriteLine("For wich weapon did you get a new skin?");
            Console.WriteLine("\nFor wich weapon specific:");
            return (Weapon)ChooseWeapon(cat);      
        }

        private int ChooseWeapon(int weaponCategory)
        {
            int offset = 0;
            int categoryCount = 0;

            switch (weaponCategory)
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

            return offset + choosenWeapon;
        }

        private int PrintWeapons(int startIndex, int endIndex)
        {
            int count = 0;
            for (int i = startIndex; i < endIndex; i++)
            {
                Console.WriteLine(count + " - " + skinStrings.weapons[i]);
                count++;
            }

            return endIndex - startIndex;
        }
        #endregion
    }
}
