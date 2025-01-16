using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Data;
using System.Drawing;
using wms.infrastructure.Helpers;
using wms.infrastructure.Models;

namespace wms.infrastructure.Extensions
{
    public static class ExcelExtension
    {
        public static async Task<ExcelResult<IEnumerable<T>>> ConvertExcelToList<T>(this object value, string filePath) where T : new()
        {
            try
            {
                T obj = new T();
                List<T> datas = new List<T>();
                List<string> lstNameProp = typeof(T).GetProperties().Select(c => c.Name).ToList();
                List<string> lstNameFile = new List<string>();
                List<Mapping> lstMappingProp = new List<Mapping>();

                var fileBytes = await FileHelper.GetFile(filePath);

                if (fileBytes == null)
                {
                    return new ExcelResult<IEnumerable<T>> { Status = false, Code = "1", Data = null };
                }

                using (var stream = new MemoryStream(fileBytes))
                {
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets["Values"];
                        int totalRows = workSheet.Dimension.Rows;//get total rows counts of excel file
                        int totalColumns = workSheet.Dimension.Columns;// get total columns count of excel file.
                        if (totalRows > 1)
                        {
                            for (int i = 2; i <= totalRows; i++)
                            {
                                var headerModel = new Dictionary<object, string>();
                                for (int j = 1; j <= totalColumns; j++)
                                {
                                    if (workSheet.Cells[i, j].Value != null)
                                        headerModel.Add(workSheet.Cells[1, j].Value, workSheet.Cells[i, j].Value.ToString());
                                    else
                                        headerModel.Add(workSheet.Cells[1, j].Value, "0");
                                }

                                var json = JsonConvert.SerializeObject(headerModel);
                                var header = JsonConvert.DeserializeObject<Mapping>(json);
                                lstMappingProp.Add(new Mapping { Name = header.Name, Title = header.Title, IsNull = header.IsNull });
                            }

                            ExcelWorksheet workSheetData = package.Workbook.Worksheets.FirstOrDefault(x => x.Name.Contains("Data"));
                            totalRows = workSheetData.Dimension.Rows;//get total rows counts of excel file
                            totalColumns = workSheetData.Dimension.Columns;// get total columns count of excel file.

                            for (int i = 2; i <= totalRows; i++)
                            {
                                var excelViewModels = new Dictionary<string, object>();
                                for (int j = 1; j <= totalColumns; j++)
                                {
                                    var title = workSheetData.Cells[1, j].Text;
                                    var headerModel = lstMappingProp.FirstOrDefault(x => title.Equals(x.Title) && !string.IsNullOrWhiteSpace(x.Title));

                                    if (headerModel == null)
                                    {
                                        continue;
                                    }

                                    excelViewModels.Add(headerModel.Name, workSheetData.Cells[i, j].Value);
                                }

                                var data = excelViewModels.ToObject<T>();
                                datas.Add(data);
                            }
                        }
                    }
                }

                if (datas == null || !datas.Any())
                {
                    return new ExcelResult<IEnumerable<T>> { Status = false, ErrorMessage = "File excel không có dữ liệu" };
                }

                return new ExcelResult<IEnumerable<T>> { Status = true, Code = "", Data = datas };
            }
            catch (DataException ex)
            {
                return new ExcelResult<IEnumerable<T>> { Status = false, ErrorMessage = "File excel không đúng format", InnerErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ExcelResult<IEnumerable<T>> { Status = false, ErrorMessage = "File excel không đúng format", InnerErrorMessage = ex.InnerException?.Message };
            }
        }

        public static async Task<ExcelResult<IEnumerable<dynamic>>> ConvertExcelToList(this object value, string filePath)
        {
            try
            {
                var datas = new List<dynamic>();
                var lstNameFile = new List<string>();
                var lstMappingProp = new List<Mapping>();

                var fileBytes = await FileHelper.GetFile(filePath);

                if (fileBytes == null)
                {
                    return new ExcelResult<IEnumerable<dynamic>> { Status = false, Code = "1", Data = null };
                }

                using (var stream = new MemoryStream(fileBytes))
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets["Values"];
                        int totalRows = workSheet.Dimension.Rows;//get total rows counts of excel file
                        int totalColumns = workSheet.Dimension.Columns;// get total columns count of excel file.
                        if (totalRows > 1)
                        {
                            for (int i = 2; i <= totalRows; i++)
                            {
                                var headerModel = new Dictionary<object, string>();
                                for (int j = 1; j <= totalColumns; j++)
                                {
                                    if (workSheet.Cells[i, j].Value != null)
                                        headerModel.Add(workSheet.Cells[1, j].Value, workSheet.Cells[i, j].Value.ToString());
                                    else
                                        headerModel.Add(workSheet.Cells[1, j].Value, "0");
                                }

                                var json = JsonConvert.SerializeObject(headerModel);
                                var header = JsonConvert.DeserializeObject<Mapping>(json);
                                lstMappingProp.Add(new Mapping { Name = header.Name, Title = header.Title, IsNull = header.IsNull });
                            }

                            var workSheetData = package.Workbook.Worksheets.FirstOrDefault(x => x.Name.Contains("Data"));
                            totalRows = workSheetData.Dimension.Rows;//get total rows counts of excel file
                            totalColumns = workSheetData.Dimension.Columns;// get total columns count of excel file.

                            for (int i = 2; i <= totalRows; i++)
                            {
                                var excelViewModels = new Dictionary<string, object>();
                                for (int j = 1; j <= totalColumns; j++)
                                {
                                    var title = workSheetData.Cells[1, j].Text;
                                    var headerModel = lstMappingProp.FirstOrDefault(x => title.Equals(x.Title) && !string.IsNullOrWhiteSpace(x.Title));

                                    if (headerModel == null)
                                    {
                                        continue;
                                    }

                                    excelViewModels.Add(headerModel.Name, workSheetData.Cells[i, j].Value);
                                }

                                var data = excelViewModels.ToObject();
                                datas.Add(data);
                            }
                        }
                    }
                }

                if (datas == null || !datas.Any())
                {
                    return new ExcelResult<IEnumerable<dynamic>> { Status = false, ErrorMessage = "File excel không có dữ liệu" };
                }

                return new ExcelResult<IEnumerable<dynamic>> { Status = true, Code = "", Data = datas };
            }
            catch (DataException ex)
            {
                return new ExcelResult<IEnumerable<dynamic>> { Status = false, ErrorMessage = "File excel không đúng format", InnerErrorMessage = ex.Message };
            }
            catch (Exception ex)
            {
                return new ExcelResult<IEnumerable<dynamic>> { Status = false, ErrorMessage = "File excel không đúng format", InnerErrorMessage = ex.InnerException?.Message };
            }
        }

        public static void ExportExcel(this IDataReader dataReader, string path, string fileName, bool sum, List<string> columnName, string nameSheet, out int totalRow)
        {
            using (ExcelPackage pack = new ExcelPackage())
            {
                ExcelWorksheet ws = pack.Workbook.Worksheets.Add(nameSheet);
                DataTable schemaTable = dataReader.GetSchemaTable();
                ws.Cells["A1"].LoadFromDataReader(dataReader, true, "", TableStyles.Dark10);

                ws.Cells[1, 1, 1, dataReader.FieldCount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[1, 1, 1, dataReader.FieldCount].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                ws.Cells[1, 1, 1, dataReader.FieldCount].Style.Font.Bold = true;
                int lastRow = ws.Dimension.End.Row;
                totalRow = lastRow;
                if (lastRow > 1)
                {
                    foreach (DataRow schemarow in schemaTable.Rows)
                    {
                        int colindex = Convert.ToInt32(schemarow["ColumnOrdinal"].ToString()) + 1;
                        ws.Column(colindex).AutoFit();
                        switch (System.Type.GetType(schemarow["DataType"].ToString()).FullName)
                        {
                            case "System.Int32":
                                ws.Column(colindex).Style.Numberformat.Format = "#,###,###,##0";
                                break;
                            case "System.Int64":
                                ws.Column(colindex).Style.Numberformat.Format = "#,###,###,##0";
                                break;
                            case "System.Double":
                                ws.Column(colindex).Style.Numberformat.Format = "#,###,###,##0";
                                break;
                            case "System.Decimal":
                                ws.Column(colindex).Style.Numberformat.Format = "#,###,###,##0";
                                break;
                            case "System.DateTime":
                                ws.Column(colindex).Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                                break;
                        }
                    }

                    //Sum column
                    if (sum)
                    {
                        ws.Cells[lastRow + 1, 1, lastRow + 1, dataReader.FieldCount].Style.Font.Bold = true;
                        columnName.ForEach(x =>
                        {
                            ws.Cells[lastRow + 1, Convert.ToInt32(x)].Formula =
                                "SUM(" + ws.Cells[2, Convert.ToInt32(x)] + ":" + ws.Cells[lastRow, Convert.ToInt32(x)] +
                                ")";
                            ws.Cells[lastRow + 1, Convert.ToInt32(x)].Style.Numberformat.Format = "#,###,###,##0";
                        });
                    }

                    if (File.Exists(path + fileName))
                        File.Delete(path + fileName);
                    CreateFolder(path);
                    FileStream objStrem = File.Create(path + fileName);
                    objStrem.Close();
                    File.WriteAllBytes(path + fileName, pack.GetAsByteArray());
                }
                else
                {
                    if (File.Exists(path + fileName))
                        File.Delete(path + fileName);
                }
            }
        }

        public static async Task<string> ExportExcel(this DataTable objTable, string fileName, string sheetName)
        {
            using (var excelPackage = new ExcelPackage())
            {
                var ws = excelPackage.Workbook.Worksheets.Add(sheetName);
                ws.Cells["A1"].LoadFromDataTable(objTable, true);
                int colNumber = 1;

                foreach (DataColumn col in objTable.Columns)
                {
                    if (col.DataType == typeof(DateTime))
                    {
                        ws.Column(colNumber).Style.Numberformat.Format = "dd/MM/yyyy hh:mm";
                    }
                    colNumber++;
                }

                ws.Cells.AutoFitColumns();
                ws.Cells[1, 1, 1, objTable.Columns.Count].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[1, 1, 1, objTable.Columns.Count].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                ws.Cells[1, 1, 1, objTable.Columns.Count].Style.Font.Bold = true;

                var result = await FileHelper.Upload(excelPackage.GetAsByteArray(), $"{fileName}.xlsx");

                return result.FilePath;
            }
        }

        public static async Task<string> ExportExcel<T>(this IEnumerable<T> data, List<string> headers, string fileName, string sheetName)
        {
            using (var excelPackage = new ExcelPackage())
            {
                var ws = excelPackage.Workbook.Worksheets.Add(sheetName);
                ws.Cells["A1"].LoadFromCollection(data, true);
                int colNumber = 1;
                var totalColumns = headers.Count;

                foreach (var property in data.First().GetType().GetProperties())
                {
                    if (property.PropertyType == typeof(Nullable<DateTime>) || property.PropertyType == typeof(DateTime))
                    {
                        ws.Column(colNumber).Style.Numberformat.Format = "dd/MM/yyyy hh:mm";
                    }
                    else if (property.PropertyType == typeof(Decimal)
                        || property.PropertyType == typeof(Double)
                        || property.PropertyType == typeof(Int32)
                        || property.PropertyType == typeof(Int64))
                    {
                        ws.Column(colNumber).Style.Numberformat.Format = "#,###,###,##0";
                    }
                    colNumber++;
                }

                for (int i = 0; i < totalColumns; i++)
                {
                    ws.Cells[1, i + 1].Value = headers[i];
                }

                ws.Cells.AutoFitColumns();
                ws.Cells[1, 1, 1, totalColumns].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[1, 1, 1, totalColumns].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                ws.Cells[1, 1, 1, totalColumns].Style.Font.Bold = true;

                var result = await FileHelper.Upload(excelPackage.GetAsByteArray(), $"{fileName}.xlsx");

                return result.FilePath;
            }
        }

        public static async Task<string> ExportExcel(this List<DataTable> objTable, string fileName, bool sum, List<string> columnName)
        {
            if (objTable.Count > 0)
            {
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    foreach (DataTable dataTable in objTable)
                    {
                        ExcelWorksheet ws = excelPackage.Workbook.Worksheets.Add(dataTable.TableName);
                        ws.Cells["A1"].LoadFromDataTable(dataTable, true, TableStyles.Dark10);

                        foreach (DataColumn objTableColumn in dataTable.Columns)
                        {
                            int colindex = Convert.ToInt32(dataTable.Columns.IndexOf(objTableColumn)) + 1;
                            ws.Column(colindex).AutoFit();
                            switch (System.Type.GetType(objTableColumn.DataType.ToString()).FullName)
                            {
                                case "System.Int32":
                                    ws.Column(colindex).Style.Numberformat.Format = "#,###,###,##0";
                                    break;
                                case "System.Int64":
                                    ws.Column(colindex).Style.Numberformat.Format = "#,###,###,##0";
                                    break;
                                case "System.Double":
                                    ws.Column(colindex).Style.Numberformat.Format = "#,###,###,##0";
                                    break;
                                case "System.Decimal":
                                    ws.Column(colindex).Style.Numberformat.Format = "#,###,###,##0";
                                    break;
                                case "System.DateTime":
                                    ws.Column(colindex).Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                                    break;
                            }
                        }

                        ws.Cells.AutoFitColumns();
                        ws.Cells[1, 1, 1, dataTable.Columns.Count].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 1, dataTable.Columns.Count].Style.Fill.BackgroundColor
                            .SetColor(Color.FromArgb(155, 194, 230));
                        ws.Cells[1, 1, 1, dataTable.Columns.Count].Style.Font.Bold = true;

                        int lastRow = ws.Dimension.End.Row;
                        if (sum)
                        {
                            ws.Cells[lastRow + 1, 1, lastRow + 1, dataTable.Columns.Count].Style.Font.Bold = true;
                            columnName.ForEach(x =>
                            {
                                ws.Cells[lastRow + 1, Convert.ToInt32(x)].Formula = "SUM(" + ws.Cells[2, Convert.ToInt32(x)] + ":" + ws.Cells[lastRow, Convert.ToInt32(x)] + ")";
                                ws.Cells[lastRow + 1, Convert.ToInt32(x)].Style.Numberformat.Format = "#,###,###,##0";
                            });
                        }
                    }

                    var result = await FileHelper.Upload(excelPackage.GetAsByteArray(), $"{fileName}.xlsx");
                }
            }

            return string.Empty;
        }

        public static ExcelExportTemplateModel ExportExcelTemplate<T>(ExcelTemplateExportModel<T> model)
        {
            try
            {
                var excelPackage = new ExcelPackage();
                int index = 1;
                var worksheet = excelPackage.Workbook.Worksheets.Add(model.SheetName);

                if (model.Data != null && model.Data.Any())
                {
                    if (model.IsDataTable)
                    {
                        var dataTable = ToDataTable(model.Data);
                        worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                    }
                    else
                    {
                        worksheet.Cells["A1"].LoadFromCollection(model.Data);
                    }
                }

                #region format datatype cell vs name header
                for (int i = 0; i < model.Headers.Count; i++)
                {
                    index = i + 1;
                    if (model.StringFormatColumns != null && model.StringFormatColumns.Any(x => x == model.Headers[index - 1]))
                    {
                        worksheet.Column(index).Style.Numberformat.Format = "@";
                    }
                    else if (model.DateFormatColumns != null && model.DateFormatColumns.Any(x => x == model.Headers[index - 1]))
                    {
                        worksheet.Column(index).Style.Numberformat.Format = "dd/MM/yyyy";
                    }
                    else if (model.DateTimeFormatColumns != null && model.DateTimeFormatColumns.Any(x => x == model.Headers[index - 1]))
                    {
                        worksheet.Column(index).Style.Numberformat.Format = "dd/MM/yyyy hh:mm";
                    }
                    else if (!string.IsNullOrEmpty(model.NumberFormatDefault) && model.NumberFormatColumns != null && model.NumberFormatColumns.Any(x => x == model.Headers[index - 1]))
                    {
                        worksheet.Column(index).Style.Numberformat.Format = model.NumberFormatDefault;
                    }
                    else
                    {
                        worksheet.Column(index).Style.Numberformat.Format = "#,##0";
                    }

                    worksheet.Cells[1, index].Value = model.Headers[i];
                }
                #endregion            

                for (int i = 0; i < model.Headers.Count; i++)
                {
                    index = i + 1;
                    worksheet.Cells[1, index].Style.Font.Bold = true;
                    worksheet.Cells[1, index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, index].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    worksheet.Column(index).AutoFit();
                }

                var valueWorksheet = excelPackage.Workbook.Worksheets.Add("Values");
                valueWorksheet.Cells[1, 1].Value = "Title";
                valueWorksheet.Cells[1, 2].Value = "Name";

                for (int i = 1; i <= model.Headers.Count; i++)
                {
                    index = i + 1;
                    valueWorksheet.Cells[index, 1].Value = model.Headers[i - 1];
                }

                for (int i = 1; i <= model.ValueHeaders.Count; i++)
                {
                    index = i + 1;
                    valueWorksheet.Cells[index, 2].Value = model.ValueHeaders[i - 1];
                }

                valueWorksheet.Hidden = eWorkSheetHidden.Hidden;

                return new ExcelExportTemplateModel
                {
                    excelPackage = excelPackage,
                    worksheet = worksheet
                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<string> DownloadExcel(this ExcelPackage excelPackage, string fileName)
        {
            try
            {
                var result = await FileHelper.Upload(excelPackage.GetAsByteArray(), $"{fileName}.xlsx");

                return result.FilePath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ExcelExportTemplateModel AddAdditionalSheet<T>(this ExcelPackage excelPackage, ExcelWorksheet worksheet, ExcelTemplateAdditionalSheetInfo<T> model)
        {
            var subWorksheet = excelPackage.Workbook.Worksheets.Add(model.SheetName);
            for (int i = 0; i < model.Headers.Count; i++)
            {
                int index = i + 1;
                subWorksheet.Cells[1, index].Value = model.Headers[i];
            }

            subWorksheet.Cells[2, 1].LoadFromCollection(model.Data);

            for (int i = 1; i <= model.Headers.Count; i++)
            {
                subWorksheet.Cells[1, i].Style.Font.Bold = true;
                subWorksheet.Cells[1, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                subWorksheet.Cells[1, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                subWorksheet.Column(i).AutoFit();
            }

            if (!string.IsNullOrEmpty(model.RankList))
            {
                if (model.Hidden)
                {
                    subWorksheet.Hidden = eWorkSheetHidden.Hidden;
                }

                subWorksheet.Cells[2, 1, model.Data.Count + 1, 1].Style.Locked = true;
                var validation = worksheet.DataValidations.AddListValidation(model.RankList);
                validation.ShowErrorMessage = true;
                validation.AllowBlank = true;
                validation.Error = "Chọn giá trị từ danh sách";
                validation.Formula.ExcelFormula = model.SheetName + "!$A$2:$A$" + (model.Data.Count + 1).ToString();
            }

            return new ExcelExportTemplateModel
            {
                excelPackage = excelPackage,
                worksheet = worksheet
            };
        }

        private static bool CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return true;
        }

        private static DataTable ToDataTable<T>(IEnumerable<T> data)
        {
            var dataTable = new DataTable();

            // Check if the data is null or empty
            if (data == null || !data.GetEnumerator().MoveNext())
            {
                return dataTable; // Return an empty DataTable
            }

            // Extract the first item to get property names and types
            var firstItem = (IDictionary<string, object>)data.First();

            // Create columns based on the properties of the first item
            foreach (var key in firstItem.Keys)
            {
                dataTable.Columns.Add(key, firstItem[key]?.GetType() ?? typeof(object));
            }

            // Populate rows
            foreach (var row in data)
            {
                var dataRow = dataTable.NewRow();
                foreach (var key in ((IDictionary<string, object>)row).Keys)
                {
                    dataRow[key] = ((IDictionary<string, object>)row)[key] ?? DBNull.Value;
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }

    public class Mapping
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public int IsNull { get; set; }
    }
}
