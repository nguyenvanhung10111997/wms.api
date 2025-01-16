using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IUserRoleService : IDisposable
    {
        Task<CRUDResult<IEnumerable<UserRoleRes>>> ReadByUserID(int userId);
        Task<CRUDResult<IEnumerable<UserRoleRes>>> ReadByRoleID(int roleId);
        Task<CRUDResult<bool>> Create(UserRoleCreateReq obj, int userId);
        Task<CRUDResult<bool>> Delete(int id, int userId);
    }
}
