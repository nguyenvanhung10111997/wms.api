using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class OrderSearchMetricReq : BaseDTO
    {
        public List<int>? LineIds { get; set; }

        public DateTime? RequestDate { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }

    public class OrderSearchMetricReqValidator : AbstractValidator<OrderSearchMetricReq>
    {
        public OrderSearchMetricReqValidator()
        {
            RuleFor(x => x.PageSize).NotNull().GreaterThanOrEqualTo(-1).LessThanOrEqualTo(200);
            RuleFor(x => x.PageSize).GreaterThan(0).When(c => c.PageIndex >= 0);
            RuleFor(x => x.PageIndex).NotNull().GreaterThanOrEqualTo(-1);
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0).When(c => c.PageSize > 0);
        }
    }
}
