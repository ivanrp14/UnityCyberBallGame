using System;
using UnityEngine;
using System.Collections;
public class Health : MonoBehaviour
{
    [SerializeField]
    private float maxhealth = 100f;

    [SerializeField]
    private float health;

    public event Action<int> OnChangeHealth = delegate { };
    public event Action OnChangeHealthRespawn = delegate { };
    public event Action OnZeroLifes = delegate { };

    public static event Action OnZeroLifesDeath = delegate { };

    public event Action Respawn = delegate { };
    public bool canTakeDamage = true;
    [Header("Animation")]
    public bool isAnimating = false;
    [SerializeField]
    private float damageCooldownDuration = 2f; // Duración del estado de daño (en segundos)
    [SerializeField]
    private float blinkInterval = 0.1f; // Intervalo de parpadeo (en segundos)

    private Renderer[] renderers;

    private void Start()
    {
        health = maxhealth;
        OnChangeHealth.Invoke(Mathf.RoundToInt(health));
        renderers = GetComponentsInChildren<Renderer>();
    }

    public float GetMaxHealth() => maxhealth;

    public float GetCurrentHealth() => health;

    public void Hurt(float damage, bool ignoreInvulnerability = false)
    {
        if (!canTakeDamage && !ignoreInvulnerability) return;
        health -= damage;
        health = Mathf.Max(health, 0);
        OnChangeHealth.Invoke(Mathf.RoundToInt(-damage));
        if (health <= 0)
        {
            OnZeroLifes.Invoke();
        }
        else
        {
            PlayDamageAnimation();
        }
    }


    public void Heal(float heal)
    {
        health += heal;
        if (health > maxhealth)
            health = maxhealth;
        OnChangeHealth.Invoke(Mathf.RoundToInt(health));
    }

    public void SetMaxHealth(float maxh)
    {
        maxhealth = maxh;
    }

    public void SetHealth(float h)
    {
        health = h;
        OnChangeHealth.Invoke(Mathf.RoundToInt(health));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Hurt(10);
        }
    }
    public void PlayDamageAnimation()
    {
        if (!isAnimating && renderers.Length > 0)
        {
            StartCoroutine(DamageCooldown());
        }
    }

    private IEnumerator DamageCooldown()
    {

        float elapsedTime = 0f;
        canTakeDamage = false;

        while (elapsedTime < damageCooldownDuration)
        {
            foreach (Renderer renderer in renderers)
            {
                if (renderer != null)
                {
                    renderer.enabled = !renderer.enabled; // Parpadeo de cada renderer
                }
            }

            elapsedTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        // Asegúrate de que todos los renderers sean visibles al final
        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }
        canTakeDamage = true;

    }


}
