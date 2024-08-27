using UnityEngine;

public abstract class Hability : MonoBehaviour
{
    public string habilityName;
    public float cooldown;
    public int level;
    public AnimationCurve priceCurve;
    public AnimationCurve augmentCurve;

    private float lastUsedTime;
    protected RobotoController controller;  // Usar protected para que la clase derivada pueda acceder
    private int price;

    // El método Start será llamado automáticamente por Unity
    protected virtual void Start()  // Usar virtual para que pueda ser sobrescrito
    {
        price = Mathf.RoundToInt(priceCurve.Evaluate(level));
        controller = GetComponent<RobotoController>();

    }
    protected virtual void Update()
    {

    }

    public bool CanUse()
    {
        return Time.time >= lastUsedTime + cooldown && CheckConditions();
    }

    public void Use()
    {
        if (CanUse())
        {
            ExecuteHability();
            lastUsedTime = Time.time;
        }
    }

    public abstract void ExecuteHability();

    protected bool CheckConditions()
    {
        // Implementa la lógica para verificar las condiciones de uso si es necesario
        return true;
    }

    public void Upgrade()
    {
        level++;
        price = Mathf.RoundToInt(priceCurve.Evaluate(level));
        ApplyAugment(augmentCurve.Evaluate(level));
    }

    public abstract void ApplyAugment(float augmentValue);

    public float GetCooldownPercentage()
    {
        float timeSinceLastUse = Time.time - lastUsedTime;
        return Mathf.Clamp01(timeSinceLastUse / cooldown);
    }

    public int GetPrice()
    {
        return price;
    }
}
