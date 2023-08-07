using System;

namespace NTF.DateTrigger
{
    public enum WeekDay
    {
        Invalid = -1,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7,
    }
    public enum CheckType
    {
        Inclusive,
        Exclusive
    }
    public enum DateTimeMethod
    {
        Networking,
        System
    }
    public static class Utils
    {

        public static int GetWeekDayAsInt(WeekDay day)
        {
            switch (day)
            {
                case WeekDay.Monday: return 1;
                case WeekDay.Tuesday: return 2;
                case WeekDay.Wednesday: return 3;
                case WeekDay.Thursday: return 4;
                case WeekDay.Friday: return 5;
                case WeekDay.Saturday: return 6;
                case WeekDay.Sunday: return 7;
                default: return -1;
            }
        }
        public static int GetWeekDayAsInt(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday: return 1;
                case DayOfWeek.Tuesday: return 2;
                case DayOfWeek.Wednesday: return 3;
                case DayOfWeek.Thursday: return 4;
                case DayOfWeek.Friday: return 5;
                case DayOfWeek.Saturday: return 6;
                case DayOfWeek.Sunday: return 7;
                default: return -1;
            }
        }
        public static bool Inclusive(int value, int min, int max)
        {
            int MIN = Math.Min(min, max);
            int MAX = Math.Max(max, min);
            return value >= MIN && value <= MAX;
        }
        public static bool Inclusive(int value, WeekDay min, WeekDay max)
        {
            int MIN = Math.Min((byte)min, (byte)max);
            int MAX = Math.Max((byte)max, (byte)min);
            return value >= MIN && value <= MAX;
        }
        public static bool Exclusive(int value, int min, int max)
        {
            int MIN = Math.Min(min, max);
            int MAX = Math.Max(max, min);
            return value > MIN && value < MAX;
        }
        public static bool Exclusive(int value, WeekDay min, WeekDay max)
        {
            int MIN = Math.Min((byte)min, (byte)max);
            int MAX = Math.Max((byte)max, (byte)min);
            return value > MIN && value < MAX;
        }
        public static WeekDay Convert(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday: return WeekDay.Monday;
                case DayOfWeek.Tuesday: return WeekDay.Tuesday;
                case DayOfWeek.Wednesday: return WeekDay.Wednesday;
                case DayOfWeek.Thursday: return WeekDay.Thursday;
                case DayOfWeek.Friday: return WeekDay.Friday;
                case DayOfWeek.Saturday: return WeekDay.Saturday;
                case DayOfWeek.Sunday: return WeekDay.Sunday;
                default: return WeekDay.Invalid;
            }
        }
    }
}