using Microsoft.AspNetCore.Http;
using wms.business.Services.Interfaces;
using wms.infrastructure.Enums;
using wms.infrastructure.Helpers;
using wms.infrastructure.Models;

namespace wms.business.Services.Implements
{
    internal class FileService : BaseService, IFileService
    {
        public FileService()
        {
        }

        public async Task<CRUDResult<FileUploadRes>> Upload(IFormFile file, string username)
        {
            try
            {
                if (file == null)
                {
                    return Error<FileUploadRes>(statusCode: CRUDStatusCodeRes.InvalidData, errorMessage: "Vui lòng chọn file");
                }

                var fileName = file.FileName;

                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    var uploadResult = await FileHelper.Upload(fileBytes, fileName);

                    return Success(uploadResult);
                }
            }
            catch (Exception ex)
            {
                return new CRUDResult<FileUploadRes> { StatusCode = CRUDStatusCodeRes.InvalidData, ErrorMessage = ex.Message };
            }
        }

        public async Task<CRUDResult<byte[]>> Get(string filePath)
        {
            try
            {
                var result = await FileHelper.GetFile(filePath);

                return Success(result);
            }
            catch(Exception ex)
            {
                return new CRUDResult<byte[]> { StatusCode = CRUDStatusCodeRes.InvalidData, ErrorMessage = ex.Message };
            }
        }
    }
}
