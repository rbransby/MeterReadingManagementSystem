﻿namespace MeterReadingsManagementSystem.Shared
{

    public enum MeterReadingProcessFailReason
    {
        None = 0,
        InvalidAccountId = 1,
        InvalidReadingDate = 2,
        InvalidReadValue = 3,
        DuplicateMeterReadEntry = 4,
    }

}
