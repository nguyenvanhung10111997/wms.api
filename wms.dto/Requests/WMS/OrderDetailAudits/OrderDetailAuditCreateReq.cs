using wms.dto.Common;

namespace wms.dto.Requests
{
    public class OrderDetailAuditCreateReq : BaseDTO
    {
        public int LineId { get; set; }

        public List<OrderDetailAuditCreateClusterInfo> Clusters { get; set; }
    }

    public class OrderDetailAuditCreateClusterInfo
    {
        public int ClusterId { get; set; }

        public string ProductCode { get; set; }

        public List<OrderDetailAuditCreateMachineInfo> Machines { get; set; }
    }

    public class OrderDetailAuditCreateMachineInfo
    {
        public int MachineId { get; set; }

        public int Quantity { get; set; }
    }

    public class OrderDetailAuditCreateSQLParamReq
    {
        public int ClusterId { get; set; }

        public string ProductCode { get; set; }

        public int MachineId { get; set; }

        public int Quantity { get; set; }
    }
}
