using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class UserChangePasswordReq : BaseDTO
    {
        public int UserId { get; set; }

        /// <summary>
        /// MD5 Hash
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// MD5 Hash
        /// </summary>
        public string NewPassword { get; set; }
    }

    public class UserChangePasswordReqValidator : AbstractValidator<UserChangePasswordReq>
    {
        public UserChangePasswordReqValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.OldPassword).NotNull().NotEmpty();
            RuleFor(x => x.NewPassword).NotNull().NotEmpty();
        }
    }
}
