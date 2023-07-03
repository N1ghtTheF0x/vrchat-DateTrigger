
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class TimeDisplay : UdonSharpBehaviour
{
    public GameObject obj;
    public DateTrigger dt;
    private Text text;
    void Start()
    {
        text = obj.GetComponent<Text>();
    }
    void Update()
    {
        if(text == null) return;
        var time = dt.GetDateTime();
        var value = time.Day + "/" + time.Month + "/" + time.Year + " " + time.Hour + ":" + time.Minute + ":" + time.Second + " = " + (dt.IsEnabled() ? "Yes" : "No")
            + (dt.IsDay() ? "\nDay" : "")
            + (dt.IsMonth() ? "\nMonth" : "")
            + (dt.IsYear() ? "\nYear" : "")
            + (dt.IsHour() ? "\nHour" : "")
            + (dt.IsMinute() ? "\nMinute" : "")
            + (dt.IsSecond() ? "\nSecond" : "");
        text.text = value;
    }
}
