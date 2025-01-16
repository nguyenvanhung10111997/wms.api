using wms.dto.Requests;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IOrderDetailService : IDisposable
    {
        Task<CRUDResult<bool>> UpdateQuantity(OrderDetailUpdateQuantityReq obj, int userId);
    }
}
