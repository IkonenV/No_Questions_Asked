using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health;

    public GameObject healthIcon1;
    public GameObject healthIcon2;
    public GameObject healthIcon3;
    public GameObject deathScreen;
    public PlayerShooting playerShooting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(health == 3)
        {
            healthIcon1.SetActive(true);
            healthIcon3.SetActive(true);
            healthIcon2.SetActive(true);
        }
        if(health == 2)
        {
            healthIcon1.SetActive(true);
            healthIcon2.SetActive(true);
            healthIcon3.SetActive(false);
        }
        if(health == 1)
        {
            healthIcon1.SetActive(true);
            healthIcon2.SetActive(false);
            healthIcon3.SetActive(false);
        }
    }
    public void TakeDamage(float amount)
    {
        health -= 1;
        if (health <= 0)
        {
            Death();
        }
    }
    public void Heal()
    {
        if(health < 3)
        {
            health += 1;
        }
    }
    public void Death()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0;
        playerShooting.ScoreCounting();
    }
    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
