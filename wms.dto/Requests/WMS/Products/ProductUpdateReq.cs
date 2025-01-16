using wms.dto.Common;

namespace wms.dto.Requests
{
    public class ProductUpdateReq : BaseDTO
    {
        public int ProductId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }
    }
}
