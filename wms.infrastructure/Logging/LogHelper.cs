using Elasticsearch.Net;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Templates;

namespace wms.infrastructure.Logging
{
    public static class LogHelper
    {
        public static void WriteSystemLog(string messageError)
        {
            var logger = CreateInstanceLogger("systemerror");

            logger.Error(new LogIdentify()
            {
                ProcessID = Guid.NewGuid().ToString()
            }, messageError);
        }

        public static ILogger CreateInstanceLogger(string indexname)
        {
            if (AppCoreConfig.Common.IsUseElasticLogger)
            {
                return CreateInstanceElasticLogger(indexname);
            }

            return CreateInstanceConsoleLogger(indexname);
        }

        private static ILogger CreateInstanceConsoleLogger(string indexName)
        {
            var expressionTemplate = new ExpressionTemplate("{ {@t, @mt, @l: if @l = 'Information' then undefined() else @l, @x, ..@p} }\n");

            return new SLogger(new LoggerConfiguration().WriteTo.Console(expressionTemplate).CreateLogger(), Engine.ContainerManager.Resolve<IHttpContextAccessor>());
        }

        private static ILogger CreateInstanceFileLogger(string indexName)
        {
            var currentDate = DateTime.Now.ToString("yyyyMMdd");
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", currentDate);
            var filePath = Path.Combine(folderPath, $"CurrentLog.txt");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var expressionTemplate = new ExpressionTemplate("{ {@t, @mt, @l: if @l = 'Information' then undefined() else @l, @x, ..@p} }\n");

            return new SLogger(new LoggerConfiguration().WriteTo.File(expressionTemplate, filePath).CreateLogger(), Engine.ContainerManager.Resolve<IHttpContextAccessor>());
        }

        private static ILogger CreateInstanceElasticLogger(string indexName)
        {
            var elasticOptions = new ElasticsearchSinkOptions(new Uri(AppCoreConfig.LoggerConfig.ServerURL))
            {
                ModifyConnectionSettings = x => x.BasicAuthentication(AppCoreConfig.LoggerConfig.Username, AppCoreConfig.LoggerConfig.Password).ServerCertificateValidationCallback(CertificateValidations.AllowAll),
                IndexFormat = AppCoreConfig.Common.Environment.ToLower() + "." + indexName + "-{0:yyyy.MM}",
                CustomFormatter = new KibanaCustomFormatter(),
                TypeName = "_doc",
                NumberOfReplicas = 0,
                EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
            };

            return new SLogger(new LoggerConfiguration().WriteTo.Elasticsearch(elasticOptions).CreateLogger(), Engine.ContainerManager.Resolve<IHttpContextAccessor>());
        }
    }
}
