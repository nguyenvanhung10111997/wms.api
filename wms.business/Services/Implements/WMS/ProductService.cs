using Dapper;
using wms.business.Services.Interfaces;
using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure;
using wms.infrastructure.Enums;
using wms.infrastructure.Models;

namespace wms.business.Services.Implements
{
    internal class ProductService : BaseService, IProductService
    {
        public ProductService(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository) : base(repository, readOnlyRepository)
        {
        }

        public async Task<CRUDResult<IEnumerable<ProductRes>>> ReadAll()
        {
            var result = await ReadRepository.StoreProcedureQueryAsync<ProductRes>("dbo.Product_ReadAll");

            if (result == null || !result.Any())
            {
                return Error<IEnumerable<ProductRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
            }

            return Success(result);
        }

        public async Task<CRUDResult<bool>> Create(ProductCreateReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Product_Create", param);

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

        public async Task<CRUDResult<bool>> Update(ProductUpdateReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Product_Update", param);

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

        public async Task<CRUDResult<bool>> Delete(int id, int userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("ProductID", id);
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Product_Delete", param);

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
    }
}
