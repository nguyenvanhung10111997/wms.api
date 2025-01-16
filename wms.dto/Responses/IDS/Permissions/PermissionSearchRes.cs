using Newtonsoft.Json;
using wms.dto.Common;

namespace wms.dto.Responses
{
    public class PermissionSearchRes : BaseRes
    {
        [JsonIgnore]
        public int TotalRecord { get; set; }

        public int PermissionId { get; set; }

        public string PermissionName { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
    }
}
