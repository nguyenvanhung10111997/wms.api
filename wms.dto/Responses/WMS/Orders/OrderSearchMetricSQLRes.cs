namespace wms.dto.Responses
{
    public class OrderSearchMetricSQLRes
    {
        public int TotalRecords { get; set; }

        public int LineId { get; set; }

        public string LineName { get; set; }
    }

    public class OrderSearchMetricDetailSQLRes
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public int OrderId { get; set; }

        public string OrderCode { get; set; }

        public int ClusterId { get; set; }

        public string ClusterName { get; set; }

        public string ProductCode { get; set; }

        public int StaticShiftId { get; set; }

        public string StaticShiftName { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int TargetQuantity { get; set; }

        public int PrevActualQuantity { get; set; }

        public int ActualQuantity { get; set; }
    }
}
