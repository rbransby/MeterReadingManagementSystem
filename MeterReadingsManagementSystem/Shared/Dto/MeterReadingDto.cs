using System;
using System.Collections.Generic;
using System.Text;

namespace MeterReadingsManagementSystem.Shared.Dto
{
    public class MeterReadingDto
    {
        public string AccountId { get; set; }
        public string MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }
}
