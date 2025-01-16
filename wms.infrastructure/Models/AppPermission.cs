namespace wms.infrastructure.Models
{
    public class AppPermission
    {
        public static List<PermissionModel> Data { get; set; }

        public static void LoadPermission(List<PermissionModel> data)
        {
            Data = data;
        }
    }
}
