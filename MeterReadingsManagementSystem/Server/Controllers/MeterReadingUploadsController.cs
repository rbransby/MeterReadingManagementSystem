using CsvHelper;
using MeterReadingsManagementSystem.Server.Services;
using MeterReadingsManagementSystem.Shared;
using MeterReadingsManagementSystem.Shared.Csv;
using MeterReadingsManagementSystem.Shared.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static MeterReadingsManagementSystem.Server.Migrations.AddSeedData;

namespace MeterReadingsManagementSystem.Server.Controllers
{
    [ApiController]
    [Route("meter-reading-uploads")]
    public class MeterReadingUploadsController : ControllerBase
    {
        private readonly MeterReadProcessingService _meterReadProcessingService;

        public MeterReadingUploadsController(MeterReadProcessingService meterReadProcessingService)
        {
            this._meterReadProcessingService = meterReadProcessingService;
        }

        [HttpPost]
        public async Task<ActionResult<MeterReadingUploadResult>> Post(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                    var entries = csvReader.GetRecords<MeterReadingEntry>();
                    var processingResults = _meterReadProcessingService.ProcessMeterReadEntries(entries);

                    var meterReadingUploadResult = new MeterReadingUploadResult()
                    {
                        ReadProcessingResults = processingResults
                    };

                    return Ok(meterReadingUploadResult);
                }
            }

        }
    }
}
