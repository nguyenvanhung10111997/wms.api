namespace wms.dto.Responses
{
    public class PermissionReadByUserRes
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public int PermissionId { get; set; }

        public string PermissionName { get; set; }

        public string Description { get; set; }
    }
}
