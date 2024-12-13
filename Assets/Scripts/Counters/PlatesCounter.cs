using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectsSO plateKitchenObjectsSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    private Stack<KitchenObject> plateStack = new Stack<KitchenObject>(); // Stack de los platos
    [SerializeField] private PlatePool platePool; // Pool de platos

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);

                // Pushea un nuevo plato al Stack desde el pool
                PlateKitchenObject plate = platePool.GetPlate();
                plate.gameObject.SetActive(true);
                plateStack.Push(plate); // Agrega el plato al Stack
            }
        }
    }

    public override void Interact(PlayerInteractions playerInteractions)
    {
        if (!playerInteractions.HasKitchenObject())
        {
            // Player no tiene objeto en mano
            if (platesSpawnedAmount > 0)
            {
                // Hay al menos 1 plato
                platesSpawnedAmount--;

                // Usa el método original para instanciar un plato
                KitchenObject.SpawnKitchenObject(plateKitchenObjectsSO, playerInteractions);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);

                // Saca el último plato del Stack
                if (plateStack.Count > 0)
                {
                    PlateKitchenObject plate = plateStack.Pop() as PlateKitchenObject;
                    if (plate != null)
                    {
                        // Devuelve el plato al pool en lugar de destruirlo
                        platePool.ReturnPlate(plate);
                    }
                }
            }
        }
    }
    public void ReturnPlateToPool(PlateKitchenObject plate)
    {
        if (plate != null)
        {
            plate.gameObject.SetActive(false); // Desactiva el plato antes de devolverlo
            platePool.ReturnPlate(plate);
        }
    }
}
