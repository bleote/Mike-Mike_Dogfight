using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Squad : MonoBehaviour
{
    private float timer = 0;
    private float destroyTime = 15;

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer > destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
