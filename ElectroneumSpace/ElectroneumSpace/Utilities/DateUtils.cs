using System;
using System.Linq;
using System.Collections.Generic;

namespace ElectroneumSpace.Utilities
{
    public static class DateUtils
    {

        public static DateTime GetDateTimeFromUnixTimestamp(long unixTimestamp)
        {
            // Make assumptions on unix epoch from ms to s (introduce year 2400 Y2K bug)
            if (unixTimestamp <= 10000000000)
                return new DateTime(1970, 1, 1).AddSeconds(unixTimestamp);
            return new DateTime(1970, 1, 1).AddMilliseconds(unixTimestamp);
        }

        public static TimeSpan GetAverageIntervalFromCollectionOfTimestamps(IList<DateTime> timestamps)
        {
            // Validate
            if (timestamps == null || timestamps.Count <= 1)
                return TimeSpan.FromSeconds(0);

            // Get Differential
            var max = timestamps.Max();
            var min = timestamps.Min();
            var diff = max - min;

            // Calculate Average
            var diffMillis = diff.TotalMilliseconds;
            var diffCount = timestamps.Count - 1;
            var diffAverage = TimeSpan.FromMilliseconds(diffMillis / diffCount);

            // Return
            return diffAverage;
        }

    }
}
