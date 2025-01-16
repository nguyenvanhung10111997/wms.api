using Dapper;
using wms.business.Services.Interfaces;
using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure;
using wms.infrastructure.Enums;
using wms.infrastructure.Models;

namespace wms.business.Services.Implements
{
    internal class MachineService : BaseService, IMachineService
    {
        public MachineService(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository) : base(repository, readOnlyRepository)
        {
        }

        public async Task<CRUDResult<IEnumerable<MachineRes>>> ReadAll()
        {
            var result = await ReadRepository.StoreProcedureQueryAsync<MachineRes>("dbo.Machine_ReadAll");

            if (result == null || !result.Any())
            {
                return Error<IEnumerable<MachineRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
            }

            return Success(result);
        }

        public async Task<CRUDResult<bool>> Create(MachineCreateReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Machine_Create", param);

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

        public async Task<CRUDResult<bool>> Update(MachineUpdateReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Machine_Update", param);

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
                param.Add("MachineID", id);
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Machine_Delete", param);

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
