using TMPro;
using UnityEngine;

public class Supply : MonoBehaviour
{
    private Rigidbody2D supply;
    private float speed = 1f;

    void Awake()
    {
        supply = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector3 newPos = transform.position + speed * Time.deltaTime * Vector3.down;

        supply.MovePosition(newPos);

        if (transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }
}
