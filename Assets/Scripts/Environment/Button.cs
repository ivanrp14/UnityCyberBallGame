using System;
using UnityEngine;

public class ButtonActivator : MonoBehaviour, IActivable
{
    public enum ActivationType
    { Permanent, Temporary, Hold }
    public ActivationType activationType = ActivationType.Permanent;
    public bool isActivated = false; // Estado del botón, si está activado o no
    public LayerMask activationLayers; // Capas que activarán el botón (puedes asignar Player y objetos físicos)

    public float sinkAmount = 0.2f; // Cuánto se hunde el botón cuando se activa
    public Material activatedMaterial; // Material que se aplicará cuando el botón esté activado
    private Material originalMaterial; // Material original del botón

    private Vector3 originalPosition; // Posición original del botón
    private Renderer buttonRenderer; // El renderer del botón para cambiar su material

    [SerializeField] private float activatorTime = 5; // Número de objetos que están activando el botón (CharacterController o Rigidbodies)
    private float activatorTimer = 0; // Temporizador para mantener el botón activado
    private bool isPressed = false; // Si el botón está siendo activado por un objeto
    public static Action OnButtonPressed;
    void Start()
    {
        // Guardamos la posición original y el material original
        originalPosition = transform.localPosition;
        buttonRenderer = GetComponent<Renderer>();

        if (buttonRenderer != null)
        {
            originalMaterial = buttonRenderer.material;
        }
    }
    void Update()
    {
        Check();
        switch (activationType)
        {
            case ActivationType.Permanent:
                if (isPressed)
                {
                    Activate();
                }
                break;
            case ActivationType.Temporary:
                if (isPressed)
                {
                    Activate();
                }

                if (activatorTimer <= 0)
                {
                    Deactivate();
                }
                break;
            case ActivationType.Hold:
                if (isPressed)
                {
                    Activate();
                }
                else
                {
                    Deactivate();
                }
                break;
        }

        if (isActivated)
        {
            transform.localPosition = new Vector3(originalPosition.x, originalPosition.y - sinkAmount, originalPosition.z);
            if (buttonRenderer != null)
            {
                buttonRenderer.material = activatedMaterial;
            }
        }
        else
        {
            transform.localPosition = originalPosition;
            if (buttonRenderer != null)
            {
                buttonRenderer.material = originalMaterial;
            }
        }
        if (activationType == ActivationType.Temporary && isActivated)
        {
            activatorTimer -= Time.deltaTime;
        }

    }
    public void Activate()
    {
        // Si el botón ya está activado, no hacer nada
        if (isActivated)
        {
            return;
        }

        // Cambia el estado del botón
        isActivated = true;
        OnButtonPressed?.Invoke();
        if (activationType == ActivationType.Temporary)
        {
            activatorTimer = activatorTime;
        }


    }
    public void Deactivate()
    {

        // Si el botón no está activado, no hacer nada
        if (!isActivated || activationType == ActivationType.Permanent)
        {
            return;
        }


        // Cambia el estado del botón
        isActivated = false;


    }
    private void Check()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, Vector3.up, out raycastHit, 1f, activationLayers))
        {
            isPressed = true;
        }
        else
        {
            isPressed = false;
        }
    }

    // Detecta colisiones con CharacterController



}
