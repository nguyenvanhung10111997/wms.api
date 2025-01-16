using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IPermissionDetailService : IDisposable
    {
        Task<CRUDResult<IEnumerable<PermissionDetailRes>>> ReadByPermissionID(int permissionId);
        Task<CRUDResult<bool>> Create(PermissionDetailCreateReq obj, int userId);
        Task<CRUDResult<bool>> Update(PermissionDetailUpdateReq obj, int userId);
        Task<CRUDResult<bool>> Delete(int id, int userId);
    }
}
