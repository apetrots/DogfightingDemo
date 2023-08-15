using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    
    public float FollowSpeed = 5.0f; 

    // pass 2
    Vector3 offset;

    void Start()
    {
        // pass 2:

        // set offset from target at the start
        offset = transform.position - target.position;        
    }


    void LateUpdate()
    {
        Vector3 targetPos = target.position + offset;

        transform.position = targetPos;
    }
}
