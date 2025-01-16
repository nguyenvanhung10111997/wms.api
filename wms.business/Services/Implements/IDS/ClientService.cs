using Autofac.Features.Indexed;
using wms.business.Enums;
using wms.business.Services.Interfaces;
using wms.dto.Responses;
using wms.infrastructure;
using wms.infrastructure.Enums;
using wms.infrastructure.Models;

namespace wms.business.Services.Implements
{
    internal class ClientService : BaseService, IClientService
    {
        private readonly Lazy<IReadOnlyRepository> _readOnlyRepository;

        public ClientService(IIndex<string, Lazy<IReadOnlyRepository>> readOnlyRepository)
        {
            _readOnlyRepository = readOnlyRepository[ConnectionEnum.IDS.ToString()];
        }

        public async Task<CRUDResult<IEnumerable<ClientRes>>> ReadAll()
        {
            var result = await _readOnlyRepository.Value.StoreProcedureQueryAsync<ClientRes>("dbo.Client_ReadAll");

            if (result == null || !result.Any())
            {
                return Error<IEnumerable<ClientRes>>(statusCode: CRUDStatusCodeRes.ResourceNotFound);
            }

            return Success(result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_readOnlyRepository != null && _readOnlyRepository.IsValueCreated)
                {
                    _readOnlyRepository.Value.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
