namespace wms.infrastructure.Models
{
    public class PermissionModel
    {
        public int RoleId { get; set; }
        public string PermissionId { get; set; }
        public string APIController { get; set; }
        public string APIAction { get; set; }
        public string APIMethod { get; set; }
    }
}
