using Dapper;
using wms.business.Services.Interfaces;
using wms.dto.Requests;
using wms.infrastructure;
using wms.infrastructure.Enums;
using wms.infrastructure.Extensions;
using wms.infrastructure.Models;

namespace wms.business.Services.Implements
{
    internal class OrderDetailAuditService : BaseService, IOrderDetailAuditService
    {
        public OrderDetailAuditService(Lazy<IRepository> repository, Lazy<IReadOnlyRepository> readOnlyRepository) : base(repository, readOnlyRepository)
        {
        }

        public async Task<CRUDResult<bool>> Create(OrderDetailAuditCreateReq obj, int userId)
        {
            try
            {
                var clusterData = obj.Clusters.SelectMany(cluster => cluster.Machines,
                    (cluser, machine) => new OrderDetailAuditCreateSQLParamReq
                    {
                        ClusterId = cluser.ClusterId,
                        ProductCode = cluser.ProductCode,
                        MachineId = machine.MachineId,
                        Quantity = machine.Quantity
                    });

                var param = new DynamicParameters();
                param.Add("LineID", obj.LineId);
                param.Add("ClusterData", clusterData.ToSQLSelectStatement());
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.OrderDetailAudit_Create", param);

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
            base.Dispose(disposing);
        }
    }
}
