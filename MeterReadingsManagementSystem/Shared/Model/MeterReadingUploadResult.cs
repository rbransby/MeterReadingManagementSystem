namespace MeterReadingsManagementSystem.Shared.Model
{
    public class MeterReadingUploadResult
    {
        public int SuccessfulReadings 
        { 
            get
            {
                return ReadProcessingResults.Count(r => r.WasSuccessful);
            } 
        }
        public int FailedReadings 
        {
            get
            {
                return ReadProcessingResults.Count(r => !r.WasSuccessful);
            }
        }

        public IEnumerable<MeterReadProcessingResult> ReadProcessingResults { get; set; } = new List<MeterReadProcessingResult>();
    }
}
