namespace wms.infrastructure
{
    public class AppCoreConfig
    {
        public static CommonConfig Common;
        public static ConnectionStrings Connection;
        public static URLConnectionConfig URLConnection;
        public static JWTConfig JWT;
        public static ProviderConfig Providers;
        public static Oauth2SwaggerConfig Oauth2Swagger;
        public static EmailConfig EmailConfig;
        public static TelegramNotifyConfig NotifyConfig;
        public static LoggerConfig LoggerConfig;
    }

    public class CommonConfig
    {
        public string IndexName { get; set; } = "system";
        public string ClientID { get; set; }
        public string Environment { get; set; }
        public int SystemUserID { get; set; }
        public bool DisableAuthen { get; set; }
        public bool IsUseElasticLogger { get; set; }
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }

    public class URLConnectionConfig
    {
        public string IDSUrl { get; set; }
    }

    public class JWTConfig
    {
        public string Base64PublicKey { get; set; }

        public string Issuer { get; set; }

        public string Audiences { get; set; }
    }

    public class ProviderConfig
    {
        public RabbitMQConfig RabbitMQ { get; set; }
    }

    public class RabbitMQConfig
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class Oauth2SwaggerConfig
    {
        public bool DisableSwagger { get; set; }
        public string ClientName { get; set; }
        public string ClientSecret { get; set; }
        public string Scopes { get; set; }
    }

    public class EmailConfig
    {
        public bool? IsAllowSendEmail { get; set; }
        public string Host { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }

    public class TelegramNotifyConfig
    {
        public string Host { get; set; }
        public string UserKey { get; set; }
        public string SecretKey { get; set; }
        public string Endpoint { get; set; }
        public string DefaultGroup { get; set; }
    }

    public class LoggerConfig
    {
        public string Type { get; set; }

        public string ServerURL { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string IndexName { get; set; }
    }
}
