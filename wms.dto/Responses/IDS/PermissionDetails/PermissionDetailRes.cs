namespace wms.dto.Responses
{
    public class PermissionDetailRes
    {
        public int Id { get; set; }

        public int PermissionId { get; set; }

        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public string APIController { get; set; }

        public string APIAction { get; set; }

        public string APIMethod { get; set; }

        public string Description { get; set; }
    }
}
