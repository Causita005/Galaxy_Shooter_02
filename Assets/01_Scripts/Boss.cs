using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    float life = 20;
    public float maxlife = 20;

    public Transform leftPoint;
    public Transform rightPoint;
    public Transform downLeftPoint;
    public Transform downRightPoint;
    public Vector2 initialPosition = new Vector2(0, 4f);
    public float moveSpeed = 3f;
    private bool movingToInitialPosition = true;
    bool moving = false;

    public Transform firePoint;
    public Transform firePoint2;  // Nuevo firePoint
    public Bullet bulletPrefab;
    public float timeBtwShoot = 0.5f;
    float timer = 0;
    public float bulletspeed = 5f;
    public float damage = 1f;

    float timer2 = 0;
    float timeBtwShoot2 = 0.8f;

    int moveAll = 1;

    float phaseBoss = 1;

    bool isShootingDiagonally = true;
    float pauseTimer = 0f;
    float pauseDuration = 2f;

    bool moveBoss = true;
    float reachThreshold = 0.1f;

    Transform target;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;

        life = maxlife;
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    void Update()
    {
        if (phaseBoss > 0.75)
        {
            bulletspeed = 1f;
            MultipleDiagonalShots();
        }
        timer2 += Time.deltaTime;

        if (phaseBoss <= 0.75 && phaseBoss > 0.5)
        {
            MovingBoss();
            Shoot();
            bulletspeed = 2f;
        }
        if (phaseBoss <= 0.5 && phaseBoss >= 0.25)
        {
            MovingBoss();
            bulletspeed = 0.3f;
            if (timer2 >= timeBtwShoot2)
            {
                ShootRandomly();
                timer2 = 0;
            }
        }
        if (phaseBoss < 0.25)
        {
            moveSpeed = 6f;
            timeBtwShoot = 0.8f;
            bulletspeed = 4f;
            MoveAllDirections();
            RotateToTarget();
            Shoot();
        }
    }

    void ShootRandomly()
    {
        int numberOfShots = Random.Range(2, 4);
        float angleStep = 360f / numberOfShots;
        float angle = 0f;

        for (int i = 0; i < numberOfShots; i++)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            Bullet b1 = Instantiate(bulletPrefab, firePoint.position, rotation); // Disparo desde firePoint
            Bullet b2 = Instantiate(bulletPrefab, firePoint2.position, rotation); // Disparo desde firePoint2

            b1.damage = damage;
            b1.GetComponent<Rigidbody2D>().velocity = rotation * Vector2.up * bulletspeed;

            b2.damage = damage;
            b2.GetComponent<Rigidbody2D>().velocity = rotation * Vector2.up * bulletspeed;

            angle += angleStep;
        }
    }

    void MultipleDiagonalShots()
    {
        if (isShootingDiagonally)
        {
            ShootDiagonally();
            isShootingDiagonally = false;
            pauseTimer = 0f;
        }
        else
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= pauseDuration)
            {
                isShootingDiagonally = true;
            }
        }
    }

    void ShootDiagonally()
    {
        int numberOfShots = Random.Range(1, 4);

        for (int i = 0; i < numberOfShots; i++)
        {
            float randomAngle = Random.Range(100f, 250f);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, randomAngle));

            Bullet b1 = Instantiate(bulletPrefab, firePoint.position, rotation); // Disparo desde firePoint
            Bullet b2 = Instantiate(bulletPrefab, firePoint2.position, rotation); // Disparo desde firePoint2

            b1.damage = damage;
            b1.GetComponent<Rigidbody2D>().velocity = rotation * Vector2.up * bulletspeed;

            b2.damage = damage;
            b2.GetComponent<Rigidbody2D>().velocity = rotation * Vector2.up * bulletspeed;
        }
    }

    void RotateToTarget()
    {
        Vector2 dir = target.position - transform.position;
        float angleZ = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + 0;
        transform.rotation = Quaternion.Euler(0, 0, -angleZ);
    }

    void MovingBoss()
    {
        if (movingToInitialPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, initialPosition) < 0.1f)
            {
                movingToInitialPosition = false;
            }
        }
        else
        {
            MoveLeftRight();
        }
    }

    void MoveLeftRight()
    {
        if (moving)
        {
            transform.position = Vector2.MoveTowards(transform.position, rightPoint.position, moveSpeed * Time.deltaTime);
            if (transform.position.x >= rightPoint.position.x)
            {
                moving = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, leftPoint.position, moveSpeed * Time.deltaTime);
            if (transform.position.x <= leftPoint.position.x)
            {
                moving = true;
            }
        }
    }

    void Shoot()
    {
        if (timer < timeBtwShoot)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;

            Bullet b1 = Instantiate(bulletPrefab, firePoint.position, transform.rotation); // Disparo desde firePoint
            Bullet b2 = Instantiate(bulletPrefab, firePoint2.position, transform.rotation); // Disparo desde firePoint2

            b1.damage = damage;
            b1.speed = bulletspeed;

            b2.damage = damage;
            b2.speed = bulletspeed;
        }
    }

    public void TakeDamage(float damage)
    {
        life -= damage;
        phaseBoss = life / maxlife;
        Debug.Log("Vida del Boss: " + life);
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    void MoveAllDirections()
    {
        if (moveAll == 2)
        {
            transform.position = Vector2.MoveTowards(transform.position, rightPoint.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, rightPoint.position) < reachThreshold)
            {
                if (moveBoss == true)
                {
                    moveAll = 3;
                }
                else
                {
                    moveAll--;
                }
            }
        }
        else if (moveAll == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, downRightPoint.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, downRightPoint.position) < reachThreshold)
            {
                if (moveBoss == true)
                {
                    moveAll = 2;
                }
                else
                {
                    moveAll = 1;
                    moveBoss = true;
                }
            }
        }
        else if (moveAll == 4)
        {
            transform.position = Vector2.MoveTowards(transform.position, downLeftPoint.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, downLeftPoint.position) < reachThreshold)
            {
                if (moveBoss == true)
                {
                    moveAll = 3;
                    moveBoss = false;
                }
            }
        }
        else if (moveAll == 3)
        {
            transform.position = Vector2.MoveTowards(transform.position, leftPoint.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, leftPoint.position) < reachThreshold)
            {
                if (moveBoss == true)
                {
                    moveAll = 4;
                }
                else
                {
                    moveAll--;
                }
            }
        }
    }
}
