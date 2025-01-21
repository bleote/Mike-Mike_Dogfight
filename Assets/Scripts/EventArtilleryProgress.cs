using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public class EventArtilleryProgress : Unity.Services.Analytics.Event
{
    public EventArtilleryProgress() : base("artilleryProgress")
    {
    }

    public string ArtilleryLevelString { set { SetParameter("artilleryLevelString", value); } }

}
