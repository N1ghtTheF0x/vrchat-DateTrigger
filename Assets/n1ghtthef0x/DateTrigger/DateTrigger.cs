
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;
using NTF.DateTrigger;
using System;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
[AddComponentMenu("N1ghtTheF0x/Date Trigger")]
public class DateTrigger : UdonSharpBehaviour
{
    // SETTINGS
    [Header("Settings")]
    // Settings Properties
    public int hourOffset = 0;
    public CheckType checkType = CheckType.Inclusive;
    public bool onStart = true;
    public bool onUpdate = true;
    // TARGETS
    [Header("GameObject Settings")]
    public GameObject targetGameobject = null;
    [Header("Animator Settings")]
    public Animator targetAnimator = null;
    public string enabledState = "DTON";
    public string disabledState = "DTOFF";
    // RANGE
    [Header("Range")]
    // Day Properties
    public bool enableDay = false;
    public int fromDay = -1;
    public int toDay = -1;
    // Week Properties
    public bool enableWeek = false;
    public WeekDay fromWeek = WeekDay.Invalid;
    public WeekDay toWeek = WeekDay.Invalid;
    // Month Properties
    public bool enableMonth = false;
    public int fromMonth = -1;
    public int toMonth = -1;
    // Year Properties
    public bool enableYear = false;
    public int fromYear = -1;
    public int toYear = -1;
    // Hour Properties
    public bool enableHour = false;
    public int fromHour = -1;
    public int toHour = -1;
    // Minute Properties
    public bool enableMinute = false;
    public int fromMinute = -1;
    public int toMinute = -1;
    // Second Properties
    public bool enableSecond = false;
    public int fromSecond = -1;
    public int toSecond = -1;
    // Internal Properties
    private bool lastTimeActive = true;
    private string currentState = string.Empty;
    private void Start()
    {
        if (onStart) Check();
    }
    private void Update()
    {
        if (onUpdate) Check();
    }
    private bool Check(int value,int min, int max)
    {
        if (min == max) return value == min;
        return checkType == CheckType.Inclusive ? Utils.Inclusive(value,min,max) : Utils.Exclusive(value,min,max);
    }
    private void Check()
    {
        DateTime curDate = Networking.GetNetworkDateTime().AddHours(hourOffset);
        bool curActive = true;

        if(enableDay) curActive = Check(curDate.Day,fromDay,toDay);
        if(enableWeek) curActive = Check(Utils.GetWeekDayAsInt(curDate.DayOfWeek),(int)fromWeek,(int)toWeek);
        if(enableMonth) curActive = Check(curDate.Month,fromMonth,toMonth);
        if(enableYear) curActive = Check(curDate.Year,fromYear,toYear);
        if(enableHour) curActive = Check(curDate.Hour,fromHour,toHour);
        if(enableMinute) curActive = Check(curDate.Minute,fromMinute,toMinute);
        if(enableSecond) curActive = Check(curDate.Second,fromSecond,toSecond);

        if(curActive != lastTimeActive) 
        {   
            if(targetGameobject != null) targetGameobject.SetActive(curActive);
            if(targetAnimator != null)
            {
                string state = curActive ? enabledState : disabledState;
                if(state != string.Empty && currentState != state)
                {
                    if (IsAnimatorPlaying()) targetAnimator.StopPlayback();
                    targetAnimator.Play(state);
                    currentState = state;
                }
            }
        }
        lastTimeActive = curActive;
    }
    private bool IsAnimatorPlaying()
    {
        if(targetAnimator == null) return false;

        for(int i = 0;i < targetAnimator.layerCount;i++)
        {
            var info = targetAnimator.GetCurrentAnimatorStateInfo(i);
            if (info.normalizedTime != 0) return true;
        }

        return false;
    }
}