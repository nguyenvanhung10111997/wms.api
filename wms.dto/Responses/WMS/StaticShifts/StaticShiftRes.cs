using wms.dto.Common;

namespace wms.dto.Responses
{
    public class StaticShiftRes : BaseRes
    {
        public int StaticShiftId { get; set; }

        public string StaticShiftName { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public bool IsOvertime { get;set; }
    }
}
