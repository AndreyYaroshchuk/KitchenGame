using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class GameInput : MonoBehaviour
{

    private const string REBING_BINDING = "Rebing_Binding"; //

    public static GameInput Instance { get; private set; }
    public event EventHandler OnInteractionAction;
    public event EventHandler OnInteractionActionAlternate;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingChange;
    private PlayerInputActions playerInputActions;
    

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause
    }
    private void Awake()
    {
        Instance = this;
        
        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(REBING_BINDING))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(REBING_BINDING)); // чтение джейсон файла
        }

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.IntercatAlternate.performed += IntercatAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;

        
      
    }
    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.IntercatAlternate.performed -= IntercatAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;
        playerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void IntercatAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractionActionAlternate?.Invoke(this,EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //if(OnInteractionAction != null)
        //{
        //    OnInteractionAction(this, EventArgs.Empty);
        //}
        OnInteractionAction?.Invoke(this, EventArgs.Empty); // прослушал сам себя 
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
       
        inputVector = inputVector.normalized;

        return inputVector;
    }
    public string GetBindingTExt(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputActions.Player.IntercatAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }
    public void RebingBinding(Binding binding, Action onActionRebound) // делигат 
    {
        playerInputActions.Player.Disable();
        InputAction inputAction;
        int buildIndex;
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                buildIndex = 1;
                break;  
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                buildIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                buildIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                buildIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                buildIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputActions.Player.IntercatAlternate;
                buildIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause               ;
                buildIndex = 0;
                break;
        }
       inputAction.PerformInteractiveRebinding(buildIndex).OnComplete(callback =>
        {
            callback.Dispose();
            playerInputActions.Player.Enable();
            onActionRebound();

            //Debug.Log(playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.SetString(REBING_BINDING, playerInputActions.SaveBindingOverridesAsJson()); // запись джейсон файла
            PlayerPrefs.Save();

            OnBindingChange?.Invoke(this, EventArgs.Empty);
        })
            .Start();
    }
}
