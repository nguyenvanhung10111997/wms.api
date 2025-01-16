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
    internal class UserRoleService : BaseService, IUserRoleService
    {
        private readonly Lazy<IRepository> _repository;
        private readonly Lazy<IReadOnlyRepository> _readOnlyRepository;

        public UserRoleService(IIndex<string, Lazy<IRepository>> repository,
            IIndex<string, Lazy<IReadOnlyRepository>> readOnlyRepository)
        {
            _repository = repository[ConnectionEnum.IDS.ToString()];
            _readOnlyRepository = readOnlyRepository[ConnectionEnum.IDS.ToString()];
        }

        public async Task<CRUDResult<IEnumerable<UserRoleRes>>> ReadByUserID(int userId)
        {
            var param = new DynamicParameters();
            param.Add("UserID", userId);

            var result = await _readOnlyRepository.Value.StoreProcedureQueryAsync<UserRoleRes>("dbo.UserRole_ReadByUserID", param);

            if (result == null || !result.Any())
            {
                return Error<IEnumerable<UserRoleRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
            }

            return Success(result);
        }

        public async Task<CRUDResult<IEnumerable<UserRoleRes>>> ReadByRoleID(int roleId)
        {
            var param = new DynamicParameters();
            param.Add("RoleID", roleId);

            var result = await _readOnlyRepository.Value.StoreProcedureQueryAsync<UserRoleRes>("dbo.UserRole_ReadByRoleID", param);

            if (result == null || !result.Any())
            {
                return Error<IEnumerable<UserRoleRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
            }

            return Success(result);
        }

        public async Task<CRUDResult<bool>> Create(UserRoleCreateReq obj, int userId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("UserID", obj.UserId);
                param.Add("RoleIDs", obj.RoleIds.ToSQLSelectStatement());
                param.Add("LoginUserID", userId);

                var executeResult = await _repository.Value.ExecuteAsync("dbo.UserRole_Create", param);

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
                param.Add("LoginUserID", userId);

                var executeResult = await _repository.Value.ExecuteAsync("dbo.UserRole_Delete", param);

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
