using Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace CSSkinScrapper
{
    internal class ExcelInterop
    {
        private static string path = "C:\\Users\\nikla\\Desktop\\CS-Skins\\CSGO-Skins.xlsx";

        public static void WriteExcel(string[,] skinarray)
        {
            int skinCount = skinarray.GetLength(0) + 2;

            Application excelApp = new Application();
            Workbook wb = excelApp.Workbooks.Open(path);
            Worksheet ws = wb.Worksheets[1];

            Range test = ws.Range["I3:I" + skinCount];
            test.set_Value(XlRangeValueDataType.xlRangeValueDefault, skinarray);

            wb.Save();
            wb.Close();
        }
    }
}
