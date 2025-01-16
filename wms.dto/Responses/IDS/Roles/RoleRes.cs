using wms.dto.Common;

namespace wms.dto.Responses
{
    public class RoleRes : BaseRes
    {
        public int RoleId { get;set; }

        public string RoleName { get;set; }

        public string Description { get; set; }
    }
}
