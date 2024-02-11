﻿using CSSkinScrapper.Interop;
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
            {
                StaticForm();
                SkinForm(true);
            }

            wr = workSheet.Cells[1, 1];
        }

        public void WritePrices(double[] skinPriceArray, bool newSkins)
        {
            if (newSkins)
                SkinForm(false);

            int colum = 5 * saveFile.runCount + 12;

            //write current prices in "current" chart
            WriteSinglePriceArray(skinPriceArray, 7);

            //write history
            WriteDate(colum);  
            WriteSinglePriceArray(skinPriceArray, colum);
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
                wr = workSheet.Cells[i + 5, 2];
                wr.Value = saveFile.skinList[i].name;
                wr = workSheet.Cells[i + 5, 4];
                double d = saveFile.skinList[i].buyPrice;
                wr.Value = d;
                totalPrice += d;
            }

            //format names
            wr = workSheet.get_Range("B5", "C" + (skinCount + 4));
            wr.Font.Size = 16;
            wr.Interior.Color = ExcelColors.LightBlue;

            //format purchase price
            wr = workSheet.get_Range("D5", "D" + (skinCount + 4));
            wr.Font.Size = 16;
            wr.Interior.Color = ExcelColors.LightYellow;
            wr.Font.Color = ExcelColors.DarkerYellow;

            //write total purchase price
            wr = workSheet.Cells[saveFile.skinCount + 6, 4];
            wr.Value = totalPrice;
            wr.Font.Size = 16;
            wr.Interior.Color = ExcelColors.LightYellow;
            wr.Font.Color = ExcelColors.DarkerYellow;

            //write SCHLUSS
            wr = workSheet.Cells[saveFile.skinCount + 6, 2];
            wr.Font.Size = 16;
            wr.Interior.Color = ExcelColors.LightBlue;
            wr.Value = "SCHLUSS:";
        }

        private void WriteDate(int colum)
        {
            wr = workSheet.Cells[2, colum];
            DateTime date = DateTime.Today;
            wr.Value = date;
            wr.Font.Size = 16;
            wr.Font.Bold = true;
            wr.Interior.Color = ExcelColors.DarkerBlue;
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

                wr = workSheet.Cells[i + 5, colum];
                wr.Value = price;
                wr.Font.Size = 16;
                wr.Interior.Color = ExcelColors.LightYellow;
                wr.Font.Color = ExcelColors.DarkerYellow;

                wr = workSheet.Cells[i + 5, colum + 1];
                wr.Value = win;
                wr.Font.Size = 16;
                ColorWinLoose(win);
            }

            //write total price
            wr = workSheet.Cells[saveFile.skinCount + 6, colum];
            wr.Value = totalPrice;
            wr.Font.Size = 16;
            wr.Interior.Color = ExcelColors.LightYellow;
            wr.Font.Color = ExcelColors.DarkerYellow;

            //write win/loose
            wr = workSheet.Cells[saveFile.skinCount + 6, colum + 1];
            wr.Value = relativePrice;
            wr.Font.Size = 16;
            ColorWinLoose(relativePrice);
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

            WriteChartHeader(7);
        }

        private void WriteChartHeader(int colum)
        {
            wr = workSheet.Cells[3, colum];
            wr.Value = "Steam:";
            wr.Font.Size = 16;
            wr.Interior.Color = ExcelColors.DarkerBlue;

            wr = workSheet.Cells[3, colum + 2];
            wr.Value = "SkinPort:";
            wr.Font.Size = 16;
            wr.Interior.Color = ExcelColors.DarkerBlue;

            WinLooseHeadline(colum);
            WinLooseHeadline(colum + 2);
        }

        private void WinLooseHeadline(int colum)
        {
            wr = workSheet.Cells[4, colum];
            wr.Value = "Preis:";
            wr.Font.Size = 14;
            wr.Interior.Color = ExcelColors.LightBlue;

            wr = workSheet.Cells[4, colum + 1];
            wr.Value = "Rendite:";
            wr.Font.Size = 14;
            wr.Interior.Color = ExcelColors.LightBlue;
        }

        private void ClearCell(int x, int y)
        {
            wr = workSheet.Cells[x, y];
            wr.Value = "";
            wr.Interior.Color = ExcelColors.White;
            wr.Font.Color = ExcelColors.Black;
            wr.Font.Size = 16;
            wr.Cells.Borders.LineStyle = XlLineStyle.xlContinuous;
            wr.Cells.Borders.Color = ExcelColors.Gray;
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