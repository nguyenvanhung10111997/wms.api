using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wms.dto.Responses;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IFileService : IDisposable
    {
        Task<CRUDResult<FileUploadRes>> Upload(IFormFile file, string username);
        Task<CRUDResult<byte[]>> Get(string filePath);
    }
}
