using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.IO;

namespace CSSkinScrapper
{
    internal class ExcelInterop
    {
        private string path;
        private Workbook workBook;
        private Worksheet workSheet;
        private Range wr;

        public ExcelInterop(string _path)
        {
            path = _path + "CSGO-Skins.xlsx";
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

        public void WriteForm(SaveFile saveFile)
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
            wr.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(180, 198, 231));
            //wr.Cells.Borders.LineStyle = XlLineStyle.xlContinuous;

            //kaufpreise formatieren
            wr = workSheet.get_Range("D4", "D" + (skinCount + 3));
            wr.Font.Size = 16;
            wr.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 235, 156));
            wr.Font.Color = ColorTranslator.ToOle(Color.FromArgb(156, 87, 0));
            //wr.Cells.Borders.LineStyle = XlLineStyle.xlContinuous;

            //aktuelle rechnung
            wr = workSheet.get_Range("H4", "H" + (skinCount + 3));
            wr.Formula = "=G4 - $D4";
            wr.Font.Size = 16;
        }

        public void WritePrices(string[,] skinPriceArray)
        {
            //aktuelle preise in "aktuell" spalte schreiben
            int skinCount = skinPriceArray.GetLength(0);

            for (int i = 0; i < skinCount; i++)
            {
                wr = workSheet.Cells[i + 4, 7];
                wr.Value = skinPriceArray[i, 0];
                wr.Font.Size = 16;
                wr.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 217, 102));
                wr.Font.Color = ColorTranslator.ToOle(Color.FromArgb(156, 87, 0));

                //rendite färben
                wr = workSheet.Cells[i + 4, 8];
                double win = wr.Cells.Value;
                if (win > 0)
                {
                    wr.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(198, 239, 206));
                    wr.Font.Color = ColorTranslator.ToOle(Color.FromArgb(0, 97, 0));
                }
                else
                {
                    wr.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(255, 199, 206));
                    wr.Font.Color = ColorTranslator.ToOle(Color.FromArgb(156, 0, 6));
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
            wr.Interior.Color = ColorTranslator.ToOle(Color.CornflowerBlue);

            wr = workSheet.Cells[2, 4];
            wr.Value = "Kaufpreise:";
            wr.Font.Size = 16;
            wr.Font.Bold = true;
            wr.Interior.Color = ColorTranslator.ToOle(Color.CornflowerBlue);
            wr = workSheet.Cells[2, 5];
            wr.Interior.Color = ColorTranslator.ToOle(Color.CornflowerBlue);

            wr = workSheet.Cells[2, 7];
            wr.Value = "Aktuell:";
            wr.Font.Size = 16;
            wr.Font.Bold = true;
            wr.Interior.Color = ColorTranslator.ToOle(Color.CornflowerBlue);

            wr = workSheet.Cells[3, 7];
            wr.Value = "Preis:";
            wr.Font.Size = 14;
            wr.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(180, 198, 231));

            wr = workSheet.Cells[3, 8];
            wr.Value = "Rendite:";
            wr.Font.Size = 14;
            wr.Interior.Color = ColorTranslator.ToOle(Color.FromArgb(180, 198, 231));
        }

        ~ExcelInterop()
        {
            workBook.Save();
            workBook.Close();
        }
    }
}