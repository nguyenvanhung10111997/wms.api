using wms.dto.Common;

namespace wms.dto.Responses
{
    public class LineReadByIDRes : BaseRes
    {
        public int LineId { get; set; }

        public string LineName { get; set; }

        public IEnumerable<LineClusterRes> Clusters { get; set; }
    }

    public class LineClusterRes
    {
        public int ClusterId { get; set; }

        public string ClusterName { get; set; }
    }
}
