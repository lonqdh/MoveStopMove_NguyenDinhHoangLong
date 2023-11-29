using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : Singleton<CameraFollow>
{
    public Transform target; // The character's transform
    public float smoothSpeed = 5.0f;
    public Vector3 offset;
    Vector3 desiredPosition;
    Vector3 smoothedPosition;


    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            desiredPosition = target.position + offset;
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
            transform.LookAt(target);
        }
        
    }
}
