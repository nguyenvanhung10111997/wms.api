namespace wms.dto.Requests
{
    public class OrderImportForUpdateQuantityReq
    {
        public List<int> LineIds { get; set; }

        public string FilePath { get; set; }
    }
}
