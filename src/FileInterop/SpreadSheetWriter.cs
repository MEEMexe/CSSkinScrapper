using CSSkinScrapper.SkinType;
using GemBox.Spreadsheet;

namespace CSSkinScrapper.FileInterop
{
    public class SpreadSheetWriter : SpreadSheetCreator
    {
        public void WritePriceArrays(double[][] priceArrays)
        {
            for (int j = 0; j < priceArrays.Length; j++)
            {
                int skinCount = priceArrays[0].Length;
                double revenue = 0;
                double totalPrice = 0;

                for (int i = 0; i < skinCount; i++)
                {
                    int sheetIndex = 7 + i;
                    var priceCell = workSheet.Cells[sheetIndex, 4 + j * 2];
                    priceCell.Style.Font.Size = 250;
                    priceCell.Style.FillPattern.GradientColor1 = ExcelColors.LightYellow;
                    priceCell.Style.Font.Color = ExcelColors.DarkerYellow;

                    var currentPrice = priceArrays[j][i];
                    var revenueCell = workSheet.Cells[sheetIndex, 5 + j * 2];
                    var win = currentPrice - double.Parse("" + workSheet.Cells[sheetIndex, 2].Value);
                    revenueCell.Style.Font.Size = 250;
                    revenueCell.Value = win;
                    priceCell.Value = currentPrice;
                    totalPrice += currentPrice;
                    revenue += win;

                    ColorWinLoose(revenueCell.Style, win);
                }

                var totalCell = workSheet.Cells[7 + skinCount + 1, 4 + j * 2];
                totalCell.Value = totalPrice;
                totalCell.Style.Font.Size = 250;
                totalCell.Style.FillPattern.GradientColor1 = ExcelColors.LightYellow;
                totalCell.Style.Font.Color = ExcelColors.DarkerYellow;
                var totalWinCell = workSheet.Cells[7 + skinCount + 1, 5 + j * 2];
                totalWinCell.Style.Font.Size = 250;
                totalWinCell.Value = revenue;
                ColorWinLoose(totalWinCell.Style, revenue);

                workSheet.Columns[4 + j * 2].AutoFit();
                workSheet.Columns[5 + j * 2].AutoFit();
            }
        }

        public void WriteNewSkins(List<Skin> skins)
        {
            int writtenCount = 0;
            while (true)
            {
                var skin = workSheet.Cells[7 + writtenCount, 1];
                var value = skin.Value as string;
                if (string.IsNullOrWhiteSpace(value))
                    break;
                writtenCount++;
            }

            int toAdd = skins.Count - writtenCount;
            int start = 7 + writtenCount;

            //this is how you could insert data after creation -> e.g. for adding skins later, adding whole markets later
            var e = workSheet.Cells.GetSubrangeRelative(start, 1, 1, toAdd);
            e.Insert(InsertShiftDirection.Down);

            for (int i = 0; i < skins.Count; i++)
            {
                var skinString = skins[i].ToString();
                var cCell = workSheet.Cells[7 + i, 1];
                cCell.Value = skinString;
                cCell.Style.FillPattern.GradientColor1 = ExcelColors.LightBlue;
                cCell.Style.Font.Size = 250;

                var priceCell = workSheet.Cells[7 + i, 2];
                priceCell.Value = skins[i].buyPrice;
                priceCell.Style.FillPattern.GradientColor1 = ExcelColors.LightYellow;
                priceCell.Style.Font.Color = ExcelColors.DarkerYellow;
                priceCell.Style.Font.Size = 250;
            }

            workSheet.Columns[1].AutoFit(0.6f);
        }

        private void ColorWinLoose(CellStyle cell, double win)
        {
            if (win > 0)
            {
                cell.FillPattern.GradientColor1 = ExcelColors.LightGreen;
                cell.Font.Color = ExcelColors.DarkerGreen;
            }
            else
            {
                cell.FillPattern.GradientColor1 = ExcelColors.LightRed;
                cell.Font.Color = ExcelColors.DarkerRed;
            }
        }
    }
}
