using wms.business.Services.Interfaces;
using wms.dto.Requests;
using wms.infrastructure;
using wms.infrastructure.Enums;
using wms.infrastructure.Models;

namespace wms.business.Services.Implements
{
    internal class OrderDetailService : BaseService, IOrderDetailService
    {
        public OrderDetailService(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository) : base(repository, readOnlyRepository)
        {
        }

        public async Task<CRUDResult<bool>> UpdateQuantity(OrderDetailUpdateQuantityReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.OrderDetail_UpdateQuantity", param);

                if (executeResult <= 0)
                {
                    return Error<bool>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Dữ liệu chưa được cập nhật");
                }

                return Success(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
