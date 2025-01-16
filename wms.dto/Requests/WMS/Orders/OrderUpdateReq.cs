using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class OrderUpdateReq : BaseDTO
    {
        public int OrderId { get; set; }

        public string OrderCode { get; set; }

        public int TotalWorkers { get; set; }
    }

    public class OrderUpdateReqValidator : AbstractValidator<OrderUpdateReq>
    {
        public OrderUpdateReqValidator()
        {
            RuleFor(x => x.OrderId).GreaterThan(0);
            RuleFor(x => x.OrderCode).NotNull().NotEmpty();
            RuleFor(x => x.TotalWorkers).GreaterThan(0);
        }
    }
}
