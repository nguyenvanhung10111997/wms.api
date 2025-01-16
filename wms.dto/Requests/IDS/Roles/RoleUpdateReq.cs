using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class RoleUpdateReq : BaseDTO
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }
    }

    public class RoleUpdateReqValidator : AbstractValidator<RoleUpdateReq>
    {
        public RoleUpdateReqValidator()
        {
            RuleFor(x => x.RoleId).GreaterThan(0);
            RuleFor(x => x.RoleName).NotNull().NotEmpty();
        }
    }
}
