using System.Security.Claims;

namespace wms.infrastructure.Models
{
    public class UserPrincipalLog
    {
        public int UserID { get; set; }
        public string Username { get; set; }

        public UserPrincipalLog(IEnumerable<Claim> claims)
        {
            if (claims.Where(p => p.Type == "/UserID").Count() > 0)
                UserID = int.Parse(claims.SingleOrDefault(p => p.Type == "/UserID").Value);
            if (claims.Where(p => p.Type == "/Username").Count() > 0)
                Username = claims.SingleOrDefault(p => p.Type == "/Username").Value;
        }
    }
}
