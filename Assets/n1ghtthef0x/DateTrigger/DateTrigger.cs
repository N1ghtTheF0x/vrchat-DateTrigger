/*
    THIS SCRIPT WAS MADE BY n1ghtthef0x
    YOU CAN CHANGE IT AND IF YOU DO ADD YOUR NAME HERE: <insert-ugly-name>
*/
using UdonSharp;
using System;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UnityEditor;
using UdonSharpEditor;
#endif
public enum WeekDay
{
    Invalid = -1,
    Monday = 1,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}
public static class DateTriggerUtils
{
    public static WeekDay GetWeek(DayOfWeek day)
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
}
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
    private bool enable = true;
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
    void CheckTime()
    {
        // Check if the target is invalid or the object of this component. If this is the case, then we've entered an invalid state (We don't want that :) )
        if (targetObj == null && targetObj == gameObject) return;
        enable = true; // When everything goes throw correctly, this should be true
        curDate = Networking.GetNetworkDateTime().AddHours(hour_offset); // Add offset to Network Date for those timezone-enjoyers
        // Each line checks for the enabled option and the range of the values
        if (ValidDay() && activeDay) enable = BetweenDay(curDate);
        if (ValidWeek() && activeWeek) enable = BetweenWeek(curDate);
        if (ValidMonth() && activeMonth) enable = BetweenMonth(curDate);
        if (ValidYear() && activeYear) enable = BetweenYear(curDate);
        if (ValidHour() && activeHour) enable = BetweenHour(curDate);
        if (ValidMinute() && activeMinute) enable = BetweenMinute(curDate);
        if (ValidSecond() && activeSecond) enable = BetweenSecond(curDate);

        targetObj.SetActive(enable); // Disable/Enable the target. This will trigger Animators, Particle Systems, etc.
    }
    bool Valid(int value) => value < 0; // Everything under 0 is invalid
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
        return value >= min && value <= max;
    }
    // Simple exclusive range check
    bool BetweenExclusive(int value, int min, int max)
    {
        return value > min && value < max;
    }
    // For each Between<Name> it checks if one of the DateTime property is between the coresponding range
    bool BetweenDay(DateTime curDate)
    {
        return Between(curDate.Day, fromDay, toDay);
    }
    bool BetweenWeek(DateTime curDate)
    {
        return Between((int)DateTriggerUtils.GetWeek(curDate.DayOfWeek), (int)fromWeek, (int)toWeek);
    }
    bool BetweenMonth(DateTime curDate)
    {
        return Between(curDate.Month, fromMonth, toMonth);
    }
    bool BetweenYear(DateTime curDate)
    {
        return Between(curDate.Year, fromYear, toYear);
    }
    bool BetweenHour(DateTime curDate)
    {
        return Between(curDate.Hour, fromHour, toHour);
    }
    bool BetweenMinute(DateTime curDate)
    {
        return Between(curDate.Minute, fromMinute, toMinute);
    }
    bool BetweenSecond(DateTime curDate)
    {
        return Between(curDate.Second, fromSecond, toSecond);
    }
}
#if !COMPILER_UDONSHARP && UNITY_EDITOR
// Custom Inspector stuff
[CustomEditor(typeof(DateTrigger))]
public class DateTriggerEditor : Editor
{
    // These properties are just temporal, they aren't saved!
    private bool showSettings = true;
    // Settings
    private bool exclusiveChecks = false;
    private GameObject targetObj = null;
    private int hourOffset = 0;
    // From
    private int fromDay = -1;
    private WeekDay fromWeek = WeekDay.Invalid;
    private int fromMonth = -1;
    private int fromYear = -1;
    private int fromHour = -1;
    private int fromMinute = -1;
    private int fromSecond = -1;
    // To
    private int toDay = -1;
    private WeekDay toWeek = WeekDay.Invalid;
    private int toMonth = -1;
    private int toYear = -1;
    private int toHour = -1;
    private int toMinute = -1;
    private int toSecond = -1;
    // Active
    private bool activeDay = false;
    private bool activeWeek = false;
    private bool activeMonth = false;
    private bool activeYear = false;
    private bool activeHour = false;
    private bool activeMinute = false;
    private bool activeSecond = false;
    public override void OnInspectorGUI()
    {
        // Show the Utilites group thing
        if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;
        DateTrigger dateTrigger = (DateTrigger)target; // target is AND should be DateTrigger

        EditorGUI.BeginChangeCheck();

        // This is the Settings group thing
        showSettings = EditorGUILayout.BeginFoldoutHeaderGroup(dateTrigger.showSettings, "Settings");

        if (showSettings)
        {
            exclusiveChecks = EditorGUILayout.Toggle(new GUIContent("Exclusive", "Whenever to use exclusive or inclusive checks"), dateTrigger.exclusiveChecks);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Target", "The GameObject to modify. This shouldn't be the same object of this component!"));
            targetObj = (GameObject)EditorGUILayout.ObjectField(dateTrigger.targetObj, typeof(GameObject), true);
            EditorGUILayout.EndHorizontal();
            if (targetObj == dateTrigger.gameObject) EditorGUILayout.HelpBox("Can't be the object with the component! This will break this script!", MessageType.Error);
            hourOffset = EditorGUILayout.IntField(new GUIContent("Hour Offset", "Add hours to the check"), dateTrigger.hour_offset);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // This is so ugly but it works 👍
        (int fDay, int tDay, bool aDay) = DateTimeField(dateTrigger.fromDay, dateTrigger.toDay, dateTrigger.activeDay, new GUIContent("Day", "Day (0-31)"), 31); fromDay = fDay; toDay = tDay; activeDay = aDay;
        (WeekDay fWeek, WeekDay tWeek, bool aWeek) = DateTimeField(dateTrigger.fromWeek, dateTrigger.toWeek, dateTrigger.activeWeek); fromWeek = fWeek; toWeek = tWeek; activeWeek = aWeek;
        (int fMonth, int tMonth, bool aMonth) = DateTimeField(dateTrigger.fromMonth, dateTrigger.toMonth, dateTrigger.activeMonth, new GUIContent("Month", "Month (1-12)"), 12); fromMonth = fMonth; toMonth = tMonth; activeMonth = aMonth;
        (int fYear, int tYear, bool aYear) = DateTimeField(dateTrigger.fromYear, dateTrigger.toYear, dateTrigger.activeYear); fromYear = fYear; toYear = tYear; activeYear = aYear;

        (int fHour, int tHour, bool aHour) = DateTimeField(dateTrigger.fromHour, dateTrigger.toHour, dateTrigger.activeHour, new GUIContent("Hour", "Hour (0-24)"), 24); fromHour = fHour; toHour = tHour; activeHour = aHour;
        (int fMinute, int tMinute, bool aMinute) = DateTimeField(dateTrigger.fromMinute, dateTrigger.toMinute, dateTrigger.activeMinute, new GUIContent("Minute", "Minute (0-60)"), 60); fromMinute = fMinute; toMinute = tMinute; activeMinute = aMinute;
        (int fSecond, int tSecond, bool aSecond) = DateTimeField(dateTrigger.fromSecond, dateTrigger.toSecond, dateTrigger.activeSecond, new GUIContent("Second", "Second (0-60)"), 60); fromSecond = fSecond; toSecond = tSecond; activeSecond = aSecond;

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(dateTrigger, "Modify DateTrigger");

            // Applying all values to the component

            dateTrigger.showSettings = showSettings;

            dateTrigger.exclusiveChecks = exclusiveChecks;
            dateTrigger.hour_offset = hourOffset;
            dateTrigger.targetObj = targetObj;

            dateTrigger.fromDay = fromDay;
            dateTrigger.fromWeek = fromWeek;
            dateTrigger.fromMonth = fromMonth;
            dateTrigger.fromYear = fromYear;

            dateTrigger.fromHour = fromHour;
            dateTrigger.fromMinute = fromMinute;
            dateTrigger.fromSecond = fromSecond;

            dateTrigger.toDay = toDay;
            dateTrigger.toWeek = toWeek;
            dateTrigger.toMonth = toMonth;
            dateTrigger.toYear = toYear;

            dateTrigger.toHour = toHour;
            dateTrigger.toMinute = toMinute;
            dateTrigger.toSecond = toSecond;

            dateTrigger.activeDay = activeDay;
            dateTrigger.activeWeek = activeWeek;
            dateTrigger.activeMonth = activeMonth;
            dateTrigger.activeYear = activeYear;

            dateTrigger.activeHour = activeHour;
            dateTrigger.activeMinute = activeMinute;
            dateTrigger.activeSecond = activeSecond;
        }
    }
    // Limits the value between -1 and max
    int Limit(int value, int max)
    {
        int min = -1;
        if (value > max) return max;
        if (value < 0) return min;
        return value;
    }
    // This is probably the fanciest method 😃
    private (int, int, bool) DateTimeField(int fromV, int toV, bool active, GUIContent content, int max)
    {
        active = EditorGUILayout.BeginToggleGroup(content, active);
        var from = Limit(EditorGUILayout.IntField("From", fromV), max);
        var to = Limit(EditorGUILayout.IntField("To", toV), max);
        EditorGUILayout.EndToggleGroup();
        return (from, to, active);
    }
    // Same as the upper method but with WeekDay enum instead
    private (WeekDay, WeekDay, bool) DateTimeField(WeekDay fromV, WeekDay toV, bool active)
    {
        active = EditorGUILayout.BeginToggleGroup(new GUIContent("Week", "Week (Mon,Tue,Wed,Thu,Fri,Sat,Sun)"), active);
        var from = (WeekDay)EditorGUILayout.EnumPopup("From", fromV);
        var to = (WeekDay)EditorGUILayout.EnumPopup("To", toV);
        EditorGUILayout.EndToggleGroup();
        return (from, to, active);
    }
    // Same as the first method but has a funny gag 🗿
    private (int, int, bool) DateTimeField(int fromV, int toV, bool active)
    {
        var curYear = DateTime.Now.Year;
        active = EditorGUILayout.BeginToggleGroup(new GUIContent("Year", "Year (1970-*)"), active);
        var from = EditorGUILayout.IntField("From", fromV);
        var to = EditorGUILayout.IntField("To", toV);
        if (to != -1 && to < curYear) EditorGUILayout.HelpBox("Time travel is nice but this won't work ;P", MessageType.Warning);
        EditorGUILayout.EndToggleGroup();
        return (from, to, active);
    }
}
#endif