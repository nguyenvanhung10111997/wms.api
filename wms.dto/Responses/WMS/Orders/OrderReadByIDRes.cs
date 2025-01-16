namespace wms.dto.Responses
{
    public class OrderReadByIDRes
    {
        public int OrderId { get; set; }

        public string OrderCode { get; set; }

        public string CustomerName { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int TotalTargetQuantity { get; set; }

        public int TotalWorkers { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderDetailInfo OrderDetail { get; set; }
    }

    public class OrderDetailInfo
    {
        public string ProductCode { get; set; }

        public int LineId { get; set; }

        public List<int> ClusterIds { get; set; }
    }

    public class OrderDetailInfoSQL
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string ProductCode { get; set; }

        public int LineId { get; set; }

        public int ClusterId { get; set; }

        public int StaticShiftId { get; set; }

        public string FixedStartTime { get; set; }

        public string FixedEndTime { get; set; }

        public int TargetQuantity { get; set; }
    }
}
