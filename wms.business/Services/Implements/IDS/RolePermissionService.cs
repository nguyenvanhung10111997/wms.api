using Autofac.Features.Indexed;
using Dapper;
using wms.business.Enums;
using wms.business.Services.Interfaces;
using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure;
using wms.infrastructure.Enums;
using wms.infrastructure.Extensions;
using wms.infrastructure.Models;

namespace wms.business.Services.Implements
{
    internal class RolePermissionService : BaseService, IRolePermissionService
    {
        private readonly Lazy<IRepository> _repository;
        private readonly Lazy<IReadOnlyRepository> _readOnlyRepository;

        public RolePermissionService(IIndex<string, Lazy<IRepository>> repository,
            IIndex<string, Lazy<IReadOnlyRepository>> readOnlyRepository)
        {
            _repository = repository[ConnectionEnum.IDS.ToString()];
            _readOnlyRepository = readOnlyRepository[ConnectionEnum.IDS.ToString()];
        }

        public async Task<CRUDResult<IEnumerable<RolePermissionRes>>> ReadByPermissionID(int permissionId)
        {
            var param = new DynamicParameters();
            param.Add("PermissionID", permissionId);

            var result = await _readOnlyRepository.Value.StoreProcedureQueryAsync<RolePermissionRes>("dbo.RolePermission_ReadByPermissionID", param);

            if (result == null || !result.Any())
            {
                return Error<IEnumerable<RolePermissionRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
            }

            return Success(result);
        }

        public async Task<CRUDResult<IEnumerable<RolePermissionRes>>> ReadByRoleID(int roleId)
        {
            var param = new DynamicParameters();
            param.Add("RoleID", roleId);

            var result = await _readOnlyRepository.Value.StoreProcedureQueryAsync<RolePermissionRes>("dbo.RolePermission_ReadByRoleID", param);

            if (result == null || !result.Any())
            {
                return Error<IEnumerable<RolePermissionRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
            }

            return Success(result);
        }

        public async Task<CRUDResult<bool>> Create(RolePermissionCreateReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("UserID", userId);

                var executeResult = await _repository.Value.ExecuteAsync("dbo.RolePermission_Create", param);

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

        public async Task<CRUDResult<bool>> BulkCreate(RolePermissionBulkCreateReq obj, int userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("RolePermissions", obj.RolePermissions.ToSQLSelectStatement());
                param.Add("UserID", userId);

                var executeResult = await _repository.Value.ExecuteAsync("dbo.RolePermission_BulkCreate", param);

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
                param.Add("ID", id);
                param.Add("UserID", userId);

                var executeResult = await _repository.Value.ExecuteAsync("dbo.RolePermission_Delete", param);

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
