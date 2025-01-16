using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IProductService : IDisposable
    {
        Task<CRUDResult<IEnumerable<ProductRes>>> ReadAll();
        Task<CRUDResult<bool>> Create(ProductCreateReq obj, int userId);
        Task<CRUDResult<bool>> Update(ProductUpdateReq obj, int userId);
        Task<CRUDResult<bool>> Delete(int id, int userId);
    }
}
