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
            if (targetObj == null) EditorGUILayout.HelpBox("This field should be an GameObject!", MessageType.Error);
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