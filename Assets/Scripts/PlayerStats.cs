using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public GameObject pistol;
    public GameObject gun;
    public Slider playerHPBar;
    public TMP_Text coinTxt;

    public float coin;

    public float speed;
    public float rangeAttack;
    public float maxHealth;
    public float currentHealth;
    public float pistolCooldown;
    public float gunCooldown;
    public float shootCooldown;

    public bool isPistol;
    public bool isGun;

    public bool isKey;

    private Animator animator;
    private GameObject activeWeapon;

    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        activeWeapon = gun;
        shootCooldown = gunCooldown;
        gun.SetActive(true);
        isGun = true;
        currentHealth = maxHealth;
        playerHPBar.maxValue = maxHealth;

    }

    private void Update()
    {
        playerHPBar.value = currentHealth;
        coinTxt.text = coin.ToString("F0");
    }

    public void ChangeWeapon()
    {
        animator.Play("ChangeWeapon");
        if (activeWeapon == gun)
        {
            activeWeapon = pistol;
            shootCooldown = pistolCooldown;
            pistol.SetActive(true);
            gun.SetActive(false);

            isPistol = true;
            isGun = false;
        }
        else if (activeWeapon == pistol)
        {
            activeWeapon = gun;
            shootCooldown = gunCooldown;
            gun.SetActive(true);
            pistol.SetActive(false);

            isPistol = false;
            isGun = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            isKey = true;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Coin"))
        {
            coin += 10;
            Destroy(other.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, rangeAttack);
    }
}
