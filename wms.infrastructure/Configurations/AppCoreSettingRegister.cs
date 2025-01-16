using Microsoft.Extensions.Configuration;

namespace wms.infrastructure.Configurations
{
    public static class AppCoreSettingRegister
    {
        public static void Binding(IConfiguration configuration)
        {
            AppCoreConfig.Common = new CommonConfig();
            configuration.Bind("CommonConfig", AppCoreConfig.Common);

            AppCoreConfig.Connection = new ConnectionStrings();
            configuration.Bind("ConnectionStrings", AppCoreConfig.Connection);

            AppCoreConfig.URLConnection = new URLConnectionConfig();
            configuration.Bind("URLConnectionConfig", AppCoreConfig.URLConnection);

            AppCoreConfig.Providers = new ProviderConfig();
            configuration.Bind("Providers", AppCoreConfig.Providers);

            AppCoreConfig.JWT = new JWTConfig();
            configuration.Bind("JWTConfig", AppCoreConfig.JWT);

            AppCoreConfig.Oauth2Swagger = new Oauth2SwaggerConfig();
            configuration.Bind("Oauth2SwaggerConfig", AppCoreConfig.Oauth2Swagger);

            AppCoreConfig.LoggerConfig = new LoggerConfig();
            configuration.Bind("LoggerConfig", AppCoreConfig.LoggerConfig);
        }
    }
}
