using Autofac.Features.Indexed;
using Dapper;
using wms.business.Enums;
using wms.business.Services.Interfaces;
using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure;
using wms.infrastructure.Enums;
using wms.infrastructure.Models;

namespace wms.business.Services.Implements
{
    internal class PermissionService : BaseService, IPermissionService
    {
        private readonly Lazy<IRepository> _repository;
        private readonly Lazy<IReadOnlyRepository> _readOnlyRepository;

        public PermissionService(IIndex<string, Lazy<IRepository>> repository,
            IIndex<string, Lazy<IReadOnlyRepository>> readOnlyRepository)
        {
            _repository = repository[ConnectionEnum.IDS.ToString()];
            _readOnlyRepository = readOnlyRepository[ConnectionEnum.IDS.ToString()];
        }

        public async Task<PagingResponse<PermissionSearchRes>> Search(PermissionSearchReq obj)
        {
            var param = obj.ToDynamicParameters();

            var dataResult = await _readOnlyRepository.Value.StoreProcedureQueryAsync<PermissionSearchRes>("dbo.Permission_Search", param);

            if (dataResult == null || !dataResult.Any())
            {
                return new PagingResponse<PermissionSearchRes> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            }

            var result = new PagingResponse<PermissionSearchRes>
            {
                StatusCode = CRUDStatusCodeRes.Success,
                TotalRecord = dataResult.First().TotalRecord,
                PageIndex = obj.PageIndex,
                PageSize = obj.PageSize,
                Records = dataResult
            };

            return result;
        }

        public async Task<CRUDResult<PermissionRes>> ReadByID(int id)
        {
            var param = new DynamicParameters();
            param.Add("ID", id);

            var result = await _readOnlyRepository.Value.StoreProcedureQueryFirstAsync<PermissionRes>("dbo.Permission_ReadByID", param);

            if (result == null)
            {
                return Error<PermissionRes>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
            }

            return Success(result);
        }

        public async Task<CRUDResult<PermissionCreateRes>> Create(PermissionCreateReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("UserID", userId);

                var createdPermissionId = await _repository.Value.ExecuteScalarAsync<int>("dbo.Permission_Create", param);

                if (createdPermissionId <= 0)
                {
                    return Error<PermissionCreateRes>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Dữ liệu chưa được cập nhật");
                }

                return Success(new PermissionCreateRes { PermissionId = createdPermissionId });
            }
            catch (Exception ex)
            {
                return Error<PermissionCreateRes>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: ex.Message);
            }
        }

        public async Task<CRUDResult<bool>> Update(PermissionUpdateReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("UserID", userId);

                var executeResult = await _repository.Value.ExecuteAsync("dbo.Permission_Update", param);

                if (executeResult <= 0)
                {
                    return Error<bool>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Dữ liệu chưa được cập nhật");
                }

                return Success(true);
            }
            catch (Exception ex)
            {
                return Error<bool>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: ex.Message);
            }
        }

        public async Task<CRUDResult<bool>> Delete(int id, int userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("PermissionID", id);
                param.Add("UserID", userId);

                var executeResult = await _repository.Value.ExecuteAsync("dbo.Permission_Delete", param);

                if (executeResult <= 0)
                {
                    return Error<bool>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Dữ liệu chưa được cập nhật");
                }

                return Success(true);
            }
            catch (Exception ex)
            {
                return Error<bool>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: ex.Message);
            }
        }

        public async Task<IEnumerable<PermissionModel>> ReadByClientID(string clientId)
        {
            var param = new DynamicParameters();
            param.Add("ClientID", clientId);

            var result = await _readOnlyRepository.Value.StoreProcedureQueryAsync<PermissionModel>("dbo.Permission_ReadByClientID", param);

            return result;
        }

        public async Task<CRUDResult<IEnumerable<PermissionReadByUserRes>>> ReadByUserID(int userId)
        {
            var param = new DynamicParameters();
            param.Add("UserID", userId);

            var result = await _readOnlyRepository.Value.StoreProcedureQueryAsync<PermissionReadByUserRes>("dbo.Permission_ReadByUserID", param);

            if (result == null || !result.Any())
            {
                return Error<IEnumerable<PermissionReadByUserRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
            }

            return Success(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_repository != null && _repository.IsValueCreated)
                {
                    _repository.Value.Dispose();
                }

                if (_readOnlyRepository != null && _readOnlyRepository.IsValueCreated)
                {
                    _readOnlyRepository.Value.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
