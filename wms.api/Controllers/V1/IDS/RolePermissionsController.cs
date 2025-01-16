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
    public class RolePermissionsController : BaseController
    {
        private readonly IRolePermissionService _rolePermissionService;

        public RolePermissionsController(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }

        /// <summary>
        /// ReadByPermissionID
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        [HttpGet("Permission/{permissionId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RolePermissionRes>))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadByPermissionID(int permissionId)
        {
            var result = await _rolePermissionService.ReadByPermissionID(permissionId);
            return ApiOK(result);
        }

        /// <summary>
        /// ReadByRoleID
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("Role/{roleId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<RolePermissionRes>))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadByRoleID(int roleId)
        {
            var result = await _rolePermissionService.ReadByRoleID(roleId);
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
        public async Task<IActionResult> Create(RolePermissionCreateReq obj)
        {
            var result = await _rolePermissionService.Create(obj, CurrentUser.UserID);
            return ApiOK(result);
        }

        /// <summary>
        /// BulkCreate
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("Bulk/Create")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ApiAuthorize]
        public async Task<IActionResult> BulkCreate(RolePermissionBulkCreateReq obj)
        {
            var result = await _rolePermissionService.BulkCreate(obj, CurrentUser.UserID);
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
            var result = await _rolePermissionService.Delete(id, CurrentUser.UserID);
            return ApiOK(result);
        }
    }
}
