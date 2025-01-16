using wms.business.Services.Interfaces;
using wms.ids.business.Configs;
using wms.infrastructure;
using wms.infrastructure.Models;

namespace wms.api.Configurations
{
    public static class PermissionConfig
    {
        public static void RegisterAppPermission(this IApplicationBuilder app)
        {
            using(var permissionService = Engine.ContainerManager.Resolve<IPermissionService>())
            {
                var permissions = permissionService.ReadByClientID(ApiConfig.Common.ClientID).Result;

                AppPermission.LoadPermission(permissions?.ToList());
            }
        }
    }
}
