using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public float amount = 0.5f;
    public float speed = 1f;
    public float timeToDestroy = 7f;

    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.ApplyPowerUp(type, amount);
            Destroy(gameObject);
        }
    }
}

public enum PowerUpType
{
    Damage,
    MoveSpeed,
    BulletSpeed,
    CriticalRate,
    FireRate,
    Shield
}
