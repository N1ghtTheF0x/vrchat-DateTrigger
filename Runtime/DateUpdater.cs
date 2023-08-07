
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using NTF.DateTrigger;
using System;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
[AddComponentMenu("N1ghtTheF0x/Date Updater")]
public class DateUpdater : UdonSharpBehaviour
{
    public DateTimeMethod method = DateTimeMethod.Networking;
    public DateTime dateTime;
    void Start()
    {
        
    }
    void Update()
    {
        if(method == DateTimeMethod.Networking) dateTime = Networking.GetNetworkDateTime();
        else if(method == DateTimeMethod.System) dateTime = DateTime.Now;
    }
}
