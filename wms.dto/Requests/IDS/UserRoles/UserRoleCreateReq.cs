using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class UserRoleCreateReq : BaseDTO
    {
        public int UserId { get; set; }

        public List<int> RoleIds { get; set; }
    }

    public class UserRoleCreateReqValidator : AbstractValidator<UserRoleCreateReq>
    {
        public UserRoleCreateReqValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.RoleIds).NotNull().NotEmpty();
        }
    }
}
