using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //para la interfaz de canvas para que funcione

public class Enemy : MonoBehaviour
{
    public EnemyType type;
    float life = 3; //
    public float maxlife = 3; //agregado de la vida total del enemigo
    public float speed = 0.5f;
    public float timeBtwShoot = 1.5f;
    float timer = 0;
    public float range = 4;
    public float damage = 1f;
    public float powerup = 1;
    bool targetInRange = false;
    Transform target;
    public Transform firePoint;
    public Bullet bulletPrefab;
    public float bulletspeed = 5f;
    public List<GameObject> powerUpPrefabs;

    public GameObject deathParticlesPrefab;


    public Image lifeBar;

    public int scorePoints = 1;

    void Start()
    {
        GameObject ship = GameObject.FindGameObjectWithTag("Player");
        target = ship.transform;
        life=maxlife;
        lifeBar.fillAmount = life / maxlife;
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case EnemyType.Normal:
                MoveForward();
                break;
            case EnemyType.NormalShoot:
                MoveForward();
                Shoot();
                break;
            case EnemyType.Kamikase:
                if (targetInRange)
                {
                    RotateToTarget();
                    MoveForward(2);
                }
                else
                {
                    MoveForward();
                    SearchTarget();
                }
                break;
            case EnemyType.Sniper:
                if (targetInRange)
                {
                    RotateToTarget();
                    Shoot();
                }
                else
                {
                    MoveForward();
                    SearchTarget();
                }
                break;
        }
    }
    public void TakeDamage(float damage)
    {
        life-=damage;
        lifeBar.fillAmount = life / maxlife;
        if (life <= 0)
        {
            
            float chance = Random.Range(0f, 100f);

            // 50% de probabilidad de spawnear un PowerUp
            if (chance <= 50f )
            {
                // Selecciona un PowerUp aleatorio de la lista
                int randomIndex = Random.Range(0, powerUpPrefabs.Count);
                Instantiate(powerUpPrefabs[randomIndex], transform.position, Quaternion.identity);
            }
            Spawner.instance.AddKilledEnemy(scorePoints);

            Die();
        }
    }
    void Die()
    {
        GameObject deathParticles = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        ParticleSystem ps = deathParticles.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            ps.Play();  
        }

        Destroy(gameObject);
        Destroy(deathParticles, ps.main.duration);
    }


    void MoveForward()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    void MoveForward(float m)
    {
        transform.Translate(Vector2.up * speed*m * Time.deltaTime);
    }
    void RotateToTarget()
    {
        Vector2 dir=target.position-transform.position;
        float angelZ= Mathf.Atan2 (dir.x, dir.y) * Mathf.Rad2Deg+0;
        transform.rotation = Quaternion.Euler(0,0,-angelZ);
    }
    void SearchTarget()
    {
        //if (target == null) return; Rotate
        float distance=Vector2.Distance(transform.position,target.position);
        if (distance <= range)
        {
            targetInRange=true;
        }
        else
        {
            targetInRange = false;
        }
    }
    void Shoot()
    {
        if (timer<timeBtwShoot)
        {
            timer += Time.deltaTime;       
        }
        else
        {
            timer = 0;
            Bullet b = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            b.damage = damage;
            b.speed = bulletspeed;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
          if (collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.TakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            Die();
        }
    }
    public enum EnemyType
    {
        Normal,
        NormalShoot,
        Kamikase,
        Sniper
    }
}
