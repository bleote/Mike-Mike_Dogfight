using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public class EventAwardReceived : Unity.Services.Analytics.Event
{
    public EventAwardReceived() : base("awardReceived")
    {
    }

    public string AwardName { set { SetParameter("awardName", value); } }

}
