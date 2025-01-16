﻿using Autofac.Features.Indexed;
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
    internal class RoleService : BaseService, IRoleService
    {
        private readonly Lazy<IRepository> _repository;
        private readonly Lazy<IReadOnlyRepository> _readOnlyRepository;

        public RoleService(IIndex<string, Lazy<IRepository>> repository,
            IIndex<string, Lazy<IReadOnlyRepository>> readOnlyRepository)
        {
            _repository = repository[ConnectionEnum.IDS.ToString()];
            _readOnlyRepository = readOnlyRepository[ConnectionEnum.IDS.ToString()];
        }

        public async Task<CRUDResult<IEnumerable<RoleRes>>> ReadAll()
        {
            var result = await _readOnlyRepository.Value.StoreProcedureQueryAsync<RoleRes>("dbo.Role_ReadAll");

            if (result == null || !result.Any())
            {
                return Error<IEnumerable<RoleRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
            }

            return Success(result);
        }

        public async Task<CRUDResult<RoleRes>> ReadByID(int id)
        {
            var param = new DynamicParameters();
            param.Add("ID", id);

            var result = await _readOnlyRepository.Value.StoreProcedureQueryFirstAsync<RoleRes>("dbo.Role_ReadByID", param);

            if (result == null)
            {
                return Error<RoleRes>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
            }

            return Success(result);
        }

        public async Task<CRUDResult<bool>> Create(RoleCreateReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("UserID", userId);

                var executeResult = await _repository.Value.ExecuteAsync("dbo.Role_Create", param);

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

        public async Task<CRUDResult<bool>> Update(RoleUpdateReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("UserID", userId);

                var executeResult = await _repository.Value.ExecuteAsync("dbo.Role_Update", param);

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
                param.Add("RoleID", id);
                param.Add("UserID", userId);

                var executeResult = await _repository.Value.ExecuteAsync("dbo.Role_Delete", param);

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
