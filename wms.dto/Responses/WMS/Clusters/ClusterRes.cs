using wms.dto.Common;

namespace wms.dto.Responses
{
    public class ClusterRes : BaseRes
    {
        public int ClusterId { get; set; }

        public string ClusterName { get; set; }
    }
}
