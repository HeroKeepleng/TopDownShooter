using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject loseScreen;
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < 1)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0f;
        }

        if (playerStats.currentHealth < 1)
        {
            loseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
