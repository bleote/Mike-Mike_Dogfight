using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RPMController : MonoBehaviour
{
    [SerializeField]
    private Image needle;
    private float currentSpeed = 240;
    private float targetSpeed = 240;
    private float needleSpeed = 50;

    [SerializeField]
    private GameObject player;

    private void Update()
    {
        if (PlayerController.gameOn)
        {
            UpdateSpeed();
        }

        SetNeedle();
    }

    private void UpdateSpeed()
    {
        float acceleration = Input.GetAxis("Vertical");

        if (acceleration > 0)
        {
            targetSpeed = 280;
        }
        else if (acceleration < 0)
        {
            targetSpeed = 200;
        }
        else
        {
            targetSpeed = 240;
        }

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * needleSpeed);
    }

    private void SetNeedle()
    {
        float minSpeed = 200;
        float maxSpeed = 280;
        float angleOffset;
        if (PlayerController.pause == true)
        {
            angleOffset = 0;
        }
        else
        {
            angleOffset = Random.Range(-0.02f, 0.02f);
        }
        

        float angle = Mathf.Lerp(-200, -280, (currentSpeed - minSpeed) / (maxSpeed - minSpeed) + angleOffset);

        needle.transform.localEulerAngles = new Vector3(0, 0, angle);
    }
}