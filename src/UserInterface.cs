using System;
using System.Net.Http;

namespace CSSkinScrapper
{
    internal class UserInterface
    {
        private static SkinStrings skinStrings = new SkinStrings();

        public static void NewSkin(SaveFile saveFile, JSONInterop jsonInterop)
        {
            Console.WriteLine("Enter Skin Name:");
            string input = Console.ReadLine();
            var skin = new Skin(input, Weapon.M4A4, false, 69, Conditions.MinimalWear);
            if (SkinScrapper.SkinExists(skin))
            {
                Console.WriteLine(skin);
                Console.WriteLine("YES");
            }


        }
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
