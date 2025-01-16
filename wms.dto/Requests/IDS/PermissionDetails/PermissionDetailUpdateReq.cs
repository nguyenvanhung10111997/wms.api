using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class PermissionDetailUpdateReq : BaseDTO
    {
        public int Id { get; set; }

        public int PermissionId { get; set; }

        public int ClientId { get; set; }

        public string APIController { get; set; }

        public string APIAction { get; set; }

        public string APIMethod { get; set; }

        public string Description { get; set; }
    }

    public class PermissionDetailUpdateReqValidator : AbstractValidator<PermissionDetailUpdateReq>
    {
        public PermissionDetailUpdateReqValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.PermissionId).GreaterThan(0);
            RuleFor(x => x.ClientId).GreaterThan(0);
            RuleFor(x => x.APIController).NotNull().NotEmpty();
            RuleFor(x => x.APIAction).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
        }
    }
}
