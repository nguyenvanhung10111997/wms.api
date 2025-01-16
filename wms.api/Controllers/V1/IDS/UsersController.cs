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
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost(nameof(Search))]
        [ProducesResponseType(200, Type = typeof(PagingResponse<UserSearchRes>))]
        [ApiAuthorize]
        public async Task<IActionResult> Search(UserSearchReq obj)
        {
            var result = await _userService.Search(obj);
            return ApiOK(result);
        }

        /// <summary>
        /// ReadByID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(UserRes))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadByID(int id)
        {
            var result = await _userService.ReadByID(id);
            return ApiOK(result);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost(nameof(Create))]
        [ProducesResponseType(200, Type = typeof(UserCreateRes))]
        [ApiAuthorize]
        public async Task<IActionResult> Create(UserCreateReq obj)
        {
            var result = await _userService.Create(obj, CurrentUser.UserID);
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
        public async Task<IActionResult> Update(UserUpdateReq obj)
        {
            var result = await _userService.Update(obj, CurrentUser.UserID);
            return ApiOK(result);
        }

        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut(nameof(ChangePassword))]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ApiAuthorize]
        public async Task<IActionResult> ChangePassword(UserChangePasswordReq obj)
        {
            var result = await _userService.ChangePassword(obj, CurrentUser.UserID);
            return ApiOK(result);
        }

        /// <summary>
        /// Lock
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut(nameof(Lock))]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ApiAuthorize]
        public async Task<IActionResult> Lock(UserLockReq obj)
        {
            var result = await _userService.Lock(obj, CurrentUser.UserID);
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
            var result = await _userService.Delete(id, CurrentUser.UserID);
            return ApiOK(result);
        }
    }
}
