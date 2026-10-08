using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform[] checkerSpawnPoints;
    public GameObject checkerPrefab;
    public GameObject healthPackPrefab;
    public float gameTimer;
    public int wave = 1;
    public float bulletTimer;
    public float bulletSpawnTime = 8;
    public float waveTime = 8;
    bool firstRound = true;
    public float healthTimer;
    public float healthTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameTimer+=Time.deltaTime;
        if(gameTimer >= waveTime || firstRound && gameTimer >= 5)
        {
            wave +=1;
            if(wave < 6)
            {
                SpawnChecker(1);
            }
            if (wave>5)
            {
                int a = Random.Range(1,3);
                SpawnChecker(a);
            }
            if(waveTime > 7)
            {
                waveTime = waveTime / 1.05f;
            }
            else
            {
                waveTime = waveTime / 1.01f;
            }
            gameTimer = 0;
            firstRound = false;
        }
        bulletTimer+=Time.deltaTime;
        if(bulletTimer > bulletSpawnTime)
        {
            SpawnBullet();
            bulletSpawnTime = Random.Range(8,12);
            bulletTimer = 0;
        }
        healthTimer+=Time.deltaTime;
        if(healthTimer > healthTime)
        {
            SpawnHealthPack();
            healthTime = Random.Range(15,20);
            healthTimer = 0;
        }
    }
    public void SpawnBullet()
    {
        float xPos = Random.Range(-33, -24);
        float zPos = Random.Range(-9, 0);
        Vector3 spawnPos = new Vector3(xPos,0.7f,zPos);
        Instantiate(bulletPrefab, spawnPos,Quaternion.identity);
    }
    public void SpawnChecker(int amount)
    {
        int i = 0;
        while (i < amount)
        {
        int index = 0;
        foreach (Transform spawnPoint in checkerSpawnPoints)
        {
            index +=1;
        }
        int spawnNumber = Random.Range(0, index);
        Instantiate(checkerPrefab, checkerSpawnPoints[spawnNumber].position, checkerSpawnPoints[spawnNumber].transform.rotation);
        
        i += 1;
        }
        
    }
    public void SpawnHealthPack()
    {
        float xPos = Random.Range(-33, -24);
        float zPos = Random.Range(-9, 0);
        Vector3 spawnPos = new Vector3(xPos,0.7f,zPos);
        Instantiate(healthPackPrefab, spawnPos,Quaternion.identity);
    }
}
