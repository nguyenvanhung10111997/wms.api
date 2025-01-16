namespace wms.ids.business.Configs
{
    public class ApiConfig
    {
        public static CommonConfig Common;
        public static ConnectionStrings Connection;
    }

    public class CommonConfig
    {
        public string ClientID { get; set; }
        public string Environment { get; set; }
        public int SystemUserID { get; set; }
        public string DefaultPassword { get; set; }
        public int MaxExcelRecord { get; set; } = 1000;
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
        public string IDSConnection { get; set; }
    }
}
