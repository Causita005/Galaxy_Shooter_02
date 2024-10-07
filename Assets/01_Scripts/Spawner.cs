using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public float timeBtwSpawn = 1.5f;
    float timer = 0;
    public Transform leftpoint;
    public Transform rightpoint;
    public List<GameObject> EnemyPrefab;
    public GameObject Boss;

    public Text killedEnemiesText;
    public Text phaseText;
    public int phase = 1;
    public int killerdEnemies = 0; 
    public static Spawner instance;

    // Configuraciones para modificar la dificultad
    public float timeBtwSpawnReduction = 0.5f;
    public float minTimeBtwSpawn = 0.5f; 

    private bool bossSpawned = false; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        killedEnemiesText.text = "Score: " + killerdEnemies;
        phaseText.text = "Oleada: " + phase++;
    }

    void Update()
    {
        if (phase < 4)
        {
            SpawnEnemy();
        }
        else  
        {
            SpawnBossWithChance();
        }
    }

    void SpawnEnemy()
    {
        if (timer < timeBtwSpawn)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            float x = Random.Range(leftpoint.position.x, rightpoint.position.x);
            int enemy = Random.Range(0, EnemyPrefab.Count);
            Vector3 newpost = new Vector3(x, transform.position.y, 0);
            Instantiate(EnemyPrefab[enemy], newpost, Quaternion.Euler(0, 0, 180)).tag = "Enemy";
        }
    }

    void SpawnBossWithChance()
    {
        if (bossSpawned) return;

        float x = Random.Range(leftpoint.position.x, rightpoint.position.x);
        Vector3 newpostBoss = new Vector3(x, transform.position.y, 0);
        Instantiate(Boss, newpostBoss, Quaternion.Euler(0, 0, 180)).tag = "Boss";

        bossSpawned = true; 
    }

    public void AddKilledEnemy(int points)
    {
        killerdEnemies += points;
        killedEnemiesText.text = "Score: " + killerdEnemies;

        if (killerdEnemies == 10) 
        {
            phaseText.text = "Oleada: " + phase++;
            killerdEnemies = 0;
            RemoveAllEnemies();
            ResetSpawner();
            killedEnemiesText.text = "Score: " + killerdEnemies;
        }
    }

    void RemoveAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    void ResetSpawner()
    {
        timer = 0;
        if (timeBtwSpawn > minTimeBtwSpawn)
        {
            timeBtwSpawn -= timeBtwSpawnReduction;
        }
    }
}
