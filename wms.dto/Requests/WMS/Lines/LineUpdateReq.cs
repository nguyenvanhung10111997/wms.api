using wms.dto.Common;

namespace wms.dto.Requests
{
    public class LineUpdateReq : BaseDTO
    {
        public int LineId { get; set; }

        public string LineName { get; set; }

        public List<int> ClusterIds { get; set; }
    }
}
