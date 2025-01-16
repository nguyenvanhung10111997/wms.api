using wms.dto.Common;
using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IOrderService : IDisposable
    {
        Task<CRUDResult<bool>> Create(OrderCreateReq obj, int userId);
        Task<CRUDResult<bool>> UpdateStatus(OrderUpdateStatusReq obj, int userId);
        Task<CRUDResult<bool>> UpdateQuantity(OrderUpdateQuantityReq obj, int userId);
        Task<CRUDResult<bool>> Update(OrderUpdateReq obj, int userId);
        Task<CRUDResult<bool>> Delete(int id, int userId);
        Task<CRUDResult<OrderReadByIDRes>> ReadByID(int id);
        Task<CRUDResult<IEnumerable<OrderReadByLineIDRes>>> ReadByLineID(int lineId);
        Task<PagingResponse<OrderSearchMetricRes>> SearchMetrics(OrderSearchMetricReq obj);
        Task<CRUDResult<ExportExcelRes>> ExportExcelTemplateForUpdateQuantity(OrderExportExcelTemplateForUpdateQuantityReq obj);
        Task<CRUDResult<bool>> ImportForUpdateQuantity(OrderImportForUpdateQuantityReq obj, int userId);
    }
}
