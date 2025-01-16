using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using wms.business.Services.Interfaces;
using wms.infrastructure.Attributes;
using wms.infrastructure.Configurations;
using wms.infrastructure.Models;

namespace wms.api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class FilesController : BaseController
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Upload 
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(Upload))]
        [ProducesResponseType(200, Type = typeof(FileUploadRes))]
        [ApiAuthorize(true)]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue, MultipartHeadersLengthLimit = int.MaxValue)]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var result = await _fileService.Upload(file, CurrentUser.Username);

            return Ok(result);
        }

        /// <summary>
        /// Upload 
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(Get))]
        [ProducesResponseType(200, Type = typeof(FileStreamResult))]
        [ApiAuthorize(true)]
        public async Task<IActionResult> Get(string filePath)
        {
            var fileByes = await _fileService.Get(filePath);

            var fileName = Path.GetFileName(filePath);
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);

            var stream = new MemoryStream(fileByes.Data);

            return new FileStreamResult(stream, contentType)
            {
                FileDownloadName = fileName
            };
        }
    }
}
