using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedShotArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject otherGO = collision.gameObject;
        if (otherGO.CompareTag("PlayerBullet"))
        {
            GameManager.missedShots++;
        }

        Destroy(otherGO);
    }
}
