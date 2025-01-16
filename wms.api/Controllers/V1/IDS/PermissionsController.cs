using Microsoft.AspNetCore.Mvc;
using wms.business.Services.Interfaces;
using wms.dto.Requests;
using wms.dto.Responses;
using wms.infrastructure.Attributes;
using wms.infrastructure.Configurations;
using wms.infrastructure.Models;

namespace wms.api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PermissionsController : BaseController
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost(nameof(Search))]
        [ProducesResponseType(200, Type = typeof(PagingResponse<PermissionSearchRes>))]
        [ApiAuthorize]
        public async Task<IActionResult> Search(PermissionSearchReq obj)
        {
            var result = await _permissionService.Search(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// ReadByID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PermissionRes))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadByID(int id)
        {
            var result = await _permissionService.ReadByID(id);
            return ApiOK(result);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost(nameof(Create))]
        [ProducesResponseType(200, Type = typeof(PermissionCreateRes))]
        [ApiAuthorize]
        public async Task<IActionResult> Create(PermissionCreateReq obj)
        {
            var result = await _permissionService.Create(obj, CurrentUser.UserID);
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
        public async Task<IActionResult> Update(PermissionUpdateReq obj)
        {
            var result = await _permissionService.Update(obj, CurrentUser.UserID);
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
            var result = await _permissionService.Delete(id, CurrentUser.UserID);
            return ApiOK(result);
        }

        /// <summary>
        /// ReadByUserID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("User/{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PermissionReadByUserRes>))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadByUserID(int userId)
        {
            var result = await _permissionService.ReadByUserID(userId);
            return ApiOK(result);
        }
    }
}
