using CSSkinScrapper.Interop;
using Range = Microsoft.Office.Interop.Excel.Range;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.IO;
using System;

namespace CSSkinScrapper
{
    internal class ExcelInterop
    {
        private SaveFile saveFile;
        private string path;
        private Workbook workBook;
        private Worksheet workSheet;

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
            {
                StaticForm();
                SkinForm(true);
            }
        }

        public void WritePrices(double[] skinPriceArray, bool newSkins)
        {
            if (newSkins)
                SkinForm(false);

            int colum = 5 * saveFile.runCount + 12;

            //write current prices in "current" chart
            WriteSinglePriceArray(skinPriceArray, 7);
            WriteSinglePriceArray(skinPriceArray, 9);

            //write history
            WriteDate(colum);  
            WriteSinglePriceArray(skinPriceArray, colum);
            WriteSinglePriceArray(skinPriceArray, colum + 2);
        }

        public void SkinForm(bool newFile)
        {
            if (!newFile)
            {
                int row = saveFile.skinCount + 6;
                ClearCell(row, 2);
                ClearCell(row, 4);
                ClearCell(row, 7);
                ClearCell(row, 8);
            }

            double totalPrice = 0;

            //write skinnames and purchase price
            int skinCount = saveFile.skinCount;

            for (int i = 0; i < skinCount; i++)
            {
                var nameCells = workSheet.Cells[i + 5, 2];
                nameCells.Value = saveFile.skinList[i].name;
                nameCells = workSheet.Cells[i + 5, 4];
                double d = saveFile.skinList[i].buyPrice;
                nameCells.Value = d;
                totalPrice += d;
            }

            //format names
            var names = workSheet.get_Range("B5", "C" + (skinCount + 4));
            names.Font.Size = 16;
            names.Interior.Color = ExcelColors.LightBlue;

            //format purchase price
            var prices = workSheet.get_Range("D5", "D" + (skinCount + 4));
            prices.Font.Size = 16;
            prices.Interior.Color = ExcelColors.LightYellow;
            prices.Font.Color = ExcelColors.DarkerYellow;

            //write total purchase price
            var total = workSheet.Cells[saveFile.skinCount + 6, 4];
            total.Value = totalPrice;
            total.Font.Size = 16;
            total.Interior.Color = ExcelColors.LightYellow;
            total.Font.Color = ExcelColors.DarkerYellow;

            //write SCHLUSS
            var schluss = workSheet.Cells[saveFile.skinCount + 6, 2];
            schluss.Font.Size = 16;
            schluss.Interior.Color = ExcelColors.LightBlue;
            schluss.Value = "SCHLUSS:";
        }

        private void WriteDate(int colum)
        {
            var dateCell = workSheet.Cells[2, colum];
            DateTime date = DateTime.Today;
            dateCell.Value = date;
            dateCell.Font.Size = 16;
            dateCell.Font.Bold = true;
            dateCell.Interior.Color = ExcelColors.DarkerBlue;
            WriteChartHeader(colum);
        }

        private void WriteSinglePriceArray(double[] skinPriceArray, int colum)
        {
            double relativePrice = 0;
            double totalPrice = 0;

            for (int i = 0; i < saveFile.skinCount; i++)
            {
                double price = skinPriceArray[i];
                double win = price - saveFile.skinList[i].buyPrice;
                totalPrice += price;
                relativePrice += win;

                var cPrice = workSheet.Cells[i + 5, colum];
                cPrice.Value = price;
                cPrice.Font.Size = 16;
                cPrice.Interior.Color = ExcelColors.LightYellow;
                cPrice.Font.Color = ExcelColors.DarkerYellow;

                var dif = workSheet.Cells[i + 5, colum + 1];
                dif.Value = win;
                dif.Font.Size = 16;
                ColorWinLoose(dif, win);
            }

            //write total price
            var total = workSheet.Cells[saveFile.skinCount + 6, colum];
            total.Value = totalPrice;
            total.Font.Size = 16;
            total.Interior.Color = ExcelColors.LightYellow;
            total.Font.Color = ExcelColors.DarkerYellow;

            //write win/loose
            var winLoose = workSheet.Cells[saveFile.skinCount + 6, colum + 1];
            winLoose.Value = relativePrice;
            winLoose.Font.Size = 16;
            ColorWinLoose(winLoose, relativePrice);
        }

        private bool ColorWinLoose(Range cell, double value)
        {
            if (value > 0)
            {
                cell.Interior.Color = ExcelColors.LightGreen;
                cell.Font.Color = ExcelColors.DarkerGreen;
                return true;
            }
            else
            {
                cell.Interior.Color = ExcelColors.LightRed;
                cell.Font.Color = ExcelColors.DarkerRed;
                return false;
            }
        }

        private void StaticForm()
        {
            //Headlines
            var skinHead = workSheet.Cells[2, 2];
            skinHead.Value = "Skins:";
            skinHead.Font.Size = 16;
            skinHead.Font.Bold = true;
            skinHead.Interior.Color = ExcelColors.DarkerBlue;

            var buyHead = workSheet.Cells[2, 4];
            buyHead.Value = "Kaufpreise:";
            buyHead.Font.Size = 16;
            buyHead.Font.Bold = true;
            buyHead.Interior.Color = ExcelColors.DarkerBlue;
            buyHead = workSheet.Cells[2, 5];
            buyHead.Interior.Color = ExcelColors.DarkerBlue;

            var currentHead = workSheet.Cells[2, 7];
            currentHead.Value = "Aktuell:";
            currentHead.Font.Size = 16;
            currentHead.Font.Bold = true;
            currentHead.Interior.Color = ExcelColors.DarkerBlue;

            WriteChartHeader(7);
        }

        private void WriteChartHeader(int colum)
        {
            var steamHead = workSheet.Cells[3, colum];
            steamHead.Value = "Steam:";
            steamHead.Font.Size = 16;
            steamHead.Interior.Color = ExcelColors.DarkerBlue;

            var skinPortHead = workSheet.Cells[3, colum + 2];
            skinPortHead.Value = "SkinPort:";
            skinPortHead.Font.Size = 16;
            skinPortHead.Interior.Color = ExcelColors.DarkerBlue;

            WinLooseHeadline(colum);
            WinLooseHeadline(colum + 2);
        }

        private void WinLooseHeadline(int colum)
        {
            var buy = workSheet.Cells[4, colum];
            buy.Value = "Preis:";
            buy.Font.Size = 14;
            buy.Interior.Color = ExcelColors.LightBlue;

            var win = workSheet.Cells[4, colum + 1];
            win.Value = "Rendite:";
            win.Font.Size = 14;
            win.Interior.Color = ExcelColors.LightBlue;
        }

        private void ClearCell(int x, int y)
        {
            var toClear = workSheet.Cells[x, y];
            toClear.Value = "";
            toClear.Interior.Color = ExcelColors.White;
            toClear.Font.Color = ExcelColors.Black;
            toClear.Font.Size = 16;
            toClear.Cells.Borders.LineStyle = XlLineStyle.xlContinuous;
            toClear.Cells.Borders.Color = ExcelColors.Gray;
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
        public static int White = ColorTranslator.ToOle(Color.FromArgb(255, 255, 255));
        public static int Gray = ColorTranslator.ToOle(Color.FromArgb(208, 206, 206));
        public static int Black = ColorTranslator.ToOle(Color.FromArgb(0, 0, 0));
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