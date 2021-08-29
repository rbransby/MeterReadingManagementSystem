using MeterReadingsManagementSystem.Server.Data;
using MeterReadingsManagementSystem.Shared;
using MeterReadingsManagementSystem.Shared.Csv;
using MeterReadingsManagementSystem.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeterReadingsManagementSystem.Server.Services
{
    public class MeterReadProcessingService
    {
        private DataContext _dataContext;

        public MeterReadProcessingService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<MeterReadProcessingResult> ProcessMeterReadEntries(IEnumerable<MeterReadingEntry> entries)
        {
            var results = new List<MeterReadProcessingResult>();
            foreach (var entry in entries)
            {
                var meterReading = new MeterReading();
                var result = new MeterReadProcessingResult(entry);
                // first lets try to parse everything to see if we're dealing with valid data
                if (long.TryParse(entry.AccountId, out long accountId))
                {
                    meterReading.AccountId = accountId;
                }
                else result.FailReasons.Add(MeterReadingUploadFailReason.InvalidAccountId);
                

                if (DateTime.TryParse(entry.MeterReadingDateTime, out DateTime dateTime))
                {
                    meterReading.MeterReadingDateTime = dateTime;
                }
                else result.FailReasons.Add(MeterReadingUploadFailReason.InvalidReadingDate);

                if (int.TryParse(entry.MeterReadValue, out int value))
                {
                    meterReading.MeterReadValue = value;
                }
                else result.FailReasons.Add(MeterReadingUploadFailReason.InvalidReadValue);

                // if we have any errors at this point, no point continuing, move to the next entry
                if (result.FailReasons.Any())
                {
                    results.Add(result);
                    continue;
                }
                
                // now validate other business logic

                // reading values should be NNNNN format - < 99999.. although not explicitly stated in the Acceptance criteria, im also assuming negative readings are invalid. Potentially need to clarify
                if (meterReading.MeterReadValue > 99999 || meterReading.MeterReadValue < 0)
                {
                    result.FailReasons.Add(MeterReadingUploadFailReason.InvalidReadValue);
                }

                // accountId must exist in Accounts
                if (!_dataContext.Accounts.Any(a => a.Id == meterReading.AccountId))
                {
                    result.FailReasons.Add(MeterReadingUploadFailReason.InvalidAccountId);
                }

                if (_dataContext.MeterReadings.Any(mr => mr.AccountId == meterReading.AccountId && mr.MeterReadingDateTime == meterReading.MeterReadingDateTime))
                {
                    result.FailReasons.Add(MeterReadingUploadFailReason.DuplicateMeterReadEntry);
                }

                if (result.FailReasons.Any())
                {
                    results.Add(result);
                    continue;
                }

                _dataContext.MeterReadings.Add(meterReading);
                results.Add(result);
            }

            return results;
        }
    }
}
