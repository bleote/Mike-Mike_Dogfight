using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public class EventAircraftProgress : Unity.Services.Analytics.Event
{
    public EventAircraftProgress() : base("aircraftProgress")
    {
    }

    public string AircraftLevelString { set { SetParameter("aircraftLevelString", value); } }

}
