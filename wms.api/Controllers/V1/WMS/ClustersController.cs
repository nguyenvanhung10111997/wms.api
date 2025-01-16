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
    public class ClustersController : BaseController
    {
        private readonly IClusterService _clusterService;

        public ClustersController(IClusterService ClusterService)
        {
            _clusterService = ClusterService;
        }

        /// <summary>
        /// ReadAll
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(ClusterRes))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadAll()
        {
            var result = await _clusterService.ReadAll();
            return ApiOK(result);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost(nameof(Create))]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ApiAuthorize]
        public async Task<IActionResult> Create(ClusterCreateReq obj)
        {
            var result = await _clusterService.Create(obj, CurrentUser.UserID);
            return ApiOK(result);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut(nameof(Update))]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ApiAuthorize]
        public async Task<IActionResult> Update(ClusterUpdateReq obj)
        {
            var result = await _clusterService.Update(obj, CurrentUser.UserID);
            return ApiOK(result);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ApiAuthorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _clusterService.Delete(id, CurrentUser.UserID);
            return ApiOK(result);
        }
    }
}
