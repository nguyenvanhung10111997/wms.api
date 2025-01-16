using wms.dto.Common;

namespace wms.dto.Responses
{
    public class ProductRes : BaseRes
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    }
}
