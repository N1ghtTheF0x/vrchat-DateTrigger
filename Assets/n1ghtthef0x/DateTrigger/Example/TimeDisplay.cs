
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class TimeDisplay : UdonSharpBehaviour
{
    public GameObject obj; // TextDisplay object. Is located in the scene
    public DateTrigger dt; // The DataTrigger to debug
    private Text text; // Unity text component
    void Start()
    {
        text = obj.GetComponent<Text>(); // This somewhat works... and I don't know if it's intentional...
    }
    void Update()
    {
        if(text == null) return; // text is null? Well f**k you
        var time = dt.GetDateTime(); // Get DateTime object from DateTrigger
        // Text format: DD/MM/YYYY HH:MM:SS (Active or nothing)
        var value = time.Day + "/" + time.Month + "/" + time.Year + " " + time.Hour + ":" + time.Minute + ":" + time.Second + " " + time.DayOfWeek.ToString()
            + (dt.Active() ? "\nActive" : "");
        text.text = value; // Show that pu... uh I mean text
    }
}
