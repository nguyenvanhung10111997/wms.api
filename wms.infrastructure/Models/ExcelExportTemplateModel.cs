using OfficeOpenXml;

namespace wms.infrastructure.Models
{
    public class ExcelExportTemplateModel
    {
        public ExcelPackage excelPackage { get; set; }
        public ExcelWorksheet worksheet { get; set; }
    }
}
