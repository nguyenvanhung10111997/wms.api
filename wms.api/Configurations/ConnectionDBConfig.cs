using Autofac;
using wms.business.Enums;
using wms.ids.business.Configs;
using wms.infrastructure.Configurations;

namespace wms.api.Configurations
{
    public static class ConnectionDBConfig
    {
        public static void RegisterDBConnection(this ContainerBuilder builder)
        {
            HostBuilderItem.DefaultConnectionString = ApiConfig.Connection.DefaultConnection;

            // Other DB connections
            var otherConnections = new Dictionary<string, string>();
            otherConnections.Add(ConnectionEnum.IDS.ToString(), ApiConfig.Connection.IDSConnection);

            HostBuilderItem.ConnectionStrings = otherConnections;
        }
    }
}
