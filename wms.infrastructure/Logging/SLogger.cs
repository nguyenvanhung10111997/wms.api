using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text;

namespace wms.infrastructure.Logging
{
    public class SLogger : LoggerBase, ILogger
    {
        private readonly Serilog.ILogger _logger;
        private readonly string _clientName;
        private readonly IHttpContextAccessor _httpContext;
        public string clientName { get { return this._clientName; } }

        public SLogger(Serilog.ILogger logger, IHttpContextAccessor context)
        {
            _logger = logger;
            _clientName = AppCoreConfig.Common.ClientID;
            _httpContext = context;
        }

        public void Trace(LogIdentify logIdentify, string message)
        {
            FilteredLog(logIdentify, LogLevel.Trace, message);
        }

        public void Debug(LogIdentify logIdentify, string message)
        {
            FilteredLog(logIdentify, LogLevel.Debug, message);
        }

        public void Info(LogIdentify logIdentify, string message)
        {
            FilteredLog(logIdentify, LogLevel.Info, message);
        }

        public void Warn(LogIdentify logIdentify, string message)
        {
            FilteredLog(logIdentify, LogLevel.Warn, message);
        }

        public void Error(LogIdentify logIdentify, string message)
        {
            FilteredLog(logIdentify, LogLevel.Error, message);
        }

        public void Fatal(LogIdentify logIdentify, string message)
        {
            FilteredLog(logIdentify, LogLevel.Fatal, message);
        }

        private async void FilteredLog(LogIdentify logIdentify, LogLevel logLevel, string message)
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(message))
                    return;

                if (logIdentify == null)
                {
                    logIdentify = new LogIdentify();
                }

                if (string.IsNullOrEmpty(logIdentify.ProcessID))
                    logIdentify.contextID = Guid.NewGuid().ToString().Replace("-", "");

                try
                {
                    string traceID = System.Diagnostics.Activity.Current?.TraceId.ToString();

                    string URI = string.Empty;
                    if (_httpContext != null && _httpContext?.HttpContext != null)
                    {
                        URI = UriHelper.GetDisplayUrl(_httpContext?.HttpContext?.Request);
                    }

                    var sb = new StringBuilder();
                    var propertyValues = new List<object>() { };

                    if (!string.IsNullOrEmpty(traceID))
                    {
                        sb.Append(" {TraceID}"); propertyValues.Add(traceID);
                    }
                    else
                    {
                        sb.Append(" {TraceID}"); propertyValues.Add(logIdentify.contextID);
                    }
                    if (string.IsNullOrEmpty(traceID) && !string.IsNullOrEmpty(logIdentify.ProcessID))
                    {
                        sb.Append(" {ProcessID}"); propertyValues.Add(logIdentify.ProcessID);
                    }
                    if (!string.IsNullOrEmpty(URI))
                    {
                        sb.Append(" {URI}"); propertyValues.Add(traceID);
                    }
                    sb.Append(" {ClientID}"); propertyValues.Add(_clientName);

                    if (!string.IsNullOrEmpty(logIdentify.SessionID))
                    {
                        sb.Append(" {SessionID}"); propertyValues.Add(logIdentify.SessionID);
                    }
                    sb.Append(" {Message}"); propertyValues.Add(message);

                    if (logLevel == LogLevel.Trace)
                        _logger.Verbose(sb.ToString(), propertyValues.ToArray());
                    else if (logLevel == LogLevel.Debug)
                        _logger.Debug(sb.ToString(), propertyValues.ToArray());
                    else if (logLevel == LogLevel.Info)
                        _logger.Information(sb.ToString(), propertyValues.ToArray());
                    else if (logLevel == LogLevel.Warn)
                        _logger.Warning(sb.ToString(), propertyValues.ToArray());
                    else if (logLevel == LogLevel.Error)
                        _logger.Error(sb.ToString(), propertyValues.ToArray());
                    else if (logLevel == LogLevel.Fatal)
                        _logger.Fatal(sb.ToString(), propertyValues.ToArray());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error:{ex.Message} inner exception {ex.InnerException?.Message}");
                }


            }).ConfigureAwait(false);
        }

        public void Info(string messageTemplate, params object[] propertyValues)
        {
            propertyValues[1] = _clientName;
            _logger.Information(messageTemplate, propertyValues);
        }

        public void Error(System.Exception ex, string messageTemplate, params object[] propertyValues)
        {
            propertyValues[1] = _clientName;
            _logger.Error(ex, messageTemplate, propertyValues);
        }

        public void Info(Exception ex, string messageTemplate, params object[] propertyValues)
        {
            propertyValues[1] = _clientName;
            _logger.Information(ex, messageTemplate, propertyValues);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            propertyValues[1] = _clientName;
            _logger.Error(messageTemplate, propertyValues);
        }

        public void Warn(string messageTemplate, params object[] propertyValues)
        {
            propertyValues[1] = _clientName;
            _logger.Warning(messageTemplate, propertyValues);
        }
        public void Email(LogSendEmail obj)
        {
            string mess = base.EmailBase(obj);
            if (!string.IsNullOrEmpty(mess))
                Error(new LogIdentify(), mess);
        }

        public void Notify(LogSendNotify obj)
        {
            string mess = base.NotifyBase(obj);
            if (!string.IsNullOrEmpty(mess))
                Error(new LogIdentify(), mess);
        }

        public void Dispose()
        {
        }
    }
}
