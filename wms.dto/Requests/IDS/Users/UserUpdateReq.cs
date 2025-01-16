using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class UserUpdateReq : BaseDTO
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Avatar { get; set; }

        /// <summary>
        /// 1: Male | 2: Female
        /// </summary>
        public int Gender { get; set; }

        public bool IsActive { get; set; }
    }

    public class UserUpdateReqValidator : AbstractValidator<UserUpdateReq>
    {
        public UserUpdateReqValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.FullName).NotNull().NotEmpty();
            RuleFor(x => x.Gender).GreaterThan(0);
        }
    }
}
