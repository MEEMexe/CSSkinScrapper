﻿//credits: https://stackoverflow.com/a/58475263

using CSSkinScrapper.UI;
using CSSkinScrapper.Interop;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CSSkinScrapper
{
    internal class UserInterface
    {
        public static bool AskNewSkin(SaveFile saveFile)
        {
            if (CheckInput(saveFile))
            {
                var ui = new NewSkinInterface();
                ui.NewSkin(saveFile.skinList);
                return true;
            }
            return false;
        }

        private static bool CheckInput(SaveFile saveFile)
        {
            if (saveFile.skinCount == 0)
            {
                return true;
            }
            else
            {
                var b = WaitForInput();
                Console.Clear();
                return b;
            }
        }

        private const int STD_INPUT_HANDLE = -10;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern nint GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CancelIoEx(nint handle, nint lpOverlapped);

        private static bool WaitForInput()
        {
            // Start the timeout
            var waitTime = 5;

            var read = false;
            Task.Delay(waitTime * 1000).ContinueWith(_ =>
            {
                if (!read)
                {
                    // Timeout => cancel the console read
                    var handle = GetStdHandle(STD_INPUT_HANDLE);
                    CancelIoEx(handle, nint.Zero);
                }
            });

            try
            {
                // Start reading from the console
                Console.WriteLine("\nDo you want to add new Skins [y] or just scan prices [n]?");
                Console.WriteLine($"\nAutomatically scanning in {waitTime} seconds...");
                var key = Console.ReadKey();
                read = true;
                if (key.KeyChar == 'y')
                {
                    Console.WriteLine("Adding new skin.");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // Handle the exception when the operation is canceled
            catch (InvalidOperationException)
            {
                Console.WriteLine("Just scanning for prices");
                return false;
            }
        }
    }
}
