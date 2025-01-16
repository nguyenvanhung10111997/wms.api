using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class RoleCreateReq : BaseDTO
    {
        public string RoleName { get; set; }

        public string Description { get; set; }
    }

    public class RoleCreateReqValidator : AbstractValidator<RoleCreateReq>
    {
        public RoleCreateReqValidator()
        {
            RuleFor(x => x.RoleName).NotNull().NotEmpty();
        }
    }
}
