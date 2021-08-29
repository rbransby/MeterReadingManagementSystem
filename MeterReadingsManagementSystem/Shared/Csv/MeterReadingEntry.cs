using System;
using System.Collections.Generic;
using System.Text;

namespace MeterReadingsManagementSystem.Shared.Csv
{
    public class MeterReadingEntry
    {
        public string AccountId { get; set; }
        public string MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
    }
}
