using wms.dto.Common;

namespace wms.dto.Responses
{
    public class MachineRes : BaseRes
    {
        public int MachineId { get; set; }

        public string MachineName { get; set; }

        public bool IsSum { get; set; }
    }
}
