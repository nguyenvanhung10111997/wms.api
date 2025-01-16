namespace wms.infrastructure.Models
{
    public class ExcelResult<T>
    {
        public T Data { get; set; }

        public bool Status { get; set; }

        public string InnerErrorMessage { get; set; }

        public string ErrorMessage { get; set; }

        public string Code { get; set; }
    }
}
