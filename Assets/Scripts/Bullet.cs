using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        PlayerShooting playerShooting = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShooting>();
        playerShooting.GetBullets();
        Destroy(gameObject);
    }
}
