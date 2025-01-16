using FluentValidation;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class OrderCreateReq : BaseDTO
    {
        public string OrderCode { get; set; }

        public string CustomerName { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int TotalTargetQuantity { get; set; }

        public int TotalWorkers { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderCreateDetailInfo OrderDetail { get; set; }
    }

    public class OrderCreateDetailInfo
    {
        public string ProductCode { get; set; }

        public int LineId { get; set; }

        public List<int> ClusterIds { get; set; }
    }

    public class OrderCreateReqValidator : AbstractValidator<OrderCreateReq>
    {
        public OrderCreateReqValidator()
        {
            RuleFor(x => x.OrderCode).NotNull().NotEmpty();
            RuleFor(x => x.CustomerName).NotNull().NotEmpty();
            RuleFor(x => x.TotalTargetQuantity).GreaterThan(0);

            RuleFor(x => x.BeginDate).LessThan(x => x.EndDate).When(x => x.EndDate != null);
            RuleFor(x => x.EndDate).GreaterThan(x => x.BeginDate).When(x => x.BeginDate != null);
        }
    }
}
