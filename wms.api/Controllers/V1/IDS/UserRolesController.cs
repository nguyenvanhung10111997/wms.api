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
    public class UserRolesController : BaseController
    {
        private readonly IUserRoleService _userRoleService;

        public UserRolesController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        /// <summary>
        /// ReadByUserID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("User/{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserRoleRes>))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadByUserID(int userId)
        {
            var result = await _userRoleService.ReadByUserID(userId);
            return ApiOK(result);
        }

        /// <summary>
        /// ReadByRoleID
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("Role/{roleId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserRoleRes>))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadByRoleID(int roleId)
        {
            var result = await _userRoleService.ReadByRoleID(roleId);
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
        public async Task<IActionResult> Create(UserRoleCreateReq obj)
        {
            var result = await _userRoleService.Create(obj, CurrentUser.UserID);
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
            var result = await _userRoleService.Delete(id, CurrentUser.UserID);
            return ApiOK(result);
        }
    }
}
