using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float Duration = 5.0f;

    float destroyTimer;

    void Start()
    {
        destroyTimer = Duration;
    }

    void Update()
    {
        if (destroyTimer <= 0.0f)
        {
            Destroy(this.gameObject);
        }
        else
        {
            destroyTimer -= Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
