using UdonSharp;
using System;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UnityEditor;
using UdonSharpEditor;
#endif

public class DateTrigger : UdonSharpBehaviour
{
    public bool exclusiveChecks = false;
    public GameObject targetObj = null;
    public int hour_offset = 0;
    public int fromDay = -1;
    public int fromMonth = -1;
    public int fromYear = -1;
    public int fromHour = -1;
    public int fromMinute = -1;
    public int fromSecond = -1;
    public int toDay = -1;
    public int toMonth = -1;
    public int toYear = -1;
    public int toHour = -1;
    public int toMinute = -1;
    public int toSecond = -1;
    private DateTime curDate = Networking.GetNetworkDateTime();
    private bool enable = true;
    private bool validDay = false;
    private bool validMonth = false;
    private bool validYear = false;
    private bool validHour = false;
    private bool validMinute = false;
    private bool validSecond = false;
    void Start()
    {
        CheckTime();
    }
    void Update()
    {
        CheckTime();
    }
    public bool IsEnabled()
    {
        return enable;
    }
    public bool IsDay()
    {
        return validDay;
    }
    public bool IsMonth()
    {
        return validMonth;
    }
    public bool IsYear()
    {
        return validYear;
    }
    public bool IsHour()
    {
        return validHour;
    }
    public bool IsMinute()
    {
        return validMinute;
    }
    public bool IsSecond()
    {
        return validSecond;
    }
    public DateTime GetDateTime()
    {
        return curDate;
    }
    void CheckTime()
    {
        if (targetObj == null) return;
        enable = true;
        curDate = Networking.GetNetworkDateTime().AddHours(hour_offset);
        if (ValidDay()) enable = validDay = BetweenDay(curDate);
        if (ValidMonth()) enable = validMonth = BetweenMonth(curDate);
        if (ValidYear()) enable = validYear = BetweenYear(curDate);
        if (ValidHour()) enable = validHour = BetweenHour(curDate);
        if (ValidMinute()) enable = validMinute = BetweenMinute(curDate);
        if (ValidSecond()) enable = validSecond = BetweenSecond(curDate);

        targetObj.SetActive(enable);
    }
    bool Valid(int value)
    {
        return value != -1;
    }
    bool ValidDay()
    {
        return Valid(fromDay) && Valid(toDay);
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
    bool Between(int value, int min, int max)
    {
        return exclusiveChecks ? BetweenExclusive(value, min, max) : BetweenInclusive(value, min, max);
    }
    bool BetweenInclusive(int value, int min, int max)
    {
        return value >= min && value <= max;
    }
    bool BetweenExclusive(int value, int min, int max)
    {
        return value > min && value < max;
    }
    bool BetweenDay(DateTime curDate)
    {
        return Between(curDate.Day, fromDay, toDay);
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
[CustomEditor(typeof(DateTrigger))]
public class DateTriggerEditor : Editor
{
    private bool showFrom = true;
    private bool showTo = true;
    private bool showSettings = true;
    // Settings
    private bool exclusiveChecks = false;
    private GameObject targetObj = null;
    private int hour_offset = 0;
    // From
    private int from_day = -1;
    private int from_month = -1;
    private int from_year = -1;
    private int from_hour = -1;
    private int from_minute = -1;
    private int from_second = -1;
    // To
    private int to_day = -1;
    private int to_month = -1;
    private int to_year = -1;
    private int to_hour = -1;
    private int to_minute = -1;
    private int to_second = -1;
    public override void OnInspectorGUI()
    {
        if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;
        DateTrigger dateTrigger = (DateTrigger)target;

        EditorGUI.BeginChangeCheck();

        showSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showSettings, "Settings");

        if (showSettings)
        {
            exclusiveChecks = EditorGUILayout.Toggle(new GUIContent("Exclusive", "Whenever to use exclusive or inclusive checks"), dateTrigger.exclusiveChecks);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Target", "The GameObject to modify. This shouldn't be the same object of this component!"));
            targetObj = (GameObject)EditorGUILayout.ObjectField(dateTrigger.targetObj, typeof(GameObject), true);
            EditorGUILayout.EndHorizontal();
            if (targetObj == dateTrigger.gameObject) EditorGUILayout.HelpBox("Can't be the object with the component! This will break this script!", MessageType.Error);
            hour_offset = EditorGUILayout.IntField(new GUIContent("Hour Offset", "Add hours to the check"), dateTrigger.hour_offset);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        showFrom = EditorGUILayout.BeginFoldoutHeaderGroup(showFrom, "From");

        if (showFrom)
        {
            from_day = DayField(dateTrigger.fromDay);
            from_month = MonthField(dateTrigger.fromMonth);
            from_year = YearField(dateTrigger.fromYear);

            from_hour = HourField(dateTrigger.fromHour);
            from_minute = MinuteField(dateTrigger.fromMinute);
            from_second = SecondField(dateTrigger.fromSecond);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
        showTo = EditorGUILayout.BeginFoldoutHeaderGroup(showTo, "To");

        if (showTo)
        {
            to_day = DayField(dateTrigger.toDay);
            to_month = MonthField(dateTrigger.toMonth);
            to_year = YearField(dateTrigger.toYear);

            to_hour = HourField(dateTrigger.toHour);
            to_minute = MinuteField(dateTrigger.toMinute);
            to_second = SecondField(dateTrigger.toSecond);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(dateTrigger,"Modify DateTrigger");

            dateTrigger.exclusiveChecks = exclusiveChecks;
            dateTrigger.hour_offset = hour_offset;
            dateTrigger.targetObj = targetObj;

            dateTrigger.fromDay = from_day;
            dateTrigger.fromMonth = from_month;
            dateTrigger.fromYear = from_year;

            dateTrigger.fromHour = from_hour;
            dateTrigger.fromMinute = from_minute;
            dateTrigger.fromSecond = from_second;

            dateTrigger.toDay = to_day;
            dateTrigger.toMonth = to_month;
            dateTrigger.toYear = to_year;

            dateTrigger.toHour = to_hour;
            dateTrigger.toMinute = to_minute;
            dateTrigger.toSecond = to_second;
        }
    }
    int Limit(int value, int max)
    {
        int min = -1;
        if (value > max) return max;
        if (value < min) return min;
        return value;
    }
    private int DayField(int value)
    {
        return Limit(EditorGUILayout.IntField(new GUIContent("Day", "Day of the month (1-31)"), value),31);
    }
    private int MonthField(int value)
    {
        return Limit(EditorGUILayout.IntField(new GUIContent("Month", "Month (1-12)"), value),12);
    }
    private int YearField(int value)
    {
        return EditorGUILayout.IntField(new GUIContent("Year", "Year (1970-*)"), value);
    }
    private int HourField(int value)
    {
        return Limit(EditorGUILayout.IntField(new GUIContent("Hour", "Hour (0-24)"), value),24);
    }
    private int MinuteField(int value)
    {
        return Limit(EditorGUILayout.IntField(new GUIContent("Minute", "Minute (0-60)"), value),60);
    }
    private int SecondField(int value)
    {
        return Limit(EditorGUILayout.IntField(new GUIContent("Second", "Second (0-60)"), value),60);
    }
}
#endif