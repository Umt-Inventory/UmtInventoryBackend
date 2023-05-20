using OfficeOpenXml;
using System.Collections.Generic;
using UmtInventoryBackend.Entities;


public class ItemExcelService
{
    public byte[] ExportItemsToExcel(List<Item> items)
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Items");

            // Set the column headers
            worksheet.Cells[1, 1].Value = "Name";
            worksheet.Cells[1, 2].Value = "Description";
            worksheet.Cells[1, 3].Value = "Price";
            worksheet.Cells[1, 4].Value = "Quantity";
            worksheet.Cells[1, 5].Value = "Condition";

            // Apply formatting to the column headers
            using (var range = worksheet.Cells[1, 1, 1, 5])
            {
                range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#4f81bd"));
            }

            // Fill in the data rows
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                worksheet.Cells[i + 2, 1].Value = item.Name;
                worksheet.Cells[i + 2, 2].Value = item.Description;
                worksheet.Cells[i + 2, 3].Value = item.Price;
                worksheet.Cells[i + 2, 4].Value = item.Quantity;
                worksheet.Cells[i + 2, 5].Value = item.Condition.ToString();
            }

            // Auto-fit the columns
            worksheet.Cells.AutoFitColumns();

            // Convert the Excel package to a byte array
            return package.GetAsByteArray();
        }
    }
}