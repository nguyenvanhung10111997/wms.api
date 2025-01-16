using Microsoft.AspNetCore.Mvc;
using wms.business.Services.Interfaces;
using wms.dto.Responses;
using wms.infrastructure.Attributes;
using wms.infrastructure.Configurations;

namespace wms.api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ClientsController : BaseController
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        /// ReadAll
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ClientRes>))]
        [ApiAuthorize]
        public async Task<IActionResult> ReadAll()
        {
            var result = await _clientService.ReadAll();
            return ApiOK(result);
        }
    }
}
