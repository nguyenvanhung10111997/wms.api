namespace wms.dto.Responses
{
    public class LineMetricStatisticRes
    {
        public int LineId { get; set; }

        public string LineName { get; set; }

        public int TotalTargetQuantity { get; set; }

        public int TotalWorkers { get; set; }

        public int TotalCurrentQuantity { get; set; }

        public int TotalActualQuantity { get; set; }

        public IEnumerable<LineClusterMetricStatisticRes> Clusters { get; set; }
    }

    public class LineClusterMetricStatisticRes
    {
        public int LineId { get; set; }

        public int ClusterId { get; set; }

        public string ClusterName { get; set; }

        public int TargetQuantity { get; set; }

        public int ActualQuantity { get; set; }
    }
}
