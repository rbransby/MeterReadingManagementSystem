using System;
using System.Collections.Generic;
using System.Text;

namespace MeterReadingsManagementSystem.Shared
{
    
    public enum MeterReadingUploadFailReason
    {
        None = 0,
        InvalidAccountId = 1,
        InvalidReadingDate = 2,
        InvalidReadValue = 3,
        DuplicateMeterReadEntry = 4,
    }

}
