using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;    //PARA MANEJAR LOS DISTINTOS COMPONENTES
using UnityEngine.SceneManagement; //RECARGA DE LA ESCENA

public class Player : MonoBehaviour
{
    public float damage = 1f;
    public float speed = 4f;
    public float bulletspeed = 5f;
    public Transform firePoint;
    public Bullet bulletPrefab;
    public Rigidbody2D rb;
    float cargadores = 0;
    public float timeBtwShoot = 0.5f;
    float timer = 0;
    float life = 5;

    public float criticalChance = 0f;
    public bool shield = false;
    public float shieldTime = 5f;

    public Text lifetext;
    public Image lifebar; 
    public float maxLife = 5;

    bool isDead = false;
    float deathTimer = 0f; 
    float deathDelay = 2f;

    void Start()
    {
        Debug.Log("Inicio del juego");
        lifetext.text = "Life = " + life;
        lifebar.fillAmount = life / maxLife;
        life = maxLife;
    }

    void Update()
    {
        if (!isDead)
        {
            Shoot();
            Reload();
            Movement();
        }
        else
        {
            deathTimer += Time.deltaTime;
            if (deathTimer >= deathDelay)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }
    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(x, y) * speed;
    }
    void Shoot()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && timer >= timeBtwShoot && cargadores < 5)
        {
            Bullet b = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            if (UnityEngine.Random.Range(0f, 1f) <= criticalChance)
            {
                b.damage = damage * 2;
            }
            else
            {
                b.damage = damage;
            }
            b.speed = bulletspeed;
            timer = 0;
            cargadores++;
        }
    }
    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            cargadores = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            SceneManager.LoadScene("Game");
            Destroy(gameObject);
        }
    }
        public void TakeDamage(float damage)
    {
        life -= damage;
        
        if (!shield)
        {
            life -= damage;
            lifetext.text = "Life = " + life;
            lifebar.fillAmount = life / maxLife;
            Debug.Log("Daño recibido: " + damage + ". Vida restante: " + life);
            if (life <= 0)
            {
                
                // SceneManager.LoadScene("Game");
                isDead = true;
            }
        }
    }
    public void ApplyPowerUp(PowerUpType powerUp, float amount)
    {
        switch (powerUp)
        {
            case PowerUpType.Damage:
                damage += amount;
                break;
            case PowerUpType.MoveSpeed:
                speed += amount;
                break;
            case PowerUpType.BulletSpeed:
                bulletspeed += amount;
                break;
            case PowerUpType.CriticalRate:
                criticalChance = amount;//0.3f
                break;
            case PowerUpType.FireRate:
                timeBtwShoot -= amount;
                if (timeBtwShoot <= 0)
                {
                    timeBtwShoot = 0.1f;
                }
                break;
            case PowerUpType.Shield:
                StartCoroutine(ApplyShield());
                break;
        }
    }
    IEnumerator ApplyShield()
    {
        shield = true;
        yield return new WaitForSeconds(shieldTime);
        shield = false;
    }
}
