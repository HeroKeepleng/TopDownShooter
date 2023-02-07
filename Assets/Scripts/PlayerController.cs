using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Joystick joystick;
    public GameObject bulletPrefab;

    public GameObject nearestEnemy;

    private bool canMove;
    public bool canFindEnemy;
    public bool canShoot;
    public bool isDelay;

    private Vector3 moveVector;
    public Vector3 bulletVector;
    private CharacterController characterController;
    private EnemyControl enemyControl;
    private PlayerStats playerStats;
    private Animator animator;

    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
    }

    void FixedUpdate()
    {
        MovePlayer();

        StartCoroutine(FindNearestEnemy());
        StartCoroutine(ShootPlayer());

        if (!canMove)
        {

            if (nearestEnemy != null)
            {
                LookNearestEnemy();
            }
        }
        animator.SetBool("Move", canMove);

    }

    private IEnumerator FindNearestEnemy()
    {
        while (canFindEnemy && !canMove)
        {
            canFindEnemy = false;
            nearestEnemy = null;
            float enemyOnScene = GameObject.FindGameObjectsWithTag("Enemy").Length;

            for (int i = 0; i < enemyOnScene; i++)
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectsWithTag("Enemy")[i].transform.position) < playerStats.rangeAttack)
                {
                    nearestEnemy = GameObject.FindGameObjectsWithTag("Enemy")[i];
                    enemyControl = nearestEnemy.GetComponent<EnemyControl>();
                    enemyControl.followPlayer = true;
                    canShoot = true;
                }else if (nearestEnemy == null)
                {
                    canShoot = false;
                }
            }

            yield return new WaitForSeconds(playerStats.shootCooldown);
            canFindEnemy = true;
        }
    }

    private IEnumerator ShootPlayer()
    {
        while (!isDelay && canShoot && !canMove)
        {
            animator.SetBool("Shoot", true);
            isDelay = true;
            canShoot = false;

            Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
            bulletVector = (nearestEnemy.transform.position - transform.position).normalized;
            yield return new WaitForSeconds(playerStats.shootCooldown);
            animator.SetBool("Shoot", false);
            isDelay = false;
        }
    }

    private void MovePlayer()
    {
        //PlayerMove
        moveVector = Vector3.zero;
        moveVector.x = joystick.Horizontal * playerStats.speed;
        moveVector.z = joystick.Vertical * playerStats.speed;

        characterController.Move(moveVector * Time.deltaTime);

        Vector3 direct = Vector3.RotateTowards(transform.forward, moveVector, playerStats.speed, 0.0f);
        transform.rotation = Quaternion.LookRotation(direct);

        canMove = moveVector != Vector3.zero;

    }
    private void LookNearestEnemy()
    {
        Vector3 targetRotation = (nearestEnemy.transform.position - transform.position);
        Vector3 direct = Vector3.RotateTowards(transform.forward, targetRotation, playerStats.speed, 0.0f);
        transform.rotation = Quaternion.LookRotation(direct);
    }
}
