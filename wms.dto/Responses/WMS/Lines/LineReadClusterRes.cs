namespace wms.dto.Responses
{
    public class LineReadClusterRes
    {
        public int LineClusterId { get; set; }

        public int LineId { get; set; }

        public int ClusterId { get; set; }

        public string ClusterName { get; set; }

        public bool IsUsed { get; set; }
    }
}
