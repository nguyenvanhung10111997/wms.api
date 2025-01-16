using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wms.business.Services.Interfaces;
using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Attributes;
using wms.infrastructure.Configurations;

namespace wms.api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class LinesController : BaseController
    {
        private readonly ILineService _lineService;

        public LinesController(ILineService LineService)
        {
            _lineService = LineService;
        }

        /// <summary>
        /// ReadAll
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<LineRes>))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadAll()
        {
            var result = await _lineService.ReadAll();
            return ApiOK(result);
        }

        /// <summary>
        /// ReadByID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(LineReadByIDRes))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadByID(int id)
        {
            var result = await _lineService.ReadByID(id);
            return ApiOK(result);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost(nameof(Create))]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ApiAuthorize]
        public async Task<IActionResult> Create(LineCreateReq obj)
        {
            var result = await _lineService.Create(obj, CurrentUser.UserID);
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
        public async Task<IActionResult> Update(LineUpdateReq obj)
        {
            var result = await _lineService.Update(obj, CurrentUser.UserID);
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
            var result = await _lineService.Delete(id, CurrentUser.UserID);
            return ApiOK(result);
        }

        /// <summary>
        /// ReadClusters
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet("{lineId}/clusters")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<LineReadClusterRes>))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadClusters(int lineId)
        {
            var result = await _lineService.ReadClusters(lineId);
            return ApiOK(result);
        }

        /// <summary>
        /// IoT API - ReadOrders
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet("{lineId}/orders")]
        [ProducesResponseType(200, Type = typeof(LineReadOrderRes))]
        [AllowAnonymous]
        public async Task<IActionResult> ReadOrders(int lineId)
        {
            var result = await _lineService.ReadOrders(lineId);
            return ApiOK(result);
        }

        /// <summary>
        /// Statistic
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("metrics/statistic")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<LineMetricStatisticRes>))]
        [ApiAuthorize]
        public async Task<IActionResult> Statistic(LineMetricStatisticReq obj)
        {
            var result = await _lineService.Statistic(obj);
            return ApiOK(result);
        }
    }
}
