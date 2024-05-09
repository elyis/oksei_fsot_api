using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SQLitePCL;

namespace oksei_fsot_api.src.Utility
{
    public class ExcelGenerator<T>
    {
        public async Task<bool> GenerateExcelAsync(
            string[] headers,
            IEnumerable<T> data,
            string pathToStorage,
            string filename)
        {
            return await Task.Run(() =>
            {
                if (!filename.EndsWith(".xlsx"))
                    return false;

                if (!Directory.Exists(pathToStorage))
                    Directory.CreateDirectory(pathToStorage);

                var workbook = new XSSFWorkbook();
                var cellStyle = CreateCellStyle(workbook);
                var sheet = workbook.CreateSheet();
                var headerRow = sheet.CreateRow(0);

                for (int i = 0; i < headers.Length; i++)
                {
                    sheet.AutoSizeColumn(i);
                    var cell = headerRow.CreateCell(i);
                    cell.CellStyle = cellStyle;
                    cell.SetCellValue(headers[i]);
                }

                FillSheetWithData(sheet, data, cellStyle);

                SetColumnWidths(sheet, data, headers);
                SaveWorkbookToFile(workbook, filename, pathToStorage);
                return true;
            });
        }

        public async Task<byte[]?> GetExcelAsync(string pathToStorage, string filename)
        {
            var fullpath = $"{pathToStorage}{filename}";
            if (!File.Exists(fullpath))
                return null;

            using Stream fileStream = File.OpenRead(fullpath);
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        private ICellStyle CreateCellStyle(XSSFWorkbook workbook)
        {
            var cellStyle = workbook.CreateCellStyle();
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.Alignment = HorizontalAlignment.Center;
            return cellStyle;
        }

        private void FillSheetWithData(ISheet sheet, IEnumerable<T> data, ICellStyle cellStyle)
        {
            var currentRow = 1;
            var properties = typeof(T).GetProperties();

            foreach (var item in data)
            {
                var newRow = sheet.CreateRow(currentRow);

                for (int i = 0; i < properties.Length; i++)
                {
                    var propertyValue = properties[i].GetValue(item, null)?.ToString();
                    var newCell = newRow.CreateCell(i);
                    newCell.CellStyle = cellStyle;
                    newCell.SetCellValue(propertyValue);
                }
                currentRow++;
            }
        }

        private void SetColumnWidths(ISheet sheet, IEnumerable<T> data, string[] headers)
        {
            var headerLengths = headers.Select(e => e.Length).ToList();
            foreach (var item in data)
            {
                var properties = typeof(T).GetProperties();
                for (int currentHeaderIndex = 0; currentHeaderIndex < headers.Length; currentHeaderIndex++)
                {
                    var propertyValueLength = properties.GetValue(currentHeaderIndex)?.ToString().Length;
                    if (propertyValueLength != null && propertyValueLength > headerLengths[currentHeaderIndex])
                        headerLengths[currentHeaderIndex] = (int)propertyValueLength;
                }
            }

            for (int i = 0; i < headerLengths.Count; i++)
                sheet.SetColumnWidth(i, (headerLengths[i] + 1) * 256);
        }

        private void SaveWorkbookToFile(XSSFWorkbook workbook, string filename, string pathToStorage)
        {
            using var fileStream = new FileStream($"{pathToStorage}{filename}", FileMode.Create);
            workbook.Write(fileStream);
        }
    }
}