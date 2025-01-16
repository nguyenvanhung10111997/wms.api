using wms.dto.Common;

namespace wms.dto.Requests
{
    public class MachineUpdateReq : BaseDTO
    {
        public int MachineId { get; set; }

        public string MachineName { get; set; }

        public bool IsSum { get; set; }
    }
}
