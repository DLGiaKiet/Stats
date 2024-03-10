using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float SmoothSpeed = 0.125f;
    public Vector3 Offset;
    private void FixedUpdate()
    {
        Vector3 desiredPosi = target.position + Offset;
        Vector3 smoothPosi = Vector3.Lerp(transform.position, desiredPosi, SmoothSpeed);
        transform.position = smoothPosi;
    }
}
