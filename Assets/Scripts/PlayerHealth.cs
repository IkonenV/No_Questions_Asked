using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;

    public GameObject healthIcon1;
    public GameObject healthIcon2;
    public GameObject healthIcon3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        Debug.Log("Kuolit");
    }

}
