using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<PagingResponse<UserSearchRes>> Search(UserSearchReq obj);
        Task<CRUDResult<UserRes>> ReadByID(int id);
        Task<CRUDResult<UserCreateRes>> Create(UserCreateReq obj, int userId);
        Task<CRUDResult<bool>> Update(UserUpdateReq obj, int userId);
        Task<CRUDResult<bool>> ChangePassword(UserChangePasswordReq obj, int userId);
        Task<CRUDResult<bool>> Lock(UserLockReq obj, int userId);
        Task<CRUDResult<bool>> Delete(int id, int userId);
    }
}
