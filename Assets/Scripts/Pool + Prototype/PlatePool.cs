using System.Collections.Generic;
using UnityEngine;

public class PlatePool : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject platePrototype; // Prototipo del plato
    [SerializeField] private int initialSize = 30; // Tamaño inicial del pool

    private Queue<PlateKitchenObject> pool = new Queue<PlateKitchenObject>();

    private void Awake()
    {
        if (platePrototype == null)
        {
            Debug.LogError("Plate prototype is not assigned in PlatePool.");
            return;
        }

        // Inicializa el pool con platos clonados del prototipo
        for (int i = 0; i < initialSize; i++)
        {
            PlateKitchenObject plate = Instantiate(platePrototype);
            plate.gameObject.SetActive(false); // Desactiva el plato
            pool.Enqueue(plate);
        }
    }

    public PlateKitchenObject GetPlate()
    {
        if (pool.Count > 0)
        {
            PlateKitchenObject plate = pool.Dequeue();
            plate.gameObject.SetActive(true); // Activa el plato al entregarlo
            return plate;
        }

        // Si el pool está vacío, crea un nuevo plato
        PlateKitchenObject newPlate = Instantiate(platePrototype);
        newPlate.gameObject.SetActive(true);
        return newPlate;
    }

    public void ReturnPlate(PlateKitchenObject plate)
    {
        if (plate == null)
        {
            Debug.LogError("Attempted to return a null plate to the pool.");
            return;
        }

        plate.Reset(); // Reinicia el estado del plato
        plate.gameObject.SetActive(false); // Desactiva el plato
        pool.Enqueue(plate);
    }
}