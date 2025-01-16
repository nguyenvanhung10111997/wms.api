using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class OrderUpdateStatusReq : BaseDTO
    {
        public int OrderId { get; set; }

        public int StatusId { get; set; }
    }

    public class OrderUpdateStatusReqValidator : AbstractValidator<OrderUpdateStatusReq>
    {
        public OrderUpdateStatusReqValidator()
        {
            RuleFor(x => x.OrderId).GreaterThan(0);
            RuleFor(x => x.StatusId).GreaterThan(0);
        }
    }
}
