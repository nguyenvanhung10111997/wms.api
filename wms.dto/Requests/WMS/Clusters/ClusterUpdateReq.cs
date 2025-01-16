using wms.dto.Common;

namespace wms.dto.Requests
{
    public class ClusterUpdateReq : BaseDTO
    {
        public int ClusterId { get; set; }

        public string ClusterName { get; set; }
    }
}
