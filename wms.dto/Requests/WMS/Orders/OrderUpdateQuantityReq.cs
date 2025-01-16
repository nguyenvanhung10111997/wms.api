using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class OrderUpdateQuantityReq : BaseDTO
    {
        public int OrderId { get; set; }

        public int TotalTargetQuantity { get; set; }

        public List<OrderUpdateDetailQuantityReq> Details { get; set; }
    }

    public class OrderUpdateDetailQuantityReq : BaseDTO
    {
        public int StaticShiftId { get; set; }

        public int TargetQuantity { get; set; }
    }

    public class OrderUpdateQuantityReqValidator : AbstractValidator<OrderUpdateQuantityReq>
    {
        public OrderUpdateQuantityReqValidator()
        {
            RuleFor(x => x.OrderId).GreaterThan(0);
            RuleFor(x => x.TotalTargetQuantity).GreaterThan(0);
        }
    }
}
