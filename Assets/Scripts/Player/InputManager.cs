using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerLook look;
    private PlayerMotor motor;
    private PlayerInteractions interactions;
    private MouseLock cursorLock;

    public event EventHandler OnInteractAction; 
    public event EventHandler OnInteractuarAction;
    
    private DebugConsole debugConsole; // Referencia a la consola de depuración

    private void Awake()
    {
        playerInput = new PlayerInput();
        
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        cursorLock = GetComponent<MouseLock>();
        interactions = GetComponent<PlayerInteractions>();
        debugConsole = GetComponent<DebugConsole>(); // Asegúrate de que sea DebugConsole si la refactorizaste

        onFoot.Interact.performed += Interact_performed;
        onFoot.Interactuar.performed += Interactuar_performed;
        
        onFoot.CursorLock.performed += ctx => cursorLock.enabled = false;
        onFoot.CursorLock.canceled += ctx => cursorLock.enabled = true;
        
        onFoot.ToggleDebug.performed += ctx => debugConsole.OnToggleDebug();
        onFoot.ExecuteCommand.canceled += ctx => debugConsole.OnReturn();
    }

    private void Interactuar_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractuarAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        interactions.HandleInteractions();
    }

    private void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
