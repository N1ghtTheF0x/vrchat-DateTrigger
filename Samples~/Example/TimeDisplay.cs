
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TimeDisplay : UdonSharpBehaviour
{
    public TextMeshPro mesh;
    public DateUpdater updater;
    void Start()
    {
        
    }
    void Update()
    {
        var date = updater.dateTime;
        if(mesh != null) mesh.text = date.ToLongDateString() + " " + date.ToLongTimeString();
    }
}
