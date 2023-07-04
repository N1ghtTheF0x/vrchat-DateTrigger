/*
    THIS SCRIPT WAS MADE BY n1ghtthef0x
    YOU CAN CHANGE IT AND IF YOU DO ADD YOUR NAME HERE: <insert-ugly-name>
*/
using UdonSharp;
using System;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DateTrigger : UdonSharpBehaviour
{
    public bool showSettings = true;
    public bool exclusiveChecks = false;
    public GameObject targetObj = null;
    public int hour_offset = 0;
    public int fromDay = -1;
    public WeekDay fromWeek = WeekDay.Invalid;
    public int fromMonth = -1;
    public int fromYear = -1;
    public int fromHour = -1;
    public int fromMinute = -1;
    public int fromSecond = -1;
    public int toDay = -1;
    public WeekDay toWeek = WeekDay.Invalid;
    public int toMonth = -1;
    public int toYear = -1;
    public int toHour = -1;
    public int toMinute = -1;
    public int toSecond = -1;
    private DateTime curDate = Networking.GetNetworkDateTime();
    public int curDay = -1;
    public WeekDay curWeek = WeekDay.Invalid;
    public int curMonth = -1;
    public int curYear = -1;
    public int curHour = -1;
    public int curMinute = -1;
    public int curSecond = -1;
    public bool enableDay = true;
    public bool enableWeek = true;
    public bool enableMonth = true;
    public bool enableYear = true;
    public bool enableHour = true;
    public bool enableMinute = true;
    public bool enableSecond = true;
    public bool activeDay = false;
    public bool activeWeek = false;
    public bool activeMonth = false;
    public bool activeYear = false;
    public bool activeHour = false;
    public bool activeMinute = false;
    public bool activeSecond = false;
    void Start()
    {
        CheckTime();
    }
    void Update()
    {
        CheckTime();
    }
    public DateTime GetDateTime() => curDate;
    public bool Active() => targetObj != null && targetObj.activeSelf;
    public bool AllEnabled() => enableDay && enableWeek && enableMonth && enableYear && enableHour && enableMinute && enableSecond;
    void SetDateTimeProperties()
    {
        curDay = curDate.Day;
        curWeek = DateTriggerUtils.ConvertWeek(curDate.DayOfWeek);
        curMonth = curDate.Month;
        curYear = curDate.Year;
        curHour = curDate.Hour;
        curMinute = curDate.Minute;
        curSecond = curDate.Second;
    }
    void CheckTime()
    {
        // Check if the target is invalid or the object of this component. If this is the case, then we've entered an invalid state (We don't want that :) )
        if (targetObj == null && targetObj == gameObject) return;
        enableDay = enableWeek = enableMonth = enableYear = enableHour = enableMinute = enableSecond = true; // When everything goes throw correctly, this should be true
        curDate = Networking.GetNetworkDateTime().AddHours(hour_offset); // Add offset to Network Date for those timezone-enjoyers
        SetDateTimeProperties();
        // Each line checks for the enabled option and the range of the values
        if (ValidDay() && activeDay) enableDay = BetweenDay();
        if (ValidWeek() && activeWeek) enableWeek = BetweenWeek();
        if (ValidMonth() && activeMonth) enableMonth = BetweenMonth();
        if (ValidYear() && activeYear) enableYear = BetweenYear();
        if (ValidHour() && activeHour) enableHour = BetweenHour();
        if (ValidMinute() && activeMinute) enableMinute = BetweenMinute();
        if (ValidSecond() && activeSecond) enableSecond = BetweenSecond();

        targetObj.SetActive(AllEnabled()); // Disable/Enable the target. This will trigger Animators, Particle Systems, etc.
    }
    bool Valid(int value) => value > -1; // Everything under 0 is invalid (Just reminding myself that this was "value < 0"... and it took me more than 3 hours to fix...)
    // For each Valid<Name> it checks if the from and to value are not under 0
    bool ValidDay()
    {
        return Valid(fromDay) && Valid(toDay);
    }
    bool ValidWeek()
    {
        return Valid((int)fromWeek) && Valid((int)toWeek);
    }
    bool ValidMonth()
    {
        return Valid(fromMonth) && Valid(toMonth);
    }
    bool ValidYear()
    {
        return Valid(fromYear) && Valid(toYear);
    }
    bool ValidHour()
    {
        return Valid(fromHour) && Valid(toHour);
    }
    bool ValidMinute()
    {
        return Valid(fromMinute) && Valid(toMinute);
    }
    bool ValidSecond()
    {
        return Valid(fromSecond) && Valid(toSecond);
    }
    // Checks if value is between min and max. the exclusiveChecks flag switches between exclusive and inclusive
    bool Between(int value, int min, int max)
    {
        return exclusiveChecks ? BetweenExclusive(value, min, max) : BetweenInclusive(value, min, max);
    }
    // Simple inclusive range check
    bool BetweenInclusive(int value, int min, int max)
    {
        int MAX = Math.Max(max,min);
        int MIN = Math.Min(max,min);
        return value >= MIN && value <= MAX;
    }
    // Simple exclusive range check
    bool BetweenExclusive(int value, int min, int max)
    {
        int MAX = Math.Max(max,min);
        int MIN = Math.Min(max,min);
        return value > MIN && value < MAX;
    }
    // For each Between<Name> it checks if one of the DateTime property is between the coresponding range
    bool BetweenDay()
    {
        return Between(curDay, fromDay, toDay);
    }
    bool BetweenWeek()
    {
        return Between(DateTriggerUtils.GetWeek(curWeek), DateTriggerUtils.GetWeek(fromWeek), DateTriggerUtils.GetWeek(toWeek));
    }
    bool BetweenMonth()
    {
        return Between(curMonth, fromMonth, toMonth);
    }
    bool BetweenYear()
    {
        return Between(curYear, fromYear, toYear);
    }
    bool BetweenHour()
    {
        return Between(curHour, fromHour, toHour);
    }
    bool BetweenMinute()
    {
        return Between(curMinute, fromMinute, toMinute);
    }
    bool BetweenSecond()
    {
        return Between(curSecond, fromSecond, toSecond);
    }
}