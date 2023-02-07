using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset = new(0, 25, -15);

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
