namespace wms.infrastructure.Models
{
    public class ExcelTemplateExportModel<T>
    {
        public List<string> Headers { get; set; }
        public List<string> ValueHeaders { get; set; }
        public string SheetName { get; set; }
        public List<string> StringFormatColumns { get; set; }
        public List<string> DateFormatColumns { get; set; }
        public List<string> DateTimeFormatColumns { get; set; }
        public IEnumerable<T> Data { get; set; }
        public List<string> NumberFormatColumns { get; set; }
        public string NumberFormatDefault { get; set; }
        public bool IsDataTable { get; set; }
    }
}
