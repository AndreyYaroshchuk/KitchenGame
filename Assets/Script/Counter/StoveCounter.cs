using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static CuttingCounter;


public class StoveCounter : BaseCounter, IHasProgress
{
   
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
      
    


    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State // энумератор 
    {
        Idle,
        Frying,
        Fried,
        Burned,

    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    private NetworkVariable<float> fryingTimer = new NetworkVariable<float> (0f);
    private NetworkVariable<float> burnedTimer = new NetworkVariable<float>(0f);
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private NetworkVariable<State> state = new NetworkVariable<State>(State.Idle);

    public override void OnNetworkSpawn()
    {   
        fryingTimer.OnValueChanged += fryingTimer_OnValueChanged;
        burnedTimer.OnValueChanged += burnedTimer_OnValueChanged;
        state.OnValueChanged += State_OnValueChanged;
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state.Value

        });
        if (state.Value == State.Burned || state.Value == State.Idle)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNirmalized = 0f
            });
        }
        
    }

    private void burnedTimer_OnValueChanged(float previousValue, float newValue)
    {
        float burnedTimerMax = burningRecipeSO != null ? burningRecipeSO.burningTimerMax : 1f;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {   
            progressNirmalized = burnedTimer.Value / burnedTimerMax
        }) ;

    }
    private void fryingTimer_OnValueChanged( float previousValue, float newValue )
    {
        float fryingTimerMax = fryingRecipeSO != null ? fryingRecipeSO.fryingTimerMax : 1f;

        //if (fryingRecipeSO != null)
        //{
        //    fryingTimerMax = fryingRecipeSO.fryingTimerMax;
        //}
        //else
        //{
        //    fryingTimerMax = 1f;    
        //} таже запись что и выше
        
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNirmalized = fryingTimer.Value / fryingTimerMax
        });
    }
    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        if (HasKitchenObject())
        {
            switch (state.Value)
            {

                case State.Idle:

                    break;
                case State.Frying:
                    
                    fryingTimer.Value += Time.deltaTime;
                    
                    
                    if (fryingTimer.Value > fryingRecipeSO.fryingTimerMax)
                    {
                        
                        Debug.Log("Пожарено");
                            
                        KitchenObject.DestroySelfKitchenObject(GetKitchenObject());

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state.Value = State.Fried;
                        burnedTimer.Value = 0f;
                        SetBurningRecipeSOClientRpc(KichenGameMultipler.Instance.GetKitchenObjectSOIndex(GetKitchenObject().GetKitchenObjectSO()));
                         
                    }

                    break;
                case State.Fried:

                    burnedTimer.Value += Time.deltaTime;
                    
                    if (burnedTimer.Value > burningRecipeSO.burningTimerMax)
                    {
                       
                        Debug.Log("Згорело");
                        KitchenObject.DestroySelfKitchenObject(GetKitchenObject());
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state.Value = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state.Value
                        });
                    
                    }

                    break;
                case State.Burned:

                    break;
            }
        }
    }
    
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())  // нет кух. объекта
        {

            if (player.HasKitchenObject()) // в руках есть кух. объект
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) // в руках есть кух объект кторой можно пожарить
                {
                    KitchenObject kitchenObjec = player.GetKitchenObject();
                    kitchenObjec.SetKitchenObjectParent(this);
                    InteractStoveCounterServerRpc(KichenGameMultipler.Instance.GetKitchenObjectSOIndex(kitchenObjec.GetKitchenObjectSO()));
                }
            }
            else  // нет ничего в руках 
            {

            }
        }
        else // на столе есть кух объект
        {
            if (player.HasKitchenObject()) // в руках есть кух. объект
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // игрок держит тарелку 


                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                    
                        GetKitchenObject().DestroySelf();
                        state.Value = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state.Value
                        });   
                    }

                }
            }
            else // ничего в руках нет 
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                StateSetServerRpc();
            }
        }

    }
    [ServerRpc (RequireOwnership = false)]
    private void StateSetServerRpc()
    {
        state.Value = State.Idle;
    }
    [ServerRpc (RequireOwnership = false)]
    private void InteractStoveCounterServerRpc( int kitchenObjectSOIndex)
    {
        fryingTimer.Value = 0f;
        state.Value = State.Frying;
        SetFryingRecipeSOClientRpc(kitchenObjectSOIndex);
    }
    [ClientRpc]
    private void SetFryingRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO  kitchenObjectSO = KichenGameMultipler.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSO);   
    }
    [ClientRpc]
    private void SetBurningRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KichenGameMultipler.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        burningRecipeSO = GetBurningRecipeSOWithInput (kitchenObjectSO);
    }
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSo)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSo);
        return fryingRecipeSO != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSo)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSo);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSo)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSo)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSo)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSo)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
    public bool IsFried()
    {
        return state.Value == State.Fried;
    }

}
