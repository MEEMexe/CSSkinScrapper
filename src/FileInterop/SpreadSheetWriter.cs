using CSSkinScrapper.SkinType;
using GemBox.Spreadsheet;

namespace CSSkinScrapper.FileInterop
{
    public class SpreadSheetWriter : SpreadSheetCreator
    {
        public void WritePriceArrays(double[][] priceArrays)
        {

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
            int start = 6 + writtenCount;

            //this is how you could insert data after creation -> e.g. for adding skins later, adding whole markets later
            var e = workSheet.Cells.GetSubrangeRelative(start, 1, 1, toAdd);
            e.Insert(InsertShiftDirection.Down);

            int maxLenght = 0;

            for (int i = 0; i < skins.Count; i++)
            {
                var skinString = skins[i].ToString();
                if (skinString.Length > maxLenght)
                    maxLenght = skinString.Length;
                var cCell = workSheet.Cells[7 + i, 1];
                cCell.Value = skinString;
                cCell.Style.FillPattern.GradientColor1 = ExcelColors.LightBlue;
                cCell.Style.Font.Size = 250;
            }

            workSheet.Columns[1].AutoFit(0.6f);
        }
    }
}
