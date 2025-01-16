using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wms.dto.Common;

namespace wms.dto.Requests
{
    public class LineMetricStatisticReq : BaseDTO
    {
        public DateTime? RequestTime { get; set; }
    }
}
