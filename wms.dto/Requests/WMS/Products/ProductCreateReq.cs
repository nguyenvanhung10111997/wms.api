using wms.dto.Common;

namespace wms.dto.Requests
{
    public class ProductCreateReq : BaseDTO
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }
    }
}
