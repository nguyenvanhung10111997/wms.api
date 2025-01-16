using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IRoleService : IDisposable
    {
        Task<CRUDResult<IEnumerable<RoleRes>>> ReadAll();
        Task<CRUDResult<RoleRes>> ReadByID(int id);
        Task<CRUDResult<bool>> Create(RoleCreateReq obj, int userId);
        Task<CRUDResult<bool>> Update(RoleUpdateReq obj, int userId);
        Task<CRUDResult<bool>> Delete(int id, int userId);
    }
}
