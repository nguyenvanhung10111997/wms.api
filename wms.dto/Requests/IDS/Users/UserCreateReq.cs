using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class UserCreateReq : BaseDTO
    {
        public string Username { get; set; }

        public string FullName { get; set; }

        public string Avatar { get; set; }

        /// <summary>
        /// 1: Male | 2: Female
        /// </summary>
        public int Gender { get; set; }

        public bool IsActive { get; set; }
    }

    public class UserCreateReqValidator : AbstractValidator<UserCreateReq>
    {
        public UserCreateReqValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty();
            RuleFor(x => x.FullName).NotNull().NotEmpty();
            RuleFor(x => x.Gender).GreaterThan(0);
        }
    }
}
