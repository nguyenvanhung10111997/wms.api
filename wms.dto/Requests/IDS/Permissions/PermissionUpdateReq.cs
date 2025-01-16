using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class PermissionUpdateReq : BaseDTO
    {
        public int PermissionId { get; set; }

        public string PermissionName { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
    }

    public class PermissionUpdateReqValidator : AbstractValidator<PermissionUpdateReq>
    {
        public PermissionUpdateReqValidator()
        {
            RuleFor(x => x.PermissionId).GreaterThan(0);
            RuleFor(x => x.PermissionName).NotNull().NotEmpty();
        }
    }
}
