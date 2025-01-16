using wms.dto.Requests;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IOrderDetailAuditService : IDisposable
    {
        Task<CRUDResult<bool>> Create(OrderDetailAuditCreateReq obj, int userId);
    }
}
