namespace wms.dto.Responses
{
    public class LineReadOrderRes
    {
        public int LineId { get; set; }

        public string LineName { get; set; }

        public IEnumerable<LineOrderInfo> Orders { get; set; }
    }

    public class LineOrderInfo
    {
        public int OrderId { get; set; }

        public string OrderCode { get; set; }

        public string CustomerName { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int TotalTargetQuantity { get; set; }

        public int TotalWorkers { get; set; }

        public decimal TotalAmount { get; set; }

        public int TotalActualQuantity { get; set; }

        public IEnumerable<LineOrderDetaiInfo> Details { get; set; }
    }

    public class LineOrderDetaiInfo
    {
        public int OrderId { get; set; }

        public int ClusterId { get; set; }

        public string ClusterName { get; set; }

        public string ProductCode { get; set; }

        public int StaticShiftId { get; set; }

        public string StaticShiftName { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int TargetQuantity { get; set; }

        public int ActualQuantity { get; set; }
    }
}
