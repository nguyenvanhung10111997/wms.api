using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IMachineService : IDisposable
    {
        Task<CRUDResult<IEnumerable<MachineRes>>> ReadAll();
        Task<CRUDResult<bool>> Create(MachineCreateReq obj, int userId);
        Task<CRUDResult<bool>> Update(MachineUpdateReq obj, int userId);
        Task<CRUDResult<bool>> Delete(int id, int userId);
    }
}
