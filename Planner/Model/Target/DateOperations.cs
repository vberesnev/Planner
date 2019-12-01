using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model.Target
{
    public static class DateOperations
    {
        public static DateTime PeriodStart(TargetType type, int year, int periodValue)
        {
            switch (type)
            {
                case TargetType.Year:
                    return new DateTime(periodValue, 1, 1, 0, 0, 0);

                case TargetType.Month:
                    return new DateTime(year, periodValue, 1, 0, 0, 0);

                case TargetType.Week:
                    DateTime start = new DateTime(year, 1, 1, 0, 0, 0);
                    if (start.DayOfWeek == System.DayOfWeek.Monday)
                        return start.AddDays(7 * (periodValue - 1));

                    while (start.DayOfWeek != System.DayOfWeek.Monday)
                    {
                        start = start.AddDays(1);
                    }
                    return start.AddDays(7 * (periodValue - 2));

                case TargetType.Day:
                    DateTime startDay = new DateTime(year, 1, 1, 0, 0, 0);
                    return startDay.AddDays(periodValue - 1);
            }
            return DateTime.MinValue;
        }

        public static DateTime PeriodFinish(TargetType type, int year, int periodValue)
        {
            switch (type)
            {
                case TargetType.Year:
                    return new DateTime(periodValue, 12, 31, 23, 59, 59);

                case TargetType.Month:
                    int lastMonthday = DateTime.DaysInMonth(year, periodValue);
                    return new DateTime(year, periodValue, lastMonthday, 23, 59, 59);

                case TargetType.Week:
                    DateTime start = new DateTime(year, 1, 1, 23, 59, 59);
                    while (start.DayOfWeek != System.DayOfWeek.Sunday)
                    {
                        start = start.AddDays(1);
                    }
                    return start.AddDays(7 * (periodValue - 1));

                case TargetType.Day:
                    DateTime startDay = new DateTime(year, 1, 1, 23, 59, 59);
                    return startDay.AddDays(periodValue - 1);
            }
            return DateTime.MaxValue;
        }
    }
}
