using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    public float rotateSpeed;

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
