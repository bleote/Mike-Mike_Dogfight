using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public class EventSurvivedWave : Unity.Services.Analytics.Event
{
    public EventSurvivedWave() : base("survivedWave")
    {
    }

    public string WaveNumber { set { SetParameter("waveNumber", value); } }
}
