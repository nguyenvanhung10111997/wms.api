namespace wms.infrastructure.Logging
{
    public interface ILogger
    {
        string clientName { get; }

        void Trace(LogIdentify logIdentify, string message);

        void Debug(LogIdentify logIdentify, string message);

        void Info(LogIdentify logIdentify, string message);

        void Warn(LogIdentify logIdentify, string message);

        void Error(LogIdentify logIdentify, string message);

        void Fatal(LogIdentify logIdentify, string message);

        void Info(Exception ex, string messageTemplate, params object[] propertyValues);

        void Error(Exception ex, string messageTemplate, params object[] propertyValues);

        void Info(string messageTemplate, params object[] propertyValues);

        void Error(string messageTemplate, params object[] propertyValues);

        void Warn(string messageTemplate, params object[] propertyValues);

        void Email(LogSendEmail obj);

        void Notify(LogSendNotify obj);
    }
}
