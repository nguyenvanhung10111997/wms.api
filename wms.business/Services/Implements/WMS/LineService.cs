using Dapper;
using wms.business.Services.Interfaces;
using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure;
using wms.infrastructure.Enums;
using wms.infrastructure.Extensions;
using wms.infrastructure.Models;

namespace wms.business.Services.Implements
{
    internal class LineService : BaseService, ILineService
    {
        public LineService(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository) : base(repository, readOnlyRepository)
        {
        }

        public async Task<CRUDResult<IEnumerable<LineRes>>> ReadAll()
        {
            var result = await ReadRepository.StoreProcedureQueryAsync<LineRes>("dbo.Line_ReadAll");

            if (result == null || !result.Any())
            {
                return Error<IEnumerable<LineRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
            }

            return Success(result);
        }

        public async Task<CRUDResult<LineReadByIDRes>> ReadByID(int id)
        {
            var param = new DynamicParameters();
            param.Add("@LineID", id);

            using (var multi = await ReadRepository.StoredProcedureQueryMultiAsync("dbo.Line_ReadByID", param))
            {
                var line = await multi.ReadFirstOrDefaultAsync<LineReadByIDRes>();

                if (line == null)
                {
                    return Error<LineReadByIDRes>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
                }

                var clusters = await multi.ReadAsync<LineClusterRes>();
                line.Clusters = clusters;

                return Success(line);
            }
        }

        public async Task<CRUDResult<bool>> Create(LineCreateReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters(nameof(obj.ClusterIds));
                param.Add("ClusterIDs", obj.ClusterIds.ToSQLSelectStatement());
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Line_Create", param);

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

        public async Task<CRUDResult<bool>> Update(LineUpdateReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters(nameof(obj.ClusterIds));
                param.Add("ClusterIDs", obj.ClusterIds.ToSQLSelectStatement());
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Line_Update", param);

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
                param.Add("LineID", id);
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Line_Delete", param);

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

        public async Task<CRUDResult<IEnumerable<LineReadClusterRes>>> ReadClusters(int lineId)
        {
            var param = new DynamicParameters();
            param.Add("@LineID", lineId);

            var result = await ReadRepository.StoreProcedureQueryAsync<LineReadClusterRes>("dbo.Line_ReadClusters", param);

            if (result == null || !result.Any())
            {
                return Error<IEnumerable<LineReadClusterRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
            }

            return Success(result);
        }

        public async Task<CRUDResult<LineReadOrderRes>> ReadOrders(int lineId)
        {
            var param = new DynamicParameters();
            param.Add("@LineID", lineId);

            using (var multi = await ReadRepository.StoredProcedureQueryMultiAsync("dbo.Line_ReadOrders", param))
            {
                var line = await multi.ReadFirstOrDefaultAsync<LineReadOrderRes>();

                if (line == null)
                {
                    return Error<LineReadOrderRes>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
                }

                var orders = await multi.ReadAsync<LineOrderInfo>();
                var orderDetails = await multi.ReadAsync<LineOrderDetaiInfo>();

                if (orderDetails != null && orderDetails.Any())
                {
                    foreach (var order in orders)
                    {
                        order.Details = orderDetails.Where(x => x.OrderId == order.OrderId);
                    }
                }

                line.Orders = orders;

                return Success(line);
            }
        }

        public async Task<CRUDResult<IEnumerable<LineMetricStatisticRes>>> Statistic(LineMetricStatisticReq obj)
        {
            var param = obj.ToDynamicParameters();

            using (var multi = await ReadRepository.StoredProcedureQueryMultiAsync("dbo.LineMetric_Statistic", param))
            {
                var lines = await multi.ReadAsync<LineMetricStatisticRes>();

                if (lines == null || !lines.Any())
                {
                    return Error<IEnumerable<LineMetricStatisticRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
                }

                var clusters = await multi.ReadAsync<LineClusterMetricStatisticRes>();

                if (clusters != null && clusters.Any())
                {
                    foreach (var line in lines)
                    {
                        line.Clusters = clusters.Where(x => x.LineId == line.LineId);
                    }
                }

                return Success(lines);
            }
        }
    }
}
