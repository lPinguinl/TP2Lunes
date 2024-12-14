using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectsSO plateKitchenObjectsSO;
    [SerializeField] private PlateKitchenObject platePrefab;
    
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;
    
    
    private void Start()
    {
        PlateKitchenObject.InitializePool(platePrefab, transform); // Inicializa el pool con el prefab
    }
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
            }
        }
    }
    public void ReturnPlateToPool(PlateKitchenObject plate)
    {
        if (plate != null)
        {
            plate.gameObject.SetActive(false); // Desactiva el plato antes de devolverlo
        }
    }
}
