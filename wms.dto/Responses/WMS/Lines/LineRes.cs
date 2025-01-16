using wms.dto.Common;

namespace wms.dto.Responses
{
    public class LineRes : BaseRes
    {
        public int LineId { get; set; }

        public string LineName { get; set; }
    }
}
