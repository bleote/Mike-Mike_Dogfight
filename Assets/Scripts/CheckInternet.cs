using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInternet : MonoBehaviour
{   
    private void Awake()
    {
        CheckInternetConnection();
    }

    private void CheckInternetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            GameManager.hasNetwork = false;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            GameManager.hasNetwork = true;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            GameManager.hasNetwork = true;
        }
    }
}
