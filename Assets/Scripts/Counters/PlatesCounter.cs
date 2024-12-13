using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ACA SE APLICA PILA
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
    [SerializeField] private PlatePool platePool;

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

                // Pushea un nuevo plato a la pila
                plateStack.Push(gameObject.AddComponent<KitchenObject>()); 
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

                KitchenObject.SpawnKitchenObject(plateKitchenObjectsSO, playerInteractions);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);

                // Saca el ulitmo q entro
                if (plateStack.Count > 0)
                {
                    plateStack.Pop(); // Saca el plato del Stack
                }
            }
        }
    }
    
    public void ReturnPlateToPool(PlateKitchenObject plate)
    {
        // Devuelve el plato al pool
        platePool.ReturnPlate(plate);
    }
    
    
}