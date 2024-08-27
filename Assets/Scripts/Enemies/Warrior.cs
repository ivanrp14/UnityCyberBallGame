using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour, IEnemy
{

    public enum FireType
    {
        Automatic,
        Burst,
        Single
    }
    public float initialHealth = 100f;
    public float initialDamage = 10f;
    public FireType fireType;

    private Health healthComponent;
    private Damage damageComponent;
    public float distanceToPlayer = 50f;

    public bool isActivated = false;
    private Animator animator;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    [SerializeField] private Transform player;

    // Start is called before the first frame update
    void OnEnable()
    {

        // Añadir el componente Health si no existe ya
        healthComponent = gameObject.GetComponent<Health>();
        damageComponent = gameObject.GetComponent<Damage>();
        if (healthComponent == null)
        {
            healthComponent = gameObject.AddComponent<Health>();
        }
        if (damageComponent == null)
        {
            damageComponent = gameObject.AddComponent<Damage>();
        }
        damageComponent.damage = initialDamage; // Configurar el daño inicial
        damageComponent.targetTag = "Player";


        // Configurar el componente Health
        healthComponent.SetMaxHealth(initialHealth);
        healthComponent.OnZeroLifes += Dead;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= distanceToPlayer && !isActivated && healthComponent.GetCurrentHealth() > 0)
        {
            isActivated = true;
            animator.SetBool(fireType.ToString(), true);

        }
        else if (player != null && Vector3.Distance(transform.position, player.position) > distanceToPlayer && isActivated)
        {
            isActivated = false;
            animator.SetBool(fireType.ToString(), false);
        }
        if (healthComponent.GetCurrentHealth() <= 0)
        {

            animator.SetBool(fireType.ToString(), false);
            animator.SetTrigger("Dead");
        }
    }


    public void TakeDamage(float amount)
    {
        // Aplicar daño usando el componente Health
        healthComponent?.Hurt(amount);
    }

    public float GetHealth()
    {
        return healthComponent != null ? healthComponent.GetCurrentHealth() : 0f;
    }

    // Lógica de disparo basada en el tipo de disparo seleccionado
    public void Shoot()
    {
        switch (fireType)
        {
            case FireType.Automatic:
                FireAutomatic();
                break;
            case FireType.Burst:
                FireBurst();
                break;
            case FireType.Single:
                FireSingle();
                break;
        }
    }

    private void FireAutomatic()
    {
        // Implementar lógica de disparo automático
        Debug.Log("Disparo automático");
    }

    private void FireBurst()
    {
        // Implementar lógica de disparo en ráfaga
        Debug.Log("Disparo en ráfaga");
    }

    private void FireSingle()
    {
        Bullet bullet = BulletPool.Instance.GetBullet();


        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.Euler(firePoint.rotation.eulerAngles + Vector3.right * 90);

    }
    private void Dead()
    {
        animator.enabled = false;
        isActivated = false;
    }


}
