using Microsoft.Office.Interop.Excel;
using System.IO;

namespace CSSkinScrapper
{
    internal class ExcelInterop
    {
        private Application excelApp;
        private static string path = "C:\\Users\\nikla\\Desktop\\CS-Skins\\CSGO-Skins.xlsx";

        public ExcelInterop()
        {
            excelApp = new Application();
        }

        public void WriteExcel(string[] skinarray)
        {
            CheckForFile();

            int skinCount = skinarray.GetLength(0) + 2;

            Workbook wb = excelApp.Workbooks.Open(path);
            Worksheet ws = wb.Worksheets[1];

            Range test = ws.Range["I3:I" + skinCount];
            test.set_Value(XlRangeValueDataType.xlRangeValueDefault, skinarray);

            wb.Save();
            wb.Close();
        }

        private void CheckForFile()
        {
            if (!File.Exists(path))
            {
                excelApp.Workbooks.Add().SaveAs(path);
            }
        }
    }
}