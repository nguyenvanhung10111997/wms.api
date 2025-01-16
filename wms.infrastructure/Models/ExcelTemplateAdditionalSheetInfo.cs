namespace wms.infrastructure.Models
{
    public class ExcelTemplateAdditionalSheetInfo<T>
    {
        public string SheetName { get; set; }

        public List<string> Headers { get; set; }

        public string RankList { get; set; }

        public List<T> Data { get; set; }

        public bool Hidden { get; set; }
    }

    public class ExcelTemplateAdditionalSheetBasicData<T>
    {
        public T Data { get; set; }
    }
}
