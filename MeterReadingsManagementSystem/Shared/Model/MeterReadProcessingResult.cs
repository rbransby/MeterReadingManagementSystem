using MeterReadingsManagementSystem.Shared.Csv;

namespace MeterReadingsManagementSystem.Shared.Model
{
    public class MeterReadProcessingResult : MeterReadingEntry
    {
        public bool WasSuccessful 
        { 
            get 
            {
                return !FailReasons.Any();
            } 
        }                
        public List<MeterReadingProcessFailReason> FailReasons { get; set; } = new List<MeterReadingProcessFailReason>();

        public MeterReadProcessingResult(MeterReadingEntry entry)
        {
            this.AccountId = entry.AccountId;
            this.MeterReadingDateTime = entry.MeterReadingDateTime;
            this.MeterReadValue = entry.MeterReadValue;
        }
    }
}
