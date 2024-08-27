using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Transform[] buildings; // Array de edificios
    [SerializeField] private float distance = 50f; // Distancia de culling

    private Camera mainCamera;

    void Start()
    {
        // Obtener la referencia a la cámara principal
        mainCamera = Camera.main;

        // Inicializar el array de edificios con los hijos directos
        int childCount = transform.childCount;
        buildings = new Transform[childCount];

        // Rellenar el array solo con los hijos directos
        for (int i = 0; i < childCount; i++)
        {
            buildings[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        foreach (Transform building in buildings)
        {
            CheckCull(building.gameObject);
        }
    }

    void CheckCull(GameObject building)
    {
        // Verifica la distancia desde la cámara al edificio
        if (Vector3.Distance(building.transform.position, mainCamera.transform.position) > distance)
        {
            building.SetActive(false);
        }
        else
        {
            building.SetActive(true);
        }
    }
}
