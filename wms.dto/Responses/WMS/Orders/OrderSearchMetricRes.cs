namespace wms.dto.Responses
{
    public class OrderSearchMetricRes
    {
        public int LineId { get; set; }

        public string LineName { get; set; }

        public IEnumerable<OrderSearchMetricClusterInfoRes> Clusters { get; set; }
    }

    public class OrderSearchMetricClusterInfoRes
    {
        public int LineId { get; set; }

        public int OrderId { get; set; }

        public string OrderCode { get; set; }

        public int ClusterId { get; set; }

        public string ClusterName { get; set; }

        public string ProductCode { get; set; }

        public int TotalTargetQuantity { get; set; }

        public int TotalActualQuantity { get; set; }

        public int TotalPrevActualQuantity { get; set; }

        public IEnumerable<OrderSearchMetricClusterDetailInfoRes> Details { get; set; }
    }

    public class OrderSearchMetricClusterDetailInfoRes
    {
        public int StaticShiftId { get; set; }

        public string StaticShiftName { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int TargetQuantity { get; set; }

        public int PrevActualQuantity { get; set; }

        public int ActualQuantity { get; set; }
    }
}
