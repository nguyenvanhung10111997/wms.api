using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class RolePermissionCreateReq : BaseDTO
    {
        public int RoleId { get; set; }

        public int PermissionId { get; set; }
    }

    public class RolePermissionCreateReqValidator : AbstractValidator<RolePermissionCreateReq>
    {
        public RolePermissionCreateReqValidator()
        {
            RuleFor(x => x.RoleId).GreaterThan(0);
            RuleFor(x => x.PermissionId).GreaterThan(0);
        }
    }
}
