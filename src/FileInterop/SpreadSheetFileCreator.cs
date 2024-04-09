using GemBox.Spreadsheet;
using System.Reflection;

namespace CSSkinScrapper.FileInterop
{
    public abstract class SpreadSheetFileCreator : IDisposable
    {
        private ExcelFile excelFile;

        /// <summary>
        /// Can't access this in derived constructors.
        /// </summary>
        protected ExcelWorksheet workSheet { get; private set; }

        public SpreadSheetFileCreator()
        {
            //handle GemBox license
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            SpreadsheetInfo.FreeLimitReached += (s, e) =>
            {
                Console.WriteLine("Wow, you've got to many skins for this program.");
                Console.ReadKey();
                throw new Exception("To many skins for Gembox.Spreadsheet free version.");
            };

            //get directory
            var exeDir = AppDomain.CurrentDomain.BaseDirectory.Replace("CSSkinScrapper.exe", "");

            //this commented code works fine in release build, but dosn't work in this specific publishing config
            //string executable = Assembly.GetExecutingAssembly().Location;
            //string? exeDir = Path.GetDirectoryName(executable);

            if (exeDir is null)
            {
                Console.WriteLine("Where the hell did you put this executable. Please put it somewhere else.");
                Console.ReadKey();
                throw new Exception("Exiting because this dosn't make sense.");
            }

            //load/create spreadsheetFile
            string odsFile = Path.Combine(exeDir, "CSGO-Skins.xlsx");
            if (!File.Exists(odsFile))
            {
                excelFile = new ExcelFile();
                workSheet = excelFile.Worksheets.Add("Revenue calculations");
                Console.WriteLine("Created");
            }
            else
            {
                excelFile = ExcelFile.Load(odsFile);
                workSheet = excelFile.Worksheets[0];
                Console.WriteLine("Loaded");
            }
        }

        public void Dispose()
        {
            Console.WriteLine("\nSaving spreadsheet as file.");
            //.ods also works fine, but can't be loaded by other programs except for the own "ExcelFile.Load" method.
            excelFile.Save("CSGO-Skins.xlsx");
        }
    }
}
