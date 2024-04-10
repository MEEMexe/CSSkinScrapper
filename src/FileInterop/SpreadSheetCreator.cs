using CSSkinScrapper.ScrapperImplemantation;
using CSSkinScrapper.SkinType;
using GemBox.Spreadsheet;

namespace CSSkinScrapper.FileInterop
{
    public abstract class SpreadSheetCreator : IDisposable
    {
        private ExcelFile excelFile;

        /// <summary>
        /// Can't access this in derived constructors.
        /// </summary>
        protected ExcelWorksheet workSheet { get; private set; }

        public List<Skin> Init()
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

            //this commented code works fine in release build, but dosn't work in this specific publishing config, though i don't know why
            //string executable = Assembly.GetExecutingAssembly().Location;
            //string? exeDir = Path.GetDirectoryName(executable);

            if (exeDir is null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Where the hell did you put this executable. Please put it somewhere else.\nPress any key to exit.");
                Console.ReadKey();
                throw new Exception("Exiting because this dosn't make sense.");
            }

            //load/create spreadsheetFile
            var skinList = new List<Skin>();
            string odsFile = Path.Combine(exeDir, "CSGO-Skins.xlsx");
            if (!File.Exists(odsFile))
            {
                excelFile = new ExcelFile();
                workSheet = excelFile.Worksheets.Add("Revenue calculations");
                WriteHeaders();

                Console.WriteLine("Spreadsheet created.");
            }
            else
            {
                excelFile = ExcelFile.Load(odsFile);
                workSheet = excelFile.Worksheets[0];

                //this is how you could insert data after creation -> e.g. for adding skins later, adding whole markets later
                //var e = workSheet.Cells.GetSubrangeRelative(6, 7, 1, 1);
                //e.Insert(InsertShiftDirection.Right);
                //e.Remove(RemoveShiftDirection.Left);

                Console.WriteLine("Spreadsheet loaded.");
                //TODO: get skin from sheet and add to skinList
            }
            return skinList;
        }

        private void WriteHeaders()
        {
            int headerRow = 5;
            int headerCol = 1;
            int headerSize = 300;
            int headerBold = 800;
            int valueBold = 450;
            var headerBackground = ExcelColors.DarkerBlue;

            //SkinScrapper headline
            FormatHeader(1, 1, "SkinScrapper 2.0 - CounterStrike skin revenue calculator", 400, false, 1000);
            workSheet.Cells[1, 1].Style.Font.Italic = true;

            //skins table headline
            FormatHeader(headerRow, headerCol, "Skins:", headerSize, true, headerBold, headerBackground);

            //skins buyprice headline
            FormatHeader(headerRow, headerCol + 2, "Kaufpreise:", headerSize, true, headerBold, headerBackground);

            //current table
            FormatHeader(headerRow - 1, headerCol + 5, "Aktuell:", headerSize, true, headerBold, headerBackground);
            FormatHeader(headerRow, headerCol + 5, "Steam:", headerSize, true, headerBold, headerBackground);
            FormatHeader(headerRow, headerCol + 7, "SkinPort:", headerSize, true, headerBold, headerBackground);

            //currentPrice/revenue
            var range = workSheet.Cells.GetSubrangeRelative(headerRow + 1, headerCol + 5, 4, 1);
            range.Style.Font.Size = headerSize - 50;
            range.Style.Font.Weight = valueBold;
            range.Style.FillPattern.GradientColor1 = ExcelColors.LightBlue;
            range[0].Value = "Preis:";
            range[1].Value = "Rendite:";
            range[2].Value = "Preis:";
            range[3].Value = "Rendite:";

            //TODO: look into Insert method from GemBox for adding new skins while spreadsheet already exists
            //SCHLUSS
        }

        //TODO: doubleCell bool is useless
        protected void FormatHeader(int row, int column, string value, int fontSize, bool doubleCell, int boldness, SpreadsheetColor background = new SpreadsheetColor())
        {
            var cell = workSheet.Cells[row, column];
            cell.Value = value;

            var font = cell.Style.Font;
            font.Size = fontSize;
            font.Weight = boldness;

            if (!background.IsEmpty)
            {
                cell.Style.FillPattern.GradientColor1 = background;
                if (doubleCell)
                    workSheet.Cells[row, column + 1].Style.FillPattern.GradientColor1 = background;
            }
        }

        public void Dispose()
        {
            Console.WriteLine("\nSaving spreadsheet as file.");
            //.ods also works fine, but can't be loaded by other programs except for the own "ExcelFile.Load" method.
            excelFile.Save("CSGO-Skins.xlsx");
        }
    }

    public static class ExcelColors
    {
        public static SpreadsheetColor White = SpreadsheetColor.FromArgb(255, 255, 255);
        public static SpreadsheetColor Gray = SpreadsheetColor.FromArgb(208, 206, 206);
        public static SpreadsheetColor Black = SpreadsheetColor.FromArgb(0, 0, 0);
        public static SpreadsheetColor LightBlue = SpreadsheetColor.FromArgb(180, 198, 231);
        public static SpreadsheetColor DarkerBlue = SpreadsheetColor.FromArgb(100, 149, 237);
        public static SpreadsheetColor LightGreen = SpreadsheetColor.FromArgb(198, 239, 206);
        public static SpreadsheetColor DarkerGreen = SpreadsheetColor.FromArgb(0, 97, 0);
        public static SpreadsheetColor LightRed = SpreadsheetColor.FromArgb(255, 199, 206);
        public static SpreadsheetColor DarkerRed = SpreadsheetColor.FromArgb(156, 0, 6);
        public static SpreadsheetColor LightYellow = SpreadsheetColor.FromArgb(255, 235, 156);
        public static SpreadsheetColor Yellow = SpreadsheetColor.FromArgb(255, 217, 102);
        public static SpreadsheetColor DarkerYellow = SpreadsheetColor.FromArgb(156, 87, 0);
    }
}
