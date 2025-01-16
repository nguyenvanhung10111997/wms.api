using Autofac.Features.Indexed;
using Dapper;
using wms.business.Enums;
using wms.business.Services.Interfaces;
using wms.dto.Requests;
using wms.dto.Responses;
using wms.ids.business.Configs;
using wms.infrastructure;
using wms.infrastructure.Enums;
using wms.infrastructure.Extensions;
using wms.infrastructure.Models;

namespace wms.business.Services.Implements
{
    internal class UserService : BaseService, IUserService
    {
        private readonly Lazy<IRepository> _repository;
        private readonly Lazy<IReadOnlyRepository> _readOnlyRepository;

        public UserService(IIndex<string, Lazy<IRepository>> repository,
            IIndex<string, Lazy<IReadOnlyRepository>> readOnlyRepository)
        {
            _repository = repository[ConnectionEnum.IDS.ToString()];
            _readOnlyRepository = readOnlyRepository[ConnectionEnum.IDS.ToString()];
        }

        public async Task<PagingResponse<UserSearchRes>> Search(UserSearchReq obj)
        {
            var param = obj.ToDynamicParameters();

            var dataResult = await _readOnlyRepository.Value.StoreProcedureQueryAsync<UserSearchRes>("dbo.User_Search", param);

            if (dataResult == null || !dataResult.Any())
            {
                return new PagingResponse<UserSearchRes> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
            }

            var result = new PagingResponse<UserSearchRes>
            {
                StatusCode = CRUDStatusCodeRes.Success,
                TotalRecord = dataResult.First().TotalRecord,
                PageIndex = obj.PageIndex,
                PageSize = obj.PageSize,
                Records = dataResult
            };

            return result;
        }

        public async Task<CRUDResult<UserRes>> ReadByID(int id)
        {
            var param = new DynamicParameters();
            param.Add("UserID", id);

            var user = await _readOnlyRepository.Value.StoreProcedureQueryFirstAsync<UserRes>("dbo.User_ReadByID", param);

            if (user == null)
            {
                return Error<UserRes>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
            }

            return Success(user);
        }

        public async Task<CRUDResult<UserCreateRes>> Create(UserCreateReq obj, int userId)
        {
            try
            {
                var defaultPassword = ApiConfig.Common.DefaultPassword.ToMD5();

                var param = obj.ToDynamicParameters();
                param.Add("Password", defaultPassword);
                param.Add("LoginUserID", userId);

                var createdUserId = await _repository.Value.ExecuteScalarAsync<int>("dbo.User_Create", param);

                if (createdUserId <= 0)
                {
                    return Error<UserCreateRes>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Dữ liệu chưa được cập nhật");
                }

                return Success(new UserCreateRes { UserId = createdUserId });
            }
            catch (Exception ex)
            {
                return Error<UserCreateRes>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: ex.Message);
            }
        }

        public async Task<CRUDResult<bool>> Update(UserUpdateReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("LoginUserID", userId);

                var executeResult = await _repository.Value.ExecuteAsync("dbo.User_Update", param);

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

        public async Task<CRUDResult<bool>> ChangePassword(UserChangePasswordReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("LoginUserID", userId);

                var executeResult = await _repository.Value.ExecuteAsync("dbo.User_ChangePassword", param);

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

        public async Task<CRUDResult<bool>> Lock(UserLockReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("LoginUserID", userId);

                var executeResult = await _repository.Value.ExecuteAsync("dbo.User_Lock", param);

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
                param.Add("UserID", id);
                param.Add("LoginUserID", userId);

                var executeResult = await _repository.Value.ExecuteAsync("dbo.User_Delete", param);

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
