using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float bulletSpeed;

    public Vector3 endPos;
    private Vector3 startPos;

    private Rigidbody bulletRb;
    private PlayerStats playerStats;
    private PlayerController playerController;

    void Start()
    {
        bulletRb = GetComponent<Rigidbody>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        startPos = transform.position;
        endPos = (playerController.bulletVector);
    }

    void Update()
    {
        bulletRb.AddForce(endPos * bulletSpeed);

        if (Vector3.Distance(startPos, transform.position) > (playerStats.rangeAttack * 2))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
