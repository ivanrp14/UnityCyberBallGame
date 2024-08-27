using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    Damage damage;
    public GameObject explosionEffect;

    private void Start()
    {
        damage = GetComponent<Damage>();
        damage.OnHit += Destroy;
    }
    void Destroy()
    {
        BulletPool.Instance.ReturnBullet(GetComponent<Bullet>());
        GameObject ps = Instantiate(explosionEffect, transform.position + new Vector3(0f, 0.5f, 1f), transform.rotation);
        ps.SetActive(true);
    }
}
