using AutoMapper;
using Dapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using wms.business.Services.Interfaces;
using wms.dto.Common;
using wms.dto.Requests;
using wms.dto.Responses;
using wms.ids.business.Configs;
using wms.infrastructure;
using wms.infrastructure.Enums;
using wms.infrastructure.Extensions;
using wms.infrastructure.Models;

namespace wms.business.Services.Implements
{
    internal class OrderService : BaseService, IOrderService
    {
        private readonly IStaticShiftService _staticShiftService;
        private readonly ILineService _lineService;
        private readonly IMapper _mapper;

        public OrderService(Lazy<IRepository> repository,
            Lazy<IReadOnlyRepository> readOnlyRepository,
            IMapper mapper, IStaticShiftService staticShiftService,
            ILineService lineService) : base(repository, readOnlyRepository)
        {
            _mapper = mapper;
            _staticShiftService = staticShiftService;
            _lineService = lineService;
        }

        public async Task<CRUDResult<bool>> Create(OrderCreateReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters(nameof(obj.OrderDetail));
                param.Add("ProductCode", obj.OrderDetail.ProductCode);
                param.Add("LineID", obj.OrderDetail.LineId);
                param.Add("ClusterIDs", obj.OrderDetail.ClusterIds.ToSQLSelectStatement());
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Order_Create", param);

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

        public async Task<CRUDResult<bool>> UpdateStatus(OrderUpdateStatusReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Order_UpdateStatus", param);

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

        public async Task<CRUDResult<bool>> UpdateQuantity(OrderUpdateQuantityReq obj, int userId)
        {
            try
            {
                var totalQuantity = obj.Details.Sum(x => x.TargetQuantity);

                if (obj.TotalTargetQuantity != totalQuantity)
                {
                    return Error<bool>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Số lượng tổng mục tiêu không đồng nhất với mục tiêu được phân bổ");
                }

                var param = obj.ToDynamicParameters(nameof(obj.Details));
                param.Add("OrderDetails", obj.Details.ToSQLSelectStatement());
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Order_UpdateQuantity", param);

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

        public async Task<CRUDResult<bool>> Update(OrderUpdateReq obj, int userId)
        {
            try
            {
                var param = obj.ToDynamicParameters();
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Order_Update", param);

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
                param.Add("OrderID", id);
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Order_Delete", param);

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

        public async Task<CRUDResult<OrderReadByIDRes>> ReadByID(int id)
        {
            var param = new DynamicParameters();
            param.Add("@OrderID", id);

            using (var multi = await ReadRepository.StoredProcedureQueryMultiAsync("dbo.Order_ReadByID", param))
            {
                var order = await multi.ReadFirstOrDefaultAsync<OrderReadByIDRes>();

                if (order == null)
                {
                    return Error<OrderReadByIDRes>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
                }

                var orderDetails = await multi.ReadAsync<OrderDetailInfoSQL>();

                if (orderDetails != null && orderDetails.Any())
                {
                    order.OrderDetail = new OrderDetailInfo
                    {
                        LineId = orderDetails.First().LineId,
                        ProductCode = orderDetails.First().ProductCode,
                        ClusterIds = orderDetails.Select(x => x.ClusterId).Distinct().ToList()
                    };
                }

                return Success(order);
            }
        }

        public async Task<CRUDResult<IEnumerable<OrderReadByLineIDRes>>> ReadByLineID(int lineId)
        {
            var param = new DynamicParameters();
            param.Add("@LineID", lineId);

            using (var multi = await ReadRepository.StoredProcedureQueryMultiAsync("dbo.Order_ReadByLineID", param))
            {
                var orders = await multi.ReadAsync<OrderReadByLineIDRes>();

                if (orders == null || !orders.Any())
                {
                    return Error<IEnumerable<OrderReadByLineIDRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
                }

                var orderDetails = await multi.ReadAsync<OrderDetailReadByLineIDRes>();

                if (orderDetails != null && orderDetails.Any())
                {
                    foreach (var order in orders)
                    {
                        order.Details = orderDetails.Where(x => x.OrderId == order.OrderId);
                    }
                }

                return Success(orders);
            }
        }

        public async Task<PagingResponse<OrderSearchMetricRes>> SearchMetrics(OrderSearchMetricReq obj)
        {
            var param = obj.ToDynamicParameters();
            param.Add("@LineIds", obj.LineIds.ToSQLSelectStatement());

            using (var multi = await ReadRepository.StoredProcedureQueryMultiAsync("dbo.Order_SearchMetrics", param))
            {
                var lines = await multi.ReadAsync<OrderSearchMetricSQLRes>();

                if (lines == null || !lines.Any())
                {
                    return new PagingResponse<OrderSearchMetricRes> { StatusCode = CRUDStatusCodeRes.ResourceNotFound };
                }

                var dataResult = _mapper.Map<IEnumerable<OrderSearchMetricRes>>(lines);

                var orderDetails = await multi.ReadAsync<OrderSearchMetricDetailSQLRes>();

                if (orderDetails != null && orderDetails.Any())
                {
                    var orderClusters = orderDetails.GroupBy(x => new { x.LineId, x.OrderId, x.OrderCode, x.ProductCode, x.ClusterId, x.ClusterName })
                        .Select(x => new OrderSearchMetricClusterInfoRes
                        {
                            LineId = x.Key.LineId,
                            OrderId = x.Key.OrderId,
                            OrderCode = x.Key.OrderCode,
                            ProductCode = x.Key.ProductCode,
                            ClusterId = x.Key.ClusterId,
                            ClusterName = x.Key.ClusterName,
                            TotalTargetQuantity = x.Max(p => p.TargetQuantity),
                            TotalActualQuantity = x.Max(p => p.ActualQuantity),
                            TotalPrevActualQuantity = x.Max(p => p.PrevActualQuantity)
                        }).ToList();

                    foreach (var orderCluster in orderClusters)
                    {
                        orderCluster.Details = orderDetails.Where(x => x.LineId == orderCluster.LineId && x.ClusterId == orderCluster.ClusterId && x.ProductCode == orderCluster.ProductCode)
                            .Select(x => new OrderSearchMetricClusterDetailInfoRes
                            {
                                StaticShiftId = x.StaticShiftId,
                                StaticShiftName = x.StaticShiftName,
                                StartTime = x.StartTime,
                                EndTime = x.EndTime,
                                TargetQuantity = x.TargetQuantity,
                                PrevActualQuantity = x.PrevActualQuantity,
                                ActualQuantity = x.ActualQuantity
                            });
                    }

                    foreach (var item in dataResult)
                    {
                        item.Clusters = orderClusters.Where(x => x.LineId == item.LineId);
                    }
                }

                var result = new PagingResponse<OrderSearchMetricRes>
                {
                    StatusCode = CRUDStatusCodeRes.Success,
                    TotalRecord = lines.First().TotalRecords,
                    PageIndex = obj.PageIndex,
                    PageSize = obj.PageSize,
                    Records = dataResult
                };

                return result;
            }
        }

        public async Task<CRUDResult<ExportExcelRes>> ExportExcelTemplateForUpdateQuantity(OrderExportExcelTemplateForUpdateQuantityReq obj)
        {
            try
            {
                var filePath = string.Empty;
                var fileName = string.Empty;
                var headers = new List<string>(new string[] { "Tổ/Chuyền", "Tổng số lượng" });
                var valueHeaders = new List<string>(new string[] { "Line", "TotalTargetQuantity" });

                var stringFormatColumns = new List<string>(new string[] { "Tổ/Chuyền" });

                var staticShifts = await _staticShiftService.ReadAll();

                if (staticShifts.StatusCode != CRUDStatusCodeRes.Success)
                {
                    return Error<ExportExcelRes>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Lỗi lấy danh sách ca làm việc");
                }

                staticShifts.Data.ToList().ForEach(x =>
                {
                    headers.Add(x.StaticShiftName);
                    valueHeaders.Add($"StaticShiftID_{x.StaticShiftId}");
                });

                var lines = await _lineService.ReadAll();

                if (lines.StatusCode != CRUDStatusCodeRes.Success)
                {
                    return Error<ExportExcelRes>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Lỗi lấy danh sách tổ/chuyền");
                }

                var lineData = lines.Data.Where(x => obj.LineIds.Any(l => l == x.LineId));
                fileName = DateTime.Now.ToString("yyyyMMddhhmm") + $"_FileMauImportDanhSachMucTieuDonHang.xlsx";

                var referenceSheetLineHeader = new List<string> { "Tổ/chuyền" };
                var referenceSheetLineData = lineData.Select(x => new ExcelTemplateAdditionalSheetBasicData<string>
                {
                    Data = $"{x.LineId}:{x.LineName}"
                }).ToList();

                var referenceSheetStaticShiftHeader = new List<string> { "Ca làm việc" };
                var referenceSheetStaticShiftData = staticShifts.Data.Select(x => new ExcelTemplateAdditionalSheetBasicData<string>
                {
                    Data = $"{x.StaticShiftId}:{x.StaticShiftName}"
                }).ToList();

                var readForUpdateQuantityParam = new DynamicParameters();
                readForUpdateQuantityParam.Add("LineIDs", obj.LineIds.ToSQLSelectStatement());

                var orderData = await ReadRepository.StoreProcedureQueryAsync<object>("dbo.Order_ReadForUpdateQuantity", readForUpdateQuantityParam);

                var exportTemplateModel = ExcelExtension.ExportExcelTemplate(new ExcelTemplateExportModel<object>
                {
                    Headers = headers,
                    ValueHeaders = valueHeaders,
                    SheetName = "DanhSachMucTieuDonHang_Data",
                    StringFormatColumns = stringFormatColumns,
                    Data = orderData,
                    IsDataTable = true
                });

                var additionalLineSheetInfo = new ExcelTemplateAdditionalSheetInfo<ExcelTemplateAdditionalSheetBasicData<string>>
                {
                    Headers = referenceSheetLineHeader,
                    SheetName = "DanhSachToChuyen",
                    Data = referenceSheetLineData,
                    RankList = "A2:A1048576"
                };

                var additionalStaticShiftSheetInfo = new ExcelTemplateAdditionalSheetInfo<ExcelTemplateAdditionalSheetBasicData<string>>
                {
                    Headers = referenceSheetStaticShiftHeader,
                    SheetName = "DanhSachCaLamViec",
                    Data = referenceSheetStaticShiftData
                };

                exportTemplateModel = exportTemplateModel.excelPackage.AddAdditionalSheet(exportTemplateModel.worksheet, additionalLineSheetInfo);
                exportTemplateModel = exportTemplateModel.excelPackage.AddAdditionalSheet(exportTemplateModel.worksheet, additionalStaticShiftSheetInfo);


                filePath = await exportTemplateModel.excelPackage.DownloadExcel(fileName);

                var result = new ExportExcelRes
                {
                    FileName = fileName,
                    FilePath = filePath
                };

                return Success(result);
            }
            catch (Exception ex)
            {
                return Error<ExportExcelRes>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: ex.Message);
            }
        }

        public async Task<CRUDResult<bool>> ImportForUpdateQuantity(OrderImportForUpdateQuantityReq obj, int userId)
        {
            try
            {
                var dataImport = await ImportBaseForUpdateQuantity(obj);

                if (dataImport.StatusCode != CRUDStatusCodeRes.Success)
                {
                    return Error<bool>(statusCode: dataImport.StatusCode, errorMessage: dataImport.ErrorMessage);
                }

                var param = new DynamicParameters();
                param.Add("OrderStaticShifts", dataImport.Data.ToSQLSelectStatement());
                param.Add("UserID", userId);

                var executeResult = await Repository.ExecuteAsync("dbo.Order_ImportForUpdateQuantity", param);

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

        private async Task<CRUDResult<IEnumerable<OrderImportForUpdateQuantitySqlParam>>> ImportBaseForUpdateQuantity(OrderImportForUpdateQuantityReq obj)
        {
            ExpandoObject data = new ExpandoObject();

            var excels = await this.ConvertExcelToList(obj.FilePath);

            if (excels.Status == false || excels.Data == null || !excels.Data.Any())
            {
                string errorMessage = "Lỗi cập nhập danh sách từ excel";
                if (!string.IsNullOrEmpty(excels.InnerErrorMessage))
                {
                    errorMessage = excels.InnerErrorMessage;

                    return Error<IEnumerable<OrderImportForUpdateQuantitySqlParam>>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: errorMessage);
                }
            }

            // Check file size
            if (excels.Data.Count() > ApiConfig.Common.MaxExcelRecord)
            {
                return Error<IEnumerable<OrderImportForUpdateQuantitySqlParam>>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "File excel có dung lượng quá lớn. Vui lòng kiểm tra lại");
            }

            // Check duplicate data
            var duplicateData = excels.Data.GroupBy(c => new { c.Line }).Where(c => c.Count() > 1);

            if (duplicateData != null && duplicateData.Any())
            {
                var firstDuplicateData = duplicateData.FirstOrDefault().FirstOrDefault();
                var errorMessage = $"Tổ/chuyền {firstDuplicateData.Line} bị trùng";

                return Error<IEnumerable<OrderImportForUpdateQuantitySqlParam>>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: errorMessage);
            }

            var totalExcelColumns = ((IDictionary<string, object>)excels.Data.FirstOrDefault()).Count;

            var orderStaticShifts = new List<OrderImportForUpdateQuantitySqlParam>();

            foreach (var item in excels.Data)
            {
                var itemDict = (IDictionary<string, object>)item;

                for (int index = 2; index < totalExcelColumns; index++)
                {
                    var orderStaticShift = new OrderImportForUpdateQuantitySqlParam
                    {
                        LineId = int.Parse(item.Line.Split(':')[0]),
                        TotalTargetQuantity = (int)item.TotalTargetQuantity,
                        StaticShiftId = int.Parse(itemDict.Keys.ElementAt(index).Split('_')[1]),
                        TargetQuantity = int.Parse(itemDict.Values.ElementAt(index).ToString())
                    };

                    orderStaticShifts.Add(orderStaticShift);
                }
            }

            // Check LineID exceptional data
            var exceptData = orderStaticShifts.Select(x => x.LineId).Except(obj.LineIds);

            if (exceptData != null && exceptData.Any())
            {
                return Error<IEnumerable<OrderImportForUpdateQuantitySqlParam>>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Số lượng chuyền được yêu cầu không khớp với số lượng chuyền trong file");
            }

            return new CRUDResult<IEnumerable<OrderImportForUpdateQuantitySqlParam>>
            {
                StatusCode = CRUDStatusCodeRes.Success,
                Data = orderStaticShifts
            };
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
