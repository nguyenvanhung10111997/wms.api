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
    public class PermissionDetailsController : BaseController
    {
        private readonly IPermissionDetailService _permissionDetailService;

        public PermissionDetailsController(IPermissionDetailService permissionDetailService)
        {
            _permissionDetailService = permissionDetailService;
        }

        /// <summary>
        /// ReadByPermissionID
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        [HttpGet("Permission/{permissionId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PermissionDetailRes>))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadByPermissionID(int permissionId)
        {
            var result = await _permissionDetailService.ReadByPermissionID(permissionId);
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
        public async Task<IActionResult> Create(PermissionDetailCreateReq obj)
        {
            var result = await _permissionDetailService.Create(obj, CurrentUser.UserID);
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
        public async Task<IActionResult> Update(PermissionDetailUpdateReq obj)
        {
            var result = await _permissionDetailService.Update(obj, CurrentUser.UserID);
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
            var result = await _permissionDetailService.Delete(id, CurrentUser.UserID);
            return ApiOK(result);
        }
    }
}
