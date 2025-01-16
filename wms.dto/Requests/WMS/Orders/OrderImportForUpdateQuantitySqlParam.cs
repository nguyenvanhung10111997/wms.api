namespace wms.dto.Requests
{
    public class OrderImportForUpdateQuantitySqlParam
    {
        public int LineId { get; set; }

        public int TotalTargetQuantity { get; set; }

        public int StaticShiftId { get; set; }

        public int TargetQuantity { get; set; }
    }
}
