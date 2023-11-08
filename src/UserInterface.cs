using System;
using System.Net.Http;

namespace CSSkinScrapper
{
    internal class UserInterface
    {
        private static SkinStrings skinStrings = new SkinStrings();

        public static void NewSkin(SaveFile saveFile, JSONInterop jsonInterop)
        {

        }

        private static Skin NewSkinUI()
        {
            Weapon skinWeapon = WeaponSelector();
            Console.WriteLine("\nEnter the name of your skin:");



            throw new NotImplementedException();
        }

        private static string SkinSelector(Weapon weapon)
        {
            string skinName = Console.ReadLine();
            if (SkinScrapper.SkinExists(skinName, weapon))
            {

            }
        }

        #region WeaponSelection
        private static Weapon WeaponSelector()
        {
            Console.WriteLine("\nFor wich weapon category:");
            int cat = WeaponCategory();

            Console.WriteLine("\nFor wich weapon specific:");
            return (Weapon)ChooseWeapon(cat);      
        }

        private static int WeaponCategory()
        {
            Console.WriteLine("0 - Pistols");
            Console.WriteLine("1 - Shotguns");
            Console.WriteLine("2 - SMGs");
            Console.WriteLine("3 - Rifles");
            Console.WriteLine("4 - LMGs");
            Console.WriteLine("5 - Sniper Rifles");
            int weaponCategory = int.Parse(Console.ReadLine());

            if (weaponCategory < 0 || weaponCategory > 5)
            {
                Console.WriteLine("Enter one of the numbers below:");
                return WeaponCategory();
            }

            return weaponCategory;
        }

        private static int ChooseWeapon(int weaponCategory)
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

            int choosenWeapon = int.Parse(Console.ReadLine());

            if (choosenWeapon < 0 || choosenWeapon > categoryCount)
            {
                Console.WriteLine("Enter one of the numbers below:");
                return ChooseWeapon(weaponCategory);
            }

            return choosenWeapon;
        }

        private static int PrintWeapons(int startIndex, int endIndex)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                Console.WriteLine("0 - " + skinStrings.weapons[i]);
            }

            return endIndex - startIndex;
        }
        #endregion
    }

    internal class OldUserInterface
    {


        public static void NewSkinOld(SaveFile saveFile, JSONInterop jsonInterop)
        {
            NewSkinRecursive(saveFile);
            Console.WriteLine("\nSaving json file.");
            jsonInterop.Save(saveFile).GetAwaiter().GetResult();
        }

        private static void NewSkinRecursive(SaveFile saveFile)
        {
            Tutorial();
            Console.WriteLine("Enter SkinApiName:");
            string? apiSkin = Console.ReadLine();

            string startPhrase = "%20%7C%20";
            string endPhrase = "%20%28";

            int start = apiSkin.IndexOf(startPhrase);
            int end = apiSkin.IndexOf(endPhrase);
            int lenght = end - start - startPhrase.Length;

            string skinName = apiSkin.Substring(start + startPhrase.Length, lenght);
            skinName = skinName.Replace("%20", " ");

            Console.WriteLine($"How much did the {skinName} cost?");
            double price = double.Parse(Console.ReadLine());

            //TODO: pass skinName instead of apiSkin when seperate stores are implemented
            //      !!! skinames can't have a whitespace at the end !!!
            //      add weapon type (enum)
            Skin skin = new Skin(apiSkin, Weapon.M4A4, false, price, Conditions.FactoryNew);
            saveFile.skinList.Add(skin);

            Console.WriteLine("Do you want to add another skin? [y] yes/[n] no");
            if (Console.ReadLine() == "y")
                NewSkinRecursive(saveFile);
        }

        private static void Tutorial()
        {
            Console.WriteLine("Show how to add skins? [y] yes/[n] no");
            string? yesno = Console.ReadLine();

            if (yesno == "y")
            {
                Console.WriteLine("How to add new skins:");
                Console.WriteLine("Search your exact on the steam market.");

                Console.WriteLine("Copy and paste the green highlighted part of the URL below.");
                Console.WriteLine("Base URL:");
                Console.Write("https://steamcommunity.com/market/listings/730/");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("StatTrak%E2%84%A2%20AK-47%20%7C%20Nightwish%20%28Factory%20New%29");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("To Copy:");
                Console.WriteLine("StatTrak%E2%84%A2%20AK-47%20%7C%20Nightwish%20%28Factory%20New%29\n\n");
            }
        }
    }
}
