namespace wms.infrastructure.Logging
{
    public class LogSendEmail
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailTo { get; set; }
        public List<string> EmailCC { get; set; }
    }
}
