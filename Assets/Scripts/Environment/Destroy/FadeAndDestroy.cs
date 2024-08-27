using UnityEngine;
using System.Collections;

public class FadeAndDestroy : MonoBehaviour
{
    public float fadeDuration = 2.0f; // Duración del fade (en segundos)
    public float delayBeforeFade = 0f; // Tiempo de espera antes de iniciar el fade (opcional)
    private Material objectMaterial;
    private Color originalColor;
    private bool isFading = false;
    public bool fade = true;
    public bool destroyOnStart = false;
    Collider collider;
    Health health;

    void Start()
    {
        TryGetComponent<Health>(out health);
        if (health != null)
        {
            health.OnZeroLifes += StartFadeAndDestroy;
        }
        // Obtener el material del objeto
        collider = GetComponent<Collider>();
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogWarning("No Renderer component found on this object.");
            return;
        }

        // Asegurarse de que el material no se comparta entre objetos
        objectMaterial = renderer.material;
        originalColor = objectMaterial.color;

        // Iniciar el fade si está configurado para hacerlo al iniciar
        if (destroyOnStart)
        {
            StartFadeAndDestroy();
        }
    }

    public void StartFadeAndDestroy()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        // Esperar el tiempo opcional antes de iniciar el fade
        collider.enabled = false;
        yield return new WaitForSeconds(delayBeforeFade);

        float elapsedTime = 0f;
        isFading = true;

        // Realizar el fade
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            // Actualizar el color del material con la nueva transparencia
            objectMaterial.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null; // Esperar al siguiente frame
        }

        // Asegurarse de que el objeto sea completamente transparente
        objectMaterial.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // Destruir el objeto
        Destroy(gameObject);
    }
}
