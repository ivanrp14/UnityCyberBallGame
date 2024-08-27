using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage = 1.0f;
    public string targetTag;
    public event Action OnHit = delegate { };
    public bool ignoreInvulnerable = false;
    public bool destroyOnHit = false;

    public bool canDoDamage = true;

    void Start()
    {
        // Check if script is enabled
        if (!enabled)
        {
            Debug.LogError("Damage script is disabled on " + gameObject.name);
        }
    }

    public void DoDamage(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<Health>(out Health hs) && canDoDamage)
        {
            Debug.Log("Applying " + damage + " damage to " + gameObject.name);
            if (ignoreInvulnerable)
            {
                hs.Hurt(damage, true);
            }
            else
                hs.Hurt(damage);
            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            OnHit.Invoke();
            DoDamage(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            OnHit.Invoke();
            DoDamage(other.gameObject);
        }
    }
}
