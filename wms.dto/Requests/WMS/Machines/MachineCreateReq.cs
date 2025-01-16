using wms.dto.Common;

namespace wms.dto.Requests
{
    public class MachineCreateReq : BaseDTO
    {
        public string MachineName { get; set; }

        public bool IsSum { get; set; }
    }
}
