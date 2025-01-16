using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IPermissionService : IDisposable
    {
        Task<PagingResponse<PermissionSearchRes>> Search(PermissionSearchReq obj);
        Task<CRUDResult<PermissionRes>> ReadByID(int id);
        Task<CRUDResult<PermissionCreateRes>> Create(PermissionCreateReq obj, int userId);
        Task<CRUDResult<bool>> Update(PermissionUpdateReq obj, int userId);
        Task<CRUDResult<bool>> Delete(int id, int userId);
        Task<IEnumerable<PermissionModel>> ReadByClientID(string clientId);
        Task<CRUDResult<IEnumerable<PermissionReadByUserRes>>> ReadByUserID(int userId);
    }
}
