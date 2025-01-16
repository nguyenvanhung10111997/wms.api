namespace wms.dto.Responses
{
    public class UserRoleRes
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }
    }
}
