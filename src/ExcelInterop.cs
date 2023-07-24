using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;
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
                OuterForm();

            wr = workSheet.Cells[1, 1];
        }

        public void WriteForm()
        {
            //schreiben von skinnamen und kaufpreisen
            int skinCount = saveFile.skinCount;

            for (int i = 0; i < skinCount; i++)
            {
                wr = workSheet.Cells[i + 4, 2];
                wr.Value = saveFile.skinNames[i];
                wr = workSheet.Cells[i + 4, 4];
                wr.Value = saveFile.skinBuyPrice[i];
            }

            //Namen formatieren
            wr = workSheet.get_Range("B4", "C" + (skinCount + 3));
            wr.Font.Size = 16;
            wr.Interior.Color = ExcelColors.LightBlue;
            //wr.Cells.Borders.LineStyle = XlLineStyle.xlContinuous;

            //kaufpreise formatieren
            wr = workSheet.get_Range("D4", "D" + (skinCount + 3));
            wr.Font.Size = 16;
            wr.Interior.Color = ExcelColors.LightYellow;
            wr.Font.Color = ExcelColors.DarkerYellow;
            //wr.Cells.Borders.LineStyle = XlLineStyle.xlContinuous;

            //aktuelle rechnung
            wr = workSheet.get_Range("H4", "H" + (skinCount + 3));
            wr.Formula = "=G4 - $D4";
            wr.Font.Size = 16;
        }

        public void WritePrices(string[,] skinPriceArray)
        {
            //aktuelle preise in "aktuell" spalte schreiben
            WriteSinglePriceArray(skinPriceArray, 7);

            //verlauf schreiben
            int colum = 3 * saveFile.runCount + 7;
            wr = workSheet.Cells[2, colum];
            DateTime date = DateTime.Today;
            wr.Value = date;
            wr.Font.Size = 16;
            wr.Font.Bold = true;
            wr.Interior.Color = ExcelColors.DarkerBlue;
            WriteWinLoose(colum);

            wr = workSheet.Cells[4, colum];
            string valueAddress = wr.get_Address(XlReferenceStyle.xlR1C1);
            valueAddress = valueAddress.Replace("$", null);
            valueAddress = valueAddress.Replace("4", null);
            wr = workSheet.Cells[4, colum + 1];
            string address = wr.get_Address(XlReferenceStyle.xlR1C1);
            address = address.Replace("$", null);
            address = address.Replace("4", null);
            wr = workSheet.get_Range(address + 4, address + (saveFile.skinCount + 3));
            wr.Formula = $"={valueAddress + 4} - $D4";
            wr.Font.Size = 16;

            WriteSinglePriceArray(skinPriceArray, colum);
        }

        private void WriteSinglePriceArray(string[,] skinPriceArray, int colum)
        {
            for (int i = 0; i < saveFile.skinCount; i++)
            {
                wr = workSheet.Cells[i + 4, colum];
                wr.Value = skinPriceArray[i, 0];
                wr.Font.Size = 16;
                wr.Interior.Color = ExcelColors.Yellow;
                wr.Font.Color = ExcelColors.DarkerYellow;

                //rendite färben
                wr = workSheet.Cells[i + 4, colum + 1];
                double win = wr.Cells.Value;
                if (win > 0)
                {
                    wr.Interior.Color = ExcelColors.LightGreen;
                    wr.Font.Color = ExcelColors.DarkerGreen;
                }
                else
                {
                    wr.Interior.Color = ExcelColors.LightRed;
                    wr.Font.Color = ExcelColors.DarkerRed;
                }
            }
        }

        private void OuterForm()
        {
            //Überschriften
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

        ~ExcelInterop()
        {
            workBook.Save();
            workBook.Close();
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