using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MoveSpeed = 0.5f;

    void Update()
    {
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");

    // 1st pass
        // Vector3 newPos = transform.position;
        // newPos.x += xDir * MoveSpeed;
        // newPos.y += yDir * MoveSpeed;
        // transform.position = newPos;

    // 2nd pass: condense
        // transform.position += new Vector3(xDir * MoveSpeed, yDir * MoveSpeed, 0.0f);

    // 3rd pass: delta time
        transform.position += new Vector3(xDir * MoveSpeed * Time.deltaTime, yDir * MoveSpeed * Time.deltaTime, 0.0f);

    // 4th pass: condense and explain vector math, vector * scalar is element-wise
        transform.position += new Vector3(xDir, yDir, 0.0f) * MoveSpeed * Time.deltaTime;


    }
}
