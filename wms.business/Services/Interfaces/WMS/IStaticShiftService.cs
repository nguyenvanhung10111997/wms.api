using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IStaticShiftService : IDisposable
    {
        Task<CRUDResult<IEnumerable<StaticShiftRes>>> ReadAll();
        Task<CRUDResult<bool>> Create(StaticShiftCreateReq obj, int userId);
        Task<CRUDResult<bool>> Update(StaticShiftUpdateReq obj, int userId);
        Task<CRUDResult<bool>> Delete(int id, int userId);
    }
}
