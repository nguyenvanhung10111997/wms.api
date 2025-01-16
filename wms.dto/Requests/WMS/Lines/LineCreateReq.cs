using wms.dto.Common;

namespace wms.dto.Requests
{
    public class LineCreateReq : BaseDTO
    {
        public string LineName { get; set; }

        public List<int> ClusterIds { get; set; }
    }
}
