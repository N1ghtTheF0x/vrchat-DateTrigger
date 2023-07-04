using System;
public static class DateTriggerUtils
{
    public static int GetWeek(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday:
                return 1;
            case DayOfWeek.Tuesday:
                return 2;
            case DayOfWeek.Wednesday:
                return 3;
            case DayOfWeek.Thursday:
                return 4;
            case DayOfWeek.Friday:
                return 5;
            case DayOfWeek.Saturday:
                return 6;
            case DayOfWeek.Sunday:
                return 7;
            default:
                return -1;
        }
    }
    public static WeekDay ConvertWeek(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday:
                return WeekDay.Monday;
            case DayOfWeek.Tuesday:
                return WeekDay.Tuesday;
            case DayOfWeek.Wednesday:
                return WeekDay.Wednesday;
            case DayOfWeek.Thursday:
                return WeekDay.Thursday;
            case DayOfWeek.Friday:
                return WeekDay.Friday;
            case DayOfWeek.Saturday:
                return WeekDay.Saturday;
            case DayOfWeek.Sunday:
                return WeekDay.Sunday;
            default:
                return WeekDay.Invalid;
        }
    }
    public static int GetWeek(WeekDay day)
    {
        switch (day)
        {
            case WeekDay.Monday:
                return 1;
            case WeekDay.Tuesday:
                return 2;
            case WeekDay.Wednesday:
                return 3;
            case WeekDay.Thursday:
                return 4;
            case WeekDay.Friday:
                return 5;
            case WeekDay.Saturday:
                return 6;
            case WeekDay.Sunday:
                return 7;
            default:
                return -1;
        }
    }
}