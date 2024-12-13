// File: DesignPatterns/PlatePool.cs
using System.Collections.Generic;
using UnityEngine;

public class PlatePool : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject platePrototype; // Prototipo del plato
    [SerializeField] private int initialSize = 10; // Tamaño inicial del pool

    private Queue<PlateKitchenObject> pool = new Queue<PlateKitchenObject>();

    private void Awake()
    {
        if (platePrototype == null)
        {
            Debug.LogError("Plate prototype is not assigned in PlatePool.");
            return;
        }

        // Inicializa el pool con objetos clónicos
        for (int i = 0; i < initialSize; i++)
        {
            PlateKitchenObject plate = platePrototype.Clone();
            plate.gameObject.SetActive(false);
            pool.Enqueue(plate);
        }
    }

    public PlateKitchenObject GetPlate()
    {
        if (platePrototype == null)
        {
            Debug.LogError("Plate prototype is not assigned in PlatePool.");
            return null;
        }

        // Devuelve un plato del pool si hay disponibles
        if (pool.Count > 0)
        {
            PlateKitchenObject plate = pool.Dequeue();
            plate.gameObject.SetActive(true);
            return plate;
        }

        // Si el pool está vacío, crea uno nuevo
        return platePrototype.Clone();
    }

    public void ReturnPlate(PlateKitchenObject plate)
    {
        if (plate == null)
        {
            Debug.LogError("Attempted to return a null plate to the pool.");
            return;
        }

        // Reinicia el estado del plato y regresa al pool
        plate.Reset();
        plate.gameObject.SetActive(false);
        pool.Enqueue(plate);
    }
}