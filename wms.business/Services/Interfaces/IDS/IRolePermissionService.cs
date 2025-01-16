using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IRolePermissionService : IDisposable
    {
        Task<CRUDResult<IEnumerable<RolePermissionRes>>> ReadByPermissionID(int permissionId);
        Task<CRUDResult<IEnumerable<RolePermissionRes>>> ReadByRoleID(int roleId);
        Task<CRUDResult<bool>> Create(RolePermissionCreateReq obj, int userId);
        Task<CRUDResult<bool>> BulkCreate(RolePermissionBulkCreateReq obj, int userId);
        Task<CRUDResult<bool>> Delete(int id, int userId);
    }
}
