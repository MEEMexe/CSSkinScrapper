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

        public ExcelInterop(string _path)
        {
            path = _path + "CSGO-Skins.xlsx";
            Application excelApp = new Application();
            if (!File.Exists(path))
            {
                excelApp.Workbooks.Add().SaveAs(path);
            }
            workBook = excelApp.Workbooks.Open(path);
            workSheet = workBook.Worksheets[1];
        }

        public void WriteForm(SaveFile saveFile)
        {
            Range workRange = workSheet.Cells[2, 2];
            workRange.Value = "Skins:";
            workRange.Font.Size = 16;
            workRange.Font.Bold = true;
            workRange.Interior.Color = ColorTranslator.ToOle(Color.AliceBlue);

        }

        public void WriteData(string[,] skinPriceArray)
        {

        }

        ~ExcelInterop()
        {
            workBook.Save();
            workBook.Close();
            System.Console.ReadKey();
        }
    }
}