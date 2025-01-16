using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wms.dto.Responses;
using wms.infrastructure.Models;

namespace wms.business.Services.Interfaces
{
    public interface IClientService
    {
        Task<CRUDResult<IEnumerable<ClientRes>>> ReadAll();
    }
}
