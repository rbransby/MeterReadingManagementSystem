using MeterReadingsManagementSystem.Server.Data;
using MeterReadingsManagementSystem.Server.Services;
using MeterReadingsManagementSystem.Shared;
using MeterReadingsManagementSystem.Shared.Csv;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MeterReadingsManagementSystem.Server.Tests
{
    public class MeterReadProcessingServiceFixture : IDisposable
    {
        private DataContext _dataContext; 
        public MeterReadProcessingServiceFixture()
        {
            var contextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("Test").Options;
            _dataContext = new DataContext(contextOptions);
            _dataContext.Accounts.AddRange(TestData.GetTestAccounts());
            _dataContext.SaveChanges();
        }

        public void Dispose()
        {
            _dataContext.MeterReadings.RemoveRange(_dataContext.MeterReadings);
            _dataContext.Accounts.RemoveRange(_dataContext.Accounts);
            _dataContext.SaveChanges();
        }

        [Fact]
        public void ProcessMeterReadEntries_OneValidEntryToProcess_ProcessedSuccessfully()
        {
            MeterReadProcessingService service = new(_dataContext);
            var testEntries = new List<MeterReadingEntry>()
            {
                new MeterReadingEntry()
                {
                    AccountId = "2351",
                    MeterReadingDateTime = "22/04/2019 12:25",
                    MeterReadValue = "1002"
                }
            };

            var result = service.ProcessMeterReadEntries(testEntries);

            Assert.Single(result);
            Assert.NotNull(result.First());
            Assert.True(result.First().WasSuccessful);
            Assert.Single(_dataContext.MeterReadings);
        }

        [Fact]
        public void ProcessMeterReadEntries_AttemptToInsertDupEntry_SecondIsRejected()
        {
            MeterReadProcessingService service = new(_dataContext);
            var testEntries = new List<MeterReadingEntry>()
            {
                new MeterReadingEntry()
                {
                    AccountId = "2351",
                    MeterReadingDateTime = "22/04/2019 12:25",
                    MeterReadValue = "1002"
                },
                new MeterReadingEntry()
                {
                    AccountId = "2351",
                    MeterReadingDateTime = "22/04/2019 12:25",
                    MeterReadValue = "1050"
                }
            };

            var result = service.ProcessMeterReadEntries(testEntries);

            var successfulEntry = result.ElementAt(0);
            var unsuccessfulEntry = result.ElementAt(1);
            Assert.Equal(2, result.Count());
            Assert.NotNull(successfulEntry);
            Assert.NotNull(unsuccessfulEntry);
            Assert.True(successfulEntry.WasSuccessful);
            Assert.False(unsuccessfulEntry.WasSuccessful);
            Assert.Equal(MeterReadingProcessFailReason.DuplicateMeterReadEntry, unsuccessfulEntry.FailReasons[0]);
            Assert.Single(_dataContext.MeterReadings);
        }

        [Fact]
        public void ProcessMeterReadEntries_AttemptToInsertSameAccountDifferentTimeEntry_BothEntriesAccepted()
        {
            MeterReadProcessingService service = new(_dataContext);
            var testEntries = new List<MeterReadingEntry>()
            {
                new MeterReadingEntry()
                {
                    AccountId = "2351",
                    MeterReadingDateTime = "22/04/2019 12:25",
                    MeterReadValue = "1002"
                },
                new MeterReadingEntry()
                {
                    AccountId = "2351",
                    MeterReadingDateTime = "25/04/2019 13:25",
                    MeterReadValue = "1050"
                }
            };

            var result = service.ProcessMeterReadEntries(testEntries);

            var successfulEntry = result.ElementAt(0);
            var secondSuccussfulEntry = result.ElementAt(1);
            Assert.Equal(2, result.Count());
            Assert.NotNull(successfulEntry);
            Assert.NotNull(secondSuccussfulEntry);
            Assert.True(successfulEntry.WasSuccessful);
            Assert.True(secondSuccussfulEntry.WasSuccessful);            
            Assert.Equal(2, _dataContext.MeterReadings.Count());
        }

        [Fact]
        public void ProcessMeterReadEntries_NonNumericValue_EntryRejected()
        {
            MeterReadProcessingService service = new(_dataContext);
            var testEntries = new List<MeterReadingEntry>()
            {
                new MeterReadingEntry()
                {
                    AccountId = "2351",
                    MeterReadingDateTime = "22/04/2019 12:25",
                    MeterReadValue = "0X765"
                }
            };

            var result = service.ProcessMeterReadEntries(testEntries);

            var unsuccessfulEntry = result.ElementAt(0);
            
            Assert.Single(result);            
            Assert.NotNull(unsuccessfulEntry);            
            Assert.False(unsuccessfulEntry.WasSuccessful);
            Assert.Equal(MeterReadingProcessFailReason.InvalidReadValue, unsuccessfulEntry.FailReasons[0]);
            Assert.Empty(_dataContext.MeterReadings);
        }

        [Fact]
        public void ProcessMeterReadEntries_BadlyFormattedDate_EntryRejected()
        {
            MeterReadProcessingService service = new(_dataContext);
            var testEntries = new List<MeterReadingEntry>()
            {
                new MeterReadingEntry()
                {
                    AccountId = "2351",
                    MeterReadingDateTime = "45/04/2019 12:25",
                    MeterReadValue = "1050"
                }
            };

            var result = service.ProcessMeterReadEntries(testEntries);

            var unsuccessfulEntry = result.ElementAt(0);

            Assert.Single(result);
            Assert.NotNull(unsuccessfulEntry);
            Assert.False(unsuccessfulEntry.WasSuccessful);
            Assert.Equal(MeterReadingProcessFailReason.InvalidReadingDate, unsuccessfulEntry.FailReasons[0]);
            Assert.Empty(_dataContext.MeterReadings);
        }

        [Fact]
        public void ProcessMeterReadEntries_AccountDoesntExist_EntryRejected()
        {
            MeterReadProcessingService service = new(_dataContext);
            var testEntries = new List<MeterReadingEntry>()
            {
                new MeterReadingEntry()
                {
                    AccountId = "9999",
                    MeterReadingDateTime = "22/04/2019 12:25",
                    MeterReadValue = "1050"
                }
            };

            var result = service.ProcessMeterReadEntries(testEntries);

            var unsuccessfulEntry = result.ElementAt(0);

            Assert.Single(result);
            Assert.NotNull(unsuccessfulEntry);
            Assert.False(unsuccessfulEntry.WasSuccessful);
            Assert.Equal(MeterReadingProcessFailReason.InvalidAccountId, unsuccessfulEntry.FailReasons[0]);
            Assert.Empty(_dataContext.MeterReadings);
        }

        [Fact]
        public void ProcessMeterReadEntries_NegativeMeterValue_EntryRejected()
        {
            MeterReadProcessingService service = new(_dataContext);
            var testEntries = new List<MeterReadingEntry>()
            {
                new MeterReadingEntry()
                {
                    AccountId = "2351",
                    MeterReadingDateTime = "22/04/2019 12:25",
                    MeterReadValue = "-6575"
                }
            };

            var result = service.ProcessMeterReadEntries(testEntries);

            var unsuccessfulEntry = result.ElementAt(0);

            Assert.Single(result);
            Assert.NotNull(unsuccessfulEntry);
            Assert.False(unsuccessfulEntry.WasSuccessful);
            Assert.Equal(MeterReadingProcessFailReason.InvalidReadValue, unsuccessfulEntry.FailReasons[0]);
            Assert.Empty(_dataContext.MeterReadings);
        }

        [Fact]
        public void ProcessMeterReadEntries_MeterValueTooBig_EntryRejected()
        {
            MeterReadProcessingService service = new(_dataContext);
            var testEntries = new List<MeterReadingEntry>()
            {
                new MeterReadingEntry()
                {
                    AccountId = "2351",
                    MeterReadingDateTime = "22/04/2019 12:25",
                    MeterReadValue = "999999"
                }
            };

            var result = service.ProcessMeterReadEntries(testEntries);

            var unsuccessfulEntry = result.ElementAt(0);

            Assert.Single(result);
            Assert.NotNull(unsuccessfulEntry);
            Assert.False(unsuccessfulEntry.WasSuccessful);
            Assert.Equal(MeterReadingProcessFailReason.InvalidReadValue, unsuccessfulEntry.FailReasons[0]);
            Assert.Empty(_dataContext.MeterReadings);
        }
    }
}