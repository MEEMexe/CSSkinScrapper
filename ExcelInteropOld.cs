using Microsoft.Office.Interop.Excel;
using System.IO;

namespace CSSkinScrapper
{
    internal class ExcelInteropOld
    {
        private string path;
        private Workbook workBook;
        private Worksheet workSheet;

        public ExcelInteropOld(string _path)
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
            int count = saveFile.skinCount;

            workSheet.Cells[2, 2] = "Skin:";

            for(int i = 0; i < count; i++)
            {
                workSheet.Cells[i + 3, 2] = saveFile.skinNames[i];
                workSheet.Cells[i + 3, 4] = saveFile.skinBuyPrice[i];
                
            }



            workSheet.get_Range("B2", "B" + count + 1).Font.Bold = true;
            

        }

        public void WriteData(string[,] skinarray)
        {          
            int skinCount = skinarray.GetLength(0) + 2;

            workSheet.get_Range("G3", "G" + skinCount).Value2 = skinarray;

            Range oRng = workSheet.get_Range("H3", "H" + skinCount);
            oRng.Formula = "=G2 - $D2";
            oRng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);

            workBook.Save();
            workBook.Close();
        }
    }
}