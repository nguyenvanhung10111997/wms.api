using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface ILineService : IDisposable
    {
        Task<CRUDResult<IEnumerable<LineRes>>> ReadAll();
        Task<CRUDResult<LineReadByIDRes>> ReadByID(int id);
        Task<CRUDResult<bool>> Create(LineCreateReq obj, int userId);
        Task<CRUDResult<bool>> Update(LineUpdateReq obj, int userId);
        Task<CRUDResult<bool>> Delete(int id, int userId);
        Task<CRUDResult<IEnumerable<LineReadClusterRes>>> ReadClusters(int lineId);
        Task<CRUDResult<LineReadOrderRes>> ReadOrders(int lineId);
        Task<CRUDResult<IEnumerable<LineMetricStatisticRes>>> Statistic(LineMetricStatisticReq obj);
    }
}
