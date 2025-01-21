using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public class EventRankPromotion : Unity.Services.Analytics.Event
{
    public EventRankPromotion() : base("rankPromotion")
    {
    }

    public string RankName { set { SetParameter("rankName", value); } }

}
