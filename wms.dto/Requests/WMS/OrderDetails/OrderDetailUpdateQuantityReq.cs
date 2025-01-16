using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class OrderDetailUpdateQuantityReq : BaseDTO
    {
        public int Id { get; set; }

        public int TargetQuantity { get; set; }
    }

    public class OrderDetailUpdateQuantityReqValidator : AbstractValidator<OrderDetailUpdateQuantityReq>
    {
        public OrderDetailUpdateQuantityReqValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.TargetQuantity).GreaterThan(0);
        }
    }
}
