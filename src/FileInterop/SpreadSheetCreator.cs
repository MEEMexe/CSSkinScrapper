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
                Console.ForegroundColor = ConsoleColor.Red;
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

                Console.WriteLine("Spreadsheet loaded.");

                //getting skinlist from spreadsheet
                int count = 0;
                while (true)
                {
                    var skin = workSheet.Cells[7 + count, 1];
                    var value = skin.Value as string;
                    if (string.IsNullOrWhiteSpace(value) | value == "SCHLUSS:")
                        break;
                    var priceCell = workSheet.Cells[7 + count, 2];
                    var price = double.Parse("" + priceCell.Value);

                    skinList.Add(new Skin(value, price));
                    count++;
                }
            }
            return skinList;
        }

        private void WriteHeaders()
        {
            //some standard values
            int headerRow = 5;
            int headerCol = 1;
            int headerSize = 300;
            int headerBold = 800;
            int valueBold = 450;
            var headerBackground = ExcelColors.DarkerBlue;

            //SkinScrapper headline
            var headerCell = workSheet.Cells[1, 1];
            headerCell.Value = "SkinScrapper 2.0 - CounterStrike skin revenue calculator";
            headerCell.Style.Font.Italic = true;
            headerCell.Style.Font.Size = 400;
            headerCell.Style.Font.Weight = 1000;

            //TODO: passing headerSize, headerBold, headerBackground is completly useless here
            //skins table headline
            FormatHeader(headerRow, headerCol, "Skins:", headerSize, headerBold, headerBackground);

            //skins buyprice headline
            FormatHeader(headerRow, headerCol + 1, "Kaufpreise:", headerSize, headerBold, headerBackground);

            //current table
            FormatHeader(headerRow - 1, headerCol + 3, "Aktuell:", headerSize, headerBold, headerBackground);
            FormatHeader(headerRow, headerCol + 3, "Steam:", headerSize, headerBold, headerBackground);
            FormatHeader(headerRow, headerCol + 5, "SkinPort:", headerSize, headerBold, headerBackground);

            //currentPrice/revenue
            var range = workSheet.Cells.GetSubrangeRelative(headerRow + 1, headerCol + 3, 4, 1);
            range.Style.Font.Size = headerSize - 50;
            range.Style.Font.Weight = valueBold;
            range.Style.FillPattern.GradientColor1 = ExcelColors.LightBlue;
            range[0].Value = "Preis:";
            range[1].Value = "Rendite:";
            range[2].Value = "Preis:";
            range[3].Value = "Rendite:";

            //schluss
            FormatHeader(headerRow + 3, headerCol, "SCHLUSS:", headerSize, headerBold, ExcelColors.LightBlue);
        }

        protected void FormatHeader(int row, int column, string value, int fontSize, int boldness, SpreadsheetColor background)
        {
            var cell = workSheet.Cells[row, column];
            cell.Value = value;

            var font = cell.Style.Font;
            font.Size = fontSize;
            font.Weight = boldness;

            cell.Style.FillPattern.GradientColor1 = background;
            workSheet.Columns[column].AutoFit();
        }

        public void Dispose()
        {
            Console.WriteLine("\nSaving spreadsheet as file.");
            //.ods also works fine, but can't be loaded by other programs except for the own "ExcelFile.Load" method.
            excelFile.Save("CSGO-Skins.xlsx");
        }
    }
}
