using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject[] explosionEffect;
    public GameObject[] destroyEffect;
    public GameObject[] hurtEffect;
    public bool fade = false;
    FadeAndDestroy fadeAndDestroy;
    private Health health;
    public float delay = 0f;
    void Start()
    {
        TryGetComponent<Health>(out health);
        if (health != null)
        {
            health.OnZeroLifes += Dead;
            health.OnChangeHealth += Hurt;
        }
        if (fade)
        {
            fadeAndDestroy = TryGetComponent<FadeAndDestroy>(out fadeAndDestroy) ? fadeAndDestroy : gameObject.AddComponent<FadeAndDestroy>();
            fadeAndDestroy.fade = true;
            fadeAndDestroy.destroyOnStart = false;
        }
    }
    void Explode()
    {
        if (explosionEffect.Length == 0)
        {


            return;
        }
        foreach (GameObject effect in explosionEffect)
        {
            GameObject ps = Instantiate(effect, transform.position + new Vector3(0f, 0.5f, 1f), transform.rotation);
            ps.SetActive(true);
        }
    }
    void Die()
    {
        if (destroyEffect.Length == 0)
        {


            return;
        }
        foreach (GameObject effect in destroyEffect)
        {
            GameObject ps = Instantiate(effect, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            ps.SetActive(true);
        }
    }
    void Hurt(int damage)
    {
        if (damage >= 0)
        {
            return;
        }
        if (hurtEffect.Length == 0)
        {


            return;
        }
        foreach (GameObject effect in hurtEffect)
        {
            GameObject ps = Instantiate(effect, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            ps.SetActive(true);
        }
    }
    void Dead()
    {
        Explode();
        if (delay > 0)
        {
            StartCoroutine(DestroyObject());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(delay);
        Die();
        Destroy(gameObject);
    }
}
