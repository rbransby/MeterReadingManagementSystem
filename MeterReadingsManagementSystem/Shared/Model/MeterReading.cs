namespace MeterReadingsManagementSystem.Shared.Model
{
    public class MeterReading
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public int MeterReadValue { get; set; }
    }
}
