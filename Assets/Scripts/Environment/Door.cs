using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipDoorController : MonoBehaviour
{

    public Transform upperDoor; // Puerta superior
    public Transform lowerDoor; // Puerta inferior

    public float openDistance = 2f; // Distancia que las puertas se moverán al abrirse
    public float openSpeed = 2f; // Velocidad de apertura
    public float closeSpeed = 2f; // Velocidad de cierre
    public float delayBeforeClosing = 2f; // Tiempo que permanece abierta antes de cerrar

    private Vector3 upperClosedPosition; // Posición cerrada de la puerta superior
    private Vector3 lowerClosedPosition; // Posición cerrada de la puerta inferior
    private Vector3 upperOpenPosition; // Posición abierta de la puerta superior
    private Vector3 lowerOpenPosition; // Posición abierta de la puerta inferior
    private Vector3 upperClosePosition; // Posición cerrada extendida de la puerta superior
    private Vector3 lowerClosePosition; // Posición cerrada extendida de la puerta inferior

    private bool isOpening = false;
    private bool isClosing = false;
    public bool IsOpen = false;
    private AudioSource audioSource;
    private ButtonActivator[] buttonActivator;
    private bool AllButtonsActivated = false;
    [SerializeField] ParticleSystem fx;

    void Start()
    {
        ButtonActivator.OnButtonPressed += CheckButtons;
        buttonActivator = FindObjectsOfType<ButtonActivator>();

        audioSource = GetComponent<AudioSource>();
        // Guardamos las posiciones cerradas al inicio
        upperClosedPosition = upperDoor.localPosition;
        lowerClosedPosition = lowerDoor.localPosition;

        // Calculamos las posiciones abiertas sumando y restando la distancia en el eje Y
        upperOpenPosition = new Vector3(upperClosedPosition.x, upperClosedPosition.y + openDistance, upperClosedPosition.z);
        lowerOpenPosition = new Vector3(lowerClosedPosition.x, lowerClosedPosition.y - openDistance, lowerClosedPosition.z);

        // Calculamos las posiciones cerradas extendidas para el cierre (doble de distancia)
        upperClosePosition = new Vector3(upperOpenPosition.x, upperOpenPosition.y - openDistance, upperOpenPosition.z);
        lowerClosePosition = new Vector3(lowerOpenPosition.x, lowerOpenPosition.y + openDistance, lowerOpenPosition.z);
    }

    void Update()
    {

        if (AllButtonsActivated)
        {
            OpenDoors();
        }
        else
        {
            StartClosing();
        }
        if (isOpening)
        {
            // Movemos las puertas hacia las posiciones abiertas
            upperDoor.localPosition = Vector3.Lerp(upperDoor.localPosition, upperOpenPosition, Time.deltaTime * openSpeed);
            lowerDoor.localPosition = Vector3.Lerp(lowerDoor.localPosition, lowerOpenPosition, Time.deltaTime * openSpeed);

            // Verificamos si han alcanzado las posiciones abiertas
            if (Vector3.Distance(upperDoor.localPosition, upperOpenPosition) < 0.01f &&
                Vector3.Distance(lowerDoor.localPosition, lowerOpenPosition) < 0.01f)
            {
                isOpening = false;
                Invoke("StartClosing", delayBeforeClosing); // Esperamos un tiempo antes de cerrar
            }
        }

        if (isClosing)
        {
            // Movemos las puertas hacia las posiciones cerradas extendidas (doble de distancia)
            upperDoor.localPosition = Vector3.Lerp(upperDoor.localPosition, upperClosePosition, Time.deltaTime * closeSpeed);
            lowerDoor.localPosition = Vector3.Lerp(lowerDoor.localPosition, lowerClosePosition, Time.deltaTime * closeSpeed);

            // Verificamos si han alcanzado las posiciones cerradas extendidas
            if (Vector3.Distance(upperDoor.localPosition, upperClosePosition) < 0.01f &&
                Vector3.Distance(lowerDoor.localPosition, lowerClosePosition) < 0.01f)
            {
                isClosing = false;
            }
        }
    }

    // Método para iniciar la apertura de las puertas
    public void OpenDoors()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        isOpening = true;
        IsOpen = true;
        isClosing = false;
    }

    // Método para iniciar el cierre de las puertas
    private void StartClosing()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        isClosing = true;
        isOpening = false;
        IsOpen = false;
    }
    public void CheckButtons()
    {

        if (buttonActivator.Length == 0)
        {
            AllButtonsActivated = false;
            return;
        }
        foreach (ButtonActivator button in buttonActivator)
        {
            if (!button.isActivated)
            {
                AllButtonsActivated = false;
                return;
            }
        }
        AllButtonsActivated = true;
        fx.Play();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (IsOpen)
            {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }


        }
    }
}
