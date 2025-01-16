using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IClusterService : IDisposable
    {
        Task<CRUDResult<IEnumerable<ClusterRes>>> ReadAll();
        Task<CRUDResult<bool>> Create(ClusterCreateReq obj, int userId);
        Task<CRUDResult<bool>> Update(ClusterUpdateReq obj, int userId);
        Task<CRUDResult<bool>> Delete(int id, int userId);
    }
}
