using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    public float mapSpeed = 4;
    private Vector3 startPosition;

    void Awake()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (PlayerController.gameOn)
        {
            transform.Translate(translation: mapSpeed * Time.deltaTime * Vector3.down);
            if (transform.position.y < -74.49f)
            {
                transform.position = startPosition;
            }
        }

    }
}
