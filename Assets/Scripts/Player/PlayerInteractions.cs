using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class PlayerInteractions : MonoBehaviour, IKitchenObjectParent
{

    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    
    private Vector3 lastInteractDir;
    private BaseCounter slectedCounter;
    private KitchenObject kitchenObject;
    
    
    public event EventHandler<OnSelectedCounterChangedEventsArgs> OnSelectedCounterChanged;
    
    public class OnSelectedCounterChangedEventsArgs : EventArgs
    {
        public BaseCounter slectedCounter;
    }

    public static PlayerInteractions Instance {get; private set;}


    private void Awake()
    {
        Instance = this;
        
    }


    private void Start()
    {
        inputManager.OnInteractAction += InputManager_OnInteractAction;
        inputManager.OnInteractuarAction += InputManager_OnInteractuarAction;
    }

    private void InputManager_OnInteractuarAction(object sender, EventArgs e)
    {
        if(!GameManager.Instance.IsGamePlaying()) return;
        
        if (slectedCounter != null)
        {
            slectedCounter.Interactuar(this);
        }

    }

    private void InputManager_OnInteractAction(object sender, System.EventArgs e)
    {
        if(!GameManager.Instance.IsGamePlaying()) return;
        
        if (slectedCounter != null)
        {
            slectedCounter.Interact(this);
        }
  
    }
    public void HandleInteractions()
    {
        Vector3 moveDir = transform.forward;
        float interactDistance = 1f;

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != slectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                    
                }   
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
        
        //Debug.Log(slectedCounter);
    }


    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.slectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventsArgs { slectedCounter = slectedCounter });

    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }
    
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}