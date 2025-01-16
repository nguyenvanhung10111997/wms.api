using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class UserLockReq : BaseDTO
    {
        public int UserId { get; set; }
    }

    public class UserLockReqValidator : AbstractValidator<UserLockReq>
    {
        public UserLockReqValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
        }
    }
}
