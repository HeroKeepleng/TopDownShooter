using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour
{
    public GameObject coinPrefab;
    public Slider enemyHPBar;

    public float countOfRays;
    public float rayLength;
    public float rayAngle;

    public float speed;
    public float angleSpeed;
    public float rangeToAttack;
    public float maxHealth;
    private float currentHealth;

    public Vector3[] patrolPoint;
    private Vector3 targetPatrolPos;
    private float distanceToPlayer;
    private GameObject player;
    private PlayerStats playerStats;

    public bool canAttack;
    public bool followPlayer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
        targetPatrolPos = patrolPoint[0];
        canAttack = true;
        enemyHPBar.maxValue = maxHealth;
        currentHealth = maxHealth;
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < rangeToAttack && followPlayer)
        {
            StartCoroutine(AttackPlayer());
            FollowPlayerRotate();
        }
        else
        {
            RaycastCone();
            PatrolMove();
            FollowPlayer();
        }

        if (currentHealth < 1)
        {
            Instantiate(coinPrefab, transform.position, coinPrefab.transform.rotation);
            Destroy(gameObject);
        }

        enemyHPBar.value = currentHealth;
    }

    void RaycastCone()
    {
        Vector3 directionForward = transform.forward;
        Vector3 directionRight = transform.forward;
        Vector3 directionLeft = transform.forward;
        Vector3 origin = transform.position;

        for (int i = 0; i < countOfRays; i++)
        {

            directionRight = Quaternion.Euler(0, rayAngle, 0) * directionRight;
            directionLeft = Quaternion.Euler(0, -rayAngle, 0) * directionLeft;

            RaycastHit hit;

            //Check Forward
            if (Physics.Raycast(origin, directionForward, out hit, rayLength))
            {
                Debug.DrawLine(origin, hit.point, Color.red);
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    followPlayer = true;
                }
            }else Debug.DrawLine(origin, origin + directionForward * rayLength, Color.green);

            //Check Right
            if (Physics.Raycast(origin, directionRight, out hit, rayLength))
            {
                Debug.DrawLine(origin, hit.point, Color.red);
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    followPlayer = true;
                }
            }else Debug.DrawLine(origin, origin + directionRight * rayLength, Color.green);

            //CheckLeft
            if (Physics.Raycast(origin, directionLeft, out hit, rayLength))
            {
                Debug.DrawLine(origin, hit.point, Color.red);
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    followPlayer = true;
                }
            }
            else Debug.DrawLine(origin, origin + directionLeft * rayLength, Color.green);
        }
    }

    private void FollowPlayer()
    {
        if (followPlayer)
        {
            //Follow
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
            FollowPlayerRotate();
        }
    }

    private void FollowPlayerRotate()
    {
        //Rotate
        float step = angleSpeed * Time.deltaTime;
        Vector3 targetRotation = (player.transform.position - transform.position);
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetRotation, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection, Vector3.up);
    }

    private void PatrolMove()
    {
        if (!followPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPatrolPos, Time.deltaTime * speed);
            transform.LookAt(targetPatrolPos);

            if (transform.position == patrolPoint[0])
            {
                targetPatrolPos = patrolPoint[1];
            }

            if (transform.position == patrolPoint[1])
            {
                targetPatrolPos = patrolPoint[0];
            }
        }
    }

    private IEnumerator AttackPlayer()
    {
        while (canAttack && distanceToPlayer < rangeToAttack)
        {
            canAttack = false;
            playerStats.currentHealth--;
            yield return new WaitForSeconds(2f);
            canAttack = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            currentHealth--;
            Destroy(other.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, rangeToAttack);
    }
}
