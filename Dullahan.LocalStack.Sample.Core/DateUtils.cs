using System;

namespace Dullahan.LocalStack.Sample.Core
{
    public static class DateUtils
    {
        public static string ToComparableDateString(this DateTime o) => o.ToString("s");
    }
}
