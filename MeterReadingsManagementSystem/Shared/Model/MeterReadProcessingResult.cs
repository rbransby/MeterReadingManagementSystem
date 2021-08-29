using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeterReadingsManagementSystem.Shared.Csv;

namespace MeterReadingsManagementSystem.Shared.Model
{
    public class MeterReadProcessingResult : MeterReadingEntry
    {
        public bool WasSuccessful 
        { 
            get 
            {
                return FailReasons.Any();
            } 
        }                
        public List<MeterReadingUploadFailReason> FailReasons { get; set; } = new List<MeterReadingUploadFailReason>();

        public MeterReadProcessingResult(MeterReadingEntry entry)
        {
            this.AccountId = entry.AccountId;
            this.MeterReadingDateTime = entry.MeterReadingDateTime;
            this.MeterReadValue = entry.MeterReadValue;
        }
    }
}
