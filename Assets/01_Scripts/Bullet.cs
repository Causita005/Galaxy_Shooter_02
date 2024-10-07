using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour //herencia para usar todos los metodos el Mono
{
    public float speed = 5f;
    public float timeToDestroy = 4f;
    public float damage = 1;
    public bool playerBullet =false;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up *speed *Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && playerBullet)
        {
            Enemy e= collision.gameObject.GetComponent<Enemy>();
            e.TakeDamage(damage);
            Destroy(gameObject);
        }else if(collision.gameObject.CompareTag("Player") && !playerBullet)
        {
            Player p = collision.gameObject.GetComponent<Player>();
            p.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss") && playerBullet)
        {
            Boss p = collision.gameObject.GetComponent<Boss>();
            p.TakeDamage(damage);
            Destroy(gameObject);
        }

    }
}
