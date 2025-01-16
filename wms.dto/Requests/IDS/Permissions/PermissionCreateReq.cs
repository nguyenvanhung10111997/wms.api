using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class PermissionCreateReq : BaseDTO
    {
        public string PermissionName { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
    }

    public class PermissionCreateReqValidator : AbstractValidator<PermissionCreateReq>
    {
        public PermissionCreateReqValidator()
        {
            RuleFor(x => x.PermissionName).NotNull().NotEmpty();
        }
    }
}
