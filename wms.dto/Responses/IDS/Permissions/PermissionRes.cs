using wms.dto.Common;

namespace wms.dto.Responses
{
    public class PermissionRes : BaseRes
    {
        public int PermissionId { get; set; }

        public string PermissionName { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
    }
}
