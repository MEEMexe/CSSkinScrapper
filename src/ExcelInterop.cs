using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;
using System.Drawing;
using System.IO;
using System;

//So i don't have to google it again:
//wr.Cells.Borders.LineStyle = XlLineStyle.xlContinuous;

namespace CSSkinScrapper
{
    internal class ExcelInterop
    {
        private SaveFile saveFile;
        private string path;
        private Workbook workBook;
        private Worksheet workSheet;
        private Range wr;

        public ExcelInterop(SaveFile _saveFile)
        {
            saveFile = _saveFile;
            path = saveFile.filePath + "CSGO-Skins.xlsx";
            bool existed = File.Exists(path);

            Application excelApp = new Application();

            if (!existed)
                excelApp.Workbooks.Add().SaveAs(path);

            workBook = excelApp.Workbooks.Open(path);
            workSheet = workBook.Worksheets[1];

            if (!existed)
                StaticForm();

            wr = workSheet.Cells[1, 1];
        }

        public void WriteForm()
        {
            double totalPrice = 0;

            //write skinnames and purchase price
            int skinCount = saveFile.skinCount;

            for (int i = 0; i < skinCount; i++)
            {
                wr = workSheet.Cells[i + 4, 2];
                wr.Value = saveFile.skinNames[i];
                wr = workSheet.Cells[i + 4, 4];
                double d = saveFile.skinBuyPrice[i];
                wr.Value = d;
                totalPrice += d;
            }

            //format names
            wr = workSheet.get_Range("B4", "C" + (skinCount + 3));
            wr.Font.Size = 16;
            wr.Interior.Color = ExcelColors.LightBlue;           

            //format purchase price
            wr = workSheet.get_Range("D4", "D" + (skinCount + 3));
            wr.Font.Size = 16;
            wr.Interior.Color = ExcelColors.LightYellow;
            wr.Font.Color = ExcelColors.DarkerYellow;

            //write total price
            wr = workSheet.Cells[saveFile.skinCount + 6, 4];
            wr.Value = totalPrice;
            wr.Font.Size = 16;
            wr.Interior.Color = ExcelColors.LightYellow;
            wr.Font.Color = ExcelColors.DarkerYellow;
        }

        public void WritePrices(double[,] skinPriceArray)
        {
            int colum = 3 * saveFile.runCount + 7;

            //write current prices in "current" chart
            WriteSinglePriceArray(skinPriceArray, 7);

            //write history
            WriteDate(colum);  
            WriteSinglePriceArray(skinPriceArray, colum);
        }

        private void WriteDate(int colum)
        {
            wr = workSheet.Cells[2, colum];
            DateTime date = DateTime.Today;
            wr.Value = date;
            wr.Font.Size = 16;
            wr.Font.Bold = true;
            wr.Interior.Color = ExcelColors.DarkerBlue;
            WriteWinLoose(colum);
        }

        private void WriteSinglePriceArray(double[,] skinPriceArray, int colum)
        {
            double relativePrice = 0;
            double totalPrice = 0;

            for (int i = 0; i < saveFile.skinCount; i++)
            {
                double price = skinPriceArray[i, 0];
                double win = price - saveFile.skinBuyPrice[i];
                totalPrice += price;
                relativePrice += win;

                wr = workSheet.Cells[i + 4, colum];
                wr.Value = price;
                wr.Font.Size = 16;
                wr.Interior.Color = ExcelColors.Yellow;
                wr.Font.Color = ExcelColors.DarkerYellow;

                //color wins/looses
                wr = workSheet.Cells[i + 4, colum + 1];
                wr.Value = win;
                wr.Font.Size = 16;
                ColorWinLoose(win);
            }

            //write total price
            wr = workSheet.Cells[saveFile.skinCount + 6, colum];
            wr.Value = totalPrice;
            wr.Font.Size = 16;
            wr.Interior.Color = ExcelColors.Yellow;
            wr.Font.Color = ExcelColors.DarkerYellow;

            //write win/loose
            wr = workSheet.Cells[saveFile.skinCount + 6, colum + 1];
            wr.Value = relativePrice;
            wr.Font.Size = 16;
            ColorWinLoose(relativePrice);
        }

        private void StaticForm()
        {
            //Headlines
            wr = workSheet.Cells[2, 2];
            wr.Value = "Skins:";
            wr.Font.Size = 16;
            wr.Font.Bold = true;
            wr.Interior.Color = ExcelColors.DarkerBlue;

            wr = workSheet.Cells[2, 4];
            wr.Value = "Kaufpreise:";
            wr.Font.Size = 16;
            wr.Font.Bold = true;
            wr.Interior.Color = ExcelColors.DarkerBlue;
            wr = workSheet.Cells[2, 5];
            wr.Interior.Color = ExcelColors.DarkerBlue;

            wr = workSheet.Cells[2, 7];
            wr.Value = "Aktuell:";
            wr.Font.Size = 16;
            wr.Font.Bold = true;
            wr.Interior.Color = ExcelColors.DarkerBlue;

            WriteWinLoose(7);
        }

        private void WriteWinLoose(int colum)
        {
            wr = workSheet.Cells[3, colum];
            wr.Value = "Preis:";
            wr.Font.Size = 14;
            wr.Interior.Color = ExcelColors.LightBlue;

            wr = workSheet.Cells[3, colum + 1];
            wr.Value = "Rendite:";
            wr.Font.Size = 14;
            wr.Interior.Color = ExcelColors.LightBlue;
        }

        private bool ColorWinLoose(double value)
        {
            if (value > 0)
            {
                wr.Interior.Color = ExcelColors.LightGreen;
                wr.Font.Color = ExcelColors.DarkerGreen;
                return true;
            }
            else
            {
                wr.Interior.Color = ExcelColors.LightRed;
                wr.Font.Color = ExcelColors.DarkerRed;
                return false;
            }
        }

        ~ExcelInterop()
        {
            workBook.Save();
            workBook.Close();
            Console.WriteLine("\nFinalized ExcelInterop.");
        }
    }

    internal static class ExcelColors
    {
        public static int LightBlue = ColorTranslator.ToOle(Color.FromArgb(180, 198, 231));
        public static int DarkerBlue = ColorTranslator.ToOle(Color.CornflowerBlue);
        public static int LightGreen = ColorTranslator.ToOle(Color.FromArgb(198, 239, 206));
        public static int DarkerGreen = ColorTranslator.ToOle(Color.FromArgb(0, 97, 0));
        public static int LightRed = ColorTranslator.ToOle(Color.FromArgb(255, 199, 206));
        public static int DarkerRed = ColorTranslator.ToOle(Color.FromArgb(156, 0, 6));
        public static int LightYellow = ColorTranslator.ToOle(Color.FromArgb(255, 235, 156));
        public static int Yellow = ColorTranslator.ToOle(Color.FromArgb(255, 217, 102));
        public static int DarkerYellow = ColorTranslator.ToOle(Color.FromArgb(156, 87, 0));
    }
}