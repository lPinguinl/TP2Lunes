using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ACA SE APLICA PILA

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private Stack<GameObject> plateVisualStack; // Stack to manage the plates visually

    private void Awake()
    {
        plateVisualStack = new Stack<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
    {
        if (plateVisualStack.Count > 0)
        {
            // Saca el ultimo q entro 
            GameObject plateGameObject = plateVisualStack.Pop(); // Saca el ultimo item
            Destroy(plateGameObject); // Destruye el objeto 
        }
    }

    private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
    {
        // Instancia un nuevo prefab
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        
        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualStack.Count, 0);

        // Añanade un nuevo plato al Stack
        plateVisualStack.Push(plateVisualTransform.gameObject); // Añanade un nuevo plato arriba
    }
}