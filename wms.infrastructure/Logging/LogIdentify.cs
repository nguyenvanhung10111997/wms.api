namespace wms.infrastructure.Logging
{
    public class LogIdentify
    {
        public string ProcessID { get; set; }

        public string UserName { get; set; }

        public string SessionID { get; set; }

        public string contextID { get; set; }

        public LogIdentify()
        {
            ProcessID = Guid.NewGuid().ToString();
        }

        public LogIdentify(Guid GUID)
        {
            ProcessID = GUID.ToString().Replace("-", "");
        }

        public LogIdentify(string userName)
        {
            ProcessID = Guid.NewGuid().ToString();
            if (!string.IsNullOrWhiteSpace(userName))
            {
                UserName = userName.Trim();
            }
        }
    }
}
