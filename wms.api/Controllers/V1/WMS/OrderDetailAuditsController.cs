using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wms.business.Services.Interfaces;
using wms.dto.Requests;
using wms.infrastructure.Configurations;

namespace wms.api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class OrderDetailAuditsController : BaseController
    {
        private readonly IOrderDetailAuditService _orderDetailAuditService;

        public OrderDetailAuditsController(IOrderDetailAuditService orderDetailAuditService)
        {
            _orderDetailAuditService = orderDetailAuditService;
        }

        /// <summary>
        /// IoT API - Create
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost(nameof(Create))]
        [ProducesResponseType(200, Type = typeof(bool))]
        [AllowAnonymous]
        public async Task<IActionResult> Create(OrderDetailAuditCreateReq obj)
        {
            var result = await _orderDetailAuditService.Create(obj, 1);
            return ApiOK(result);
        }
    }
}
