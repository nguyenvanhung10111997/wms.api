using wms.dto.Common;

namespace wms.dto.Responses
{
    public class OrderReadByLineIDRes : BaseRes
    {
        public int OrderId { get; set; }

        public string OrderCode { get; set; }

        public string CustomerName { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int TotalTargetQuantity { get; set; }

        public int TotalWorkers { get; set; }

        public decimal TotalAmount { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public IEnumerable<OrderDetailReadByLineIDRes> Details { get; set; }
    }

    public class OrderDetailReadByLineIDRes
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ClusterId { get; set; }

        public string ClusterName { get; set; }

        public string ProductCode { get; set; }

        public int StaticShiftId { get; set; }

        public string StaticShiftName { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public bool IsOvertime { get; set; }

        public int TargetQuantity { get; set; }
    }
}
