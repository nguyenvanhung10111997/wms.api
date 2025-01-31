﻿using System.Security.Claims;

namespace wms.infrastructure.Models
{
    public class UserPrincipal : ClaimsPrincipal
    {
        public int UserID { get; set; }

        public string Username { get; set; }

        public int? UserTypeID { get; set; }

        public string Fullname { get; set; }

        public IEnumerable<int> RoleIDs { get; set; }

        public UserPrincipal()
        {
        }

        public UserPrincipal(List<Claim> claims)
        {
            UserID = (claims.Any((Claim x) => x.Type == "/UserID") ? int.Parse(claims.FirstOrDefault((Claim p) => p.Type == "/UserID")!.Value) : 0);
            Username = (claims.Any((Claim x) => x.Type == "/Username") ? claims.FirstOrDefault((Claim p) => p.Type == "/Username")!.Value : string.Empty);
            Fullname = (claims.Any((Claim x) => x.Type == "/Fullname") ? claims.FirstOrDefault((Claim p) => p.Type == "/Fullname")!.Value : string.Empty);
            UserTypeID = (claims.Any((Claim x) => x.Type == "/UserTypeID") ? int.Parse(claims.FirstOrDefault((Claim p) => p.Type == "/UserTypeID")!.Value) : 0);

            var claim = (claims.Any((Claim x) => x.Type == "/RoleIDs") ? claims.FirstOrDefault((Claim p) => p.Type == "/RoleIDs") : null);

            if (claim != null)
            {
                RoleIDs = from i in claim.Value.Replace("[", "").Replace("]", "").Split(',')
                          select Convert.ToInt32(i);
            }
        }

        public bool IsPermission(string permissionName)
        {
            if (string.IsNullOrWhiteSpace(permissionName) || RoleIDs == null || !RoleIDs.Any())
            {
                return false;
            }

            return true;
            //return AppPermission.Data.Where((Permission r) => RoleIDs.Contains(r.RoleID) && r.RoleFunctionName == roleFunctionName).Any();
        }
    }
}
