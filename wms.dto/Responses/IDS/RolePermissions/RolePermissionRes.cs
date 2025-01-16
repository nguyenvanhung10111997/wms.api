using wms.dto.Common;

namespace wms.dto.Responses
{
    public class RolePermissionRes
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public int PermissionId { get; set; }

        public string PermissionName { get; set; }

        public string Description { get; set; }
    }
}
