using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public float lifeTime = 2f, timer;
    public GameObject impactEffect;

    private void OnEnable()
    {
        timer = lifeTime;
    }

    private void Update()
    {
        transform.Translate(-transform.forward * speed * Time.deltaTime);
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            DestroyBullet();
        }
    }

    private void DestroyBullet(bool impact = false)
    {
        if (impact)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }
        BulletPool.Instance.ReturnBullet(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().Hurt(damage);

        }
        if (other.tag != "Enemy")
        {
            DestroyBullet(true);
        }
    }


}
