using Microsoft.AspNetCore.Mvc;
using wms.business.Services.Interfaces;
using wms.dto.Common;
using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Attributes;
using wms.infrastructure.Configurations;
using wms.infrastructure.Models;

namespace wms.api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost(nameof(Create))]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ApiAuthorize]
        public async Task<IActionResult> Create(OrderCreateReq obj)
        {
            var result = await _orderService.Create(obj, CurrentUser.UserID);
            return ApiOK(result);
        }

        /// <summary>
        /// UpdateStatus
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut(nameof(UpdateStatus))]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ApiAuthorize]
        public async Task<IActionResult> UpdateStatus(OrderUpdateStatusReq obj)
        {
            var result = await _orderService.UpdateStatus(obj, CurrentUser.UserID);
            return ApiOK(result);
        }

        /// <summary>
        /// UpdateQuantity
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut(nameof(UpdateQuantity))]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ApiAuthorize]
        public async Task<IActionResult> UpdateQuantity(OrderUpdateQuantityReq obj)
        {
            var result = await _orderService.UpdateQuantity(obj, CurrentUser.UserID);
            return ApiOK(result);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut(nameof(Update))]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ApiAuthorize]
        public async Task<IActionResult> Update(OrderUpdateReq obj)
        {
            var result = await _orderService.Update(obj, CurrentUser.UserID);
            return ApiOK(result);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ApiAuthorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _orderService.Delete(id, CurrentUser.UserID);
            return ApiOK(result);
        }

        /// <summary>
        /// ReadByID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(OrderReadByIDRes))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadByID(int id)
        {
            var result = await _orderService.ReadByID(id);
            return ApiOK(result);
        }

        /// <summary>
        /// ReadByLineID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Line/{lineId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrderReadByLineIDRes>))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadByLineID(int lineId)
        {
            var result = await _orderService.ReadByLineID(lineId);
            return ApiOK(result);
        }

        /// <summary>
        /// SearchMetrics
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("Metrics/Search")]
        [ProducesResponseType(200, Type = typeof(PagingResponse<OrderSearchMetricRes>))]
        [ApiAuthorize]
        public async Task<IActionResult> SearchMetrics(OrderSearchMetricReq obj)
        {
            var result = await _orderService.SearchMetrics(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// ExportExcelTemplateForUpdateQuantity
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("Quantity/ExportTemplate")]
        [ProducesResponseType(200, Type = typeof(ExportExcelRes))]
        [ApiAuthorize]
        public async Task<IActionResult> ExportExcelTemplateForUpdateQuantity(OrderExportExcelTemplateForUpdateQuantityReq obj)
        {
            var result = await _orderService.ExportExcelTemplateForUpdateQuantity(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// ImportForUpdateQuantity
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("Quantity/Import")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ApiAuthorize]
        public async Task<IActionResult> ImportForUpdateQuantity(OrderImportForUpdateQuantityReq obj)
        {
            var result = await _orderService.ImportForUpdateQuantity(obj, CurrentUser.UserID);
            return ApiOK(result);
        }
    }
}
