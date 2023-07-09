#if UNITY_EDITOR
using UnityEditor;
using UdonSharpEditor;
using NTF.DateTrigger;
using UnityEngine;

[CustomEditor(typeof(DateTrigger))]
public class DateTriggerEditor : Editor
{
    // SETTINGS
    private bool settingsEnabled = true;
    // Settings Properties
    private int hourOffset = 0;
    private CheckType checkType = CheckType.Inclusive;
    private bool onStart = true;
    private bool onUpdate = true;
    // TARGETS
    private bool targetsEnabled = true;
    // Targets Properties
    private Animator targetAnimator = null;
    private string targetParameter = "DateTrigger";
    // private
    private bool rangeEnabled = true;
    // Day Properties
    private bool enableDay = false;
    private int fromDay = -1;
    private int toDay = -1;
    // Week Properties
    private bool enableWeek = false;
    private WeekDay fromWeek = WeekDay.Invalid;
    private WeekDay toWeek = WeekDay.Invalid;
    // Month Properties
    private bool enableMonth = false;
    private int fromMonth = -1;
    private int toMonth = -1;
    // Year Properties
    private bool enableYear = false;
    private int fromYear = -1;
    private int toYear = -1;
    // Hour Properties
    private bool enableHour = false;
    private int fromHour = -1;
    private int toHour = -1;
    // Minute Properties
    private bool enableMinute = false;
    private int fromMinute = -1;
    private int toMinute = -1;
    // Second Properties
    private bool enableSecond = false;
    private int fromSecond = -1;
    private int toSecond = -1;
    public override void OnInspectorGUI()
    {
        if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;

        DateTrigger dateTrigger = (DateTrigger)target;

        EditorGUI.BeginChangeCheck();
        InspectorLayout(dateTrigger);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(dateTrigger, "Modify Date Trigger");
            InspectorSave(dateTrigger);
        }
    }
    private void InspectorLayout(DateTrigger dateTrigger)
    {
        InspectorLayoutSettings(dateTrigger);
        InspectorLayoutTargets(dateTrigger);
        InspectorLayoutRange(dateTrigger);
    }

    private void InspectorLayoutSettings(DateTrigger dateTrigger)
    {
        settingsEnabled = EditorGUILayout.BeginFoldoutHeaderGroup(dateTrigger.settingsEnabled, "Settings");

        if (settingsEnabled)
        {
            hourOffset = EditorGUILayout.IntField(new GUIContent("Hour Offset", "Add Hours to the check for timezones or something like that"), dateTrigger.hourOffset);
            checkType = (CheckType)EditorGUILayout.EnumPopup(new GUIContent("Internal Check type", "Whenever you want Inclusive or Exclusive checking"), dateTrigger.checkType);
            EditorGUILayout.BeginHorizontal();
            onStart = EditorGUILayout.Toggle(new GUIContent("on Start", "Should the script execute at launch?"), dateTrigger.onStart);
            onUpdate = EditorGUILayout.Toggle(new GUIContent("on Update","Should the script execute all the time?"), dateTrigger.onUpdate);
            EditorGUILayout.EndHorizontal();
            if (!onStart && !onUpdate) EditorGUILayout.HelpBox("At least one option should be enabled!", MessageType.Error);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
    }
    private void InspectorLayoutTargets(DateTrigger dateTrigger)
    {
        targetsEnabled = EditorGUILayout.BeginFoldoutHeaderGroup(dateTrigger.settingsEnabled, "Targets");

        if(targetsEnabled)
        { 
            if (dateTrigger.gameObject.TryGetComponent(out Animator animator))
            {
                targetAnimator = animator;
                EditorGUILayout.HelpBox("Using Animator on this GameObject", MessageType.Info);
            }
            else
            {
                targetAnimator = (Animator)EditorGUILayout.ObjectField(new GUIContent("Target", "Plays animation depending if the script is active"), dateTrigger.targetAnimator, typeof(Animator), true);
            }

            if (targetAnimator == null) EditorGUILayout.HelpBox("You should assign an Animator or put an Animator Component!", MessageType.Error);
            targetParameter = EditorGUILayout.TextField(new GUIContent("Parameter","The parameter in the Animator. Should be an boolean."), dateTrigger.targetParameter);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }
    private void InspectorLayoutRange(DateTrigger dateTrigger)
    {
        rangeEnabled = EditorGUILayout.BeginFoldoutHeaderGroup(dateTrigger.rangeEnabled, "Range");

        if(rangeEnabled)
        {
            var day = DateTimeField(dateTrigger.enableDay,dateTrigger.fromDay,dateTrigger.toDay,new GUIContent("Day","1 - 31"));
            enableDay = day.Item1;fromDay = day.Item2;toDay = day.Item3;
            var week = DateTimeField(dateTrigger.enableWeek, dateTrigger.fromWeek, dateTrigger.toWeek, new GUIContent("Week", "Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday"));
            enableWeek = week.Item1;fromWeek = week.Item2;toWeek = week.Item3;
            var month = DateTimeField(dateTrigger.enableMonth, dateTrigger.fromMonth, dateTrigger.toMonth, new GUIContent("Month","1 - 12"));
            enableMonth = month.Item1;fromMonth = month.Item2;toMonth = month.Item3;
            var year = DateTimeField(dateTrigger.enableYear, dateTrigger.fromYear, dateTrigger.toYear, new GUIContent("Year","1 - 9999"));
            enableYear = year.Item1;fromYear = year.Item2;toYear = year.Item3;

            var hour = DateTimeField(dateTrigger.enableHour, dateTrigger.fromHour, dateTrigger.toHour, new GUIContent("Hour", "0 - 23"));
            enableHour = hour.Item1;fromHour = hour.Item2;toHour = hour.Item3;
            var minute = DateTimeField(dateTrigger.enableMinute, dateTrigger.fromMinute, dateTrigger.toMinute, new GUIContent("Minute", "0 - 59"));
            enableMinute = minute.Item1;fromMinute = minute.Item2;toMinute = minute.Item3;
            var second = DateTimeField(dateTrigger.enableSecond, dateTrigger.fromSecond, dateTrigger.toSecond, new GUIContent("Second","0 - 59"));
            enableSecond = second.Item1;fromSecond = second.Item2;toSecond = second.Item3;
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
    }
    private (bool,int,int) DateTimeField(bool enabled,int from,int to,GUIContent content)
    {
        bool e = EditorGUILayout.BeginToggleGroup(content, enabled);
        int f = EditorGUILayout.IntField("From",from);
        int t = EditorGUILayout.IntField("To",to);
        if (e && (f == -1 || t == -1)) EditorGUILayout.HelpBox("Both values need to be higher than -1. This will not work!", MessageType.Warning);
        if (e && f != -1 && t != -1 && f == t) EditorGUILayout.HelpBox("This will now check EXACTLY " + f, MessageType.Info);
        EditorGUILayout.EndToggleGroup();
        return (e,f,t);
    }
    private (bool,WeekDay,WeekDay) DateTimeField(bool enabled,WeekDay from,WeekDay to,GUIContent content)
    {
        bool e = EditorGUILayout.BeginToggleGroup(content, enabled);
        WeekDay f = (WeekDay)EditorGUILayout.EnumPopup("From", from);
        WeekDay t = (WeekDay)EditorGUILayout.EnumPopup("To", to);
        if (e && (f == WeekDay.Invalid || t == WeekDay.Invalid)) EditorGUILayout.HelpBox("Both values need to be something other than Invalid. This will not work!", MessageType.Warning);
        if (e && f != WeekDay.Invalid && t != WeekDay.Invalid && f == t) EditorGUILayout.HelpBox("This will now check EXACTLY " + f.ToString(), MessageType.Info);
        EditorGUILayout.EndToggleGroup();
        return (e, f, t);
    }
    private void InspectorSave(DateTrigger dateTrigger)
    {
        dateTrigger.settingsEnabled = settingsEnabled;

        dateTrigger.hourOffset = hourOffset;
        dateTrigger.checkType = checkType;
        dateTrigger.onStart = onStart;
        dateTrigger.onUpdate = onUpdate;

        dateTrigger.targetAnimator = targetAnimator;
        dateTrigger.targetParameter = targetParameter;

        dateTrigger.rangeEnabled = rangeEnabled;

        dateTrigger.enableDay = enableDay;
        dateTrigger.fromDay = fromDay;
        dateTrigger.toDay = toDay;

        dateTrigger.enableWeek = enableWeek;
        dateTrigger.fromWeek = fromWeek;
        dateTrigger.toWeek = toWeek;

        dateTrigger.enableMonth = enableMonth;
        dateTrigger.fromMonth = fromMonth;
        dateTrigger.toMonth = toMonth;

        dateTrigger.enableYear = enableYear;
        dateTrigger.fromYear = fromYear;
        dateTrigger.toYear = toYear;

        dateTrigger.enableHour = enableHour;
        dateTrigger.fromHour = fromHour;
        dateTrigger.toHour = toHour;

        dateTrigger.enableMinute = enableMinute;
        dateTrigger.fromMinute = fromMinute;
        dateTrigger.toMinute = toMinute;

        dateTrigger.enableSecond = enableSecond;
        dateTrigger.fromSecond = fromSecond;
        dateTrigger.toSecond = toSecond;
    }
}
#endif