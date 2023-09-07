using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Rendering;
using UnityEngine;
using Unity.Netcode;
using System;


public class Player : NetworkBehaviour, IKitchenObjectParent
{
    public static Player LocalInstance { get; private set; }

    public event EventHandler OnPlayerPicked;


    public event EventHandler<OnSlectedCounterChangeEventArgs> OnSlectedCounterChange;


    public static event EventHandler OnPlayerSpawnet;

    public static event EventHandler OnAnyPlayerPicked;

    public class OnSlectedCounterChangeEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;

    }
    public static void ResetStaticData()
    {
        OnPlayerSpawnet = null;
        // OnAnyPlayerPicked = null;
    }

    [SerializeField] private float moveSpeed = 7f;

    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private LayerMask collisionsLayerMask;
    [SerializeField] private Transform objHoldPoint;
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private List<Vector3> spawnPositionList;
    private KitchenObject kitchenObject;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;



    private void Start()
    {
        GameInput.Instance.OnInteractionAction += GameInput_OnInteractionAction;
        GameInput.Instance.OnInteractionActionAlternate += GameInput_OnInteractionActionAlternate;
        PlayerData playerData = KichenGameMultipler.Instance.GetPlayerDataFromClientId(OwnerClientId);
        playerVisual.SetPlayerColor(KichenGameMultipler.Instance.GetPlayerColor(playerData.colorId));
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
        }
        transform.position = spawnPositionList[(int)OwnerClientId];
        OnPlayerSpawnet.Invoke(this, EventArgs.Empty);
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }

    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == OwnerClientId)
        {
            KitchenObject.DestroySelfKitchenObject(GetKitchenObject());
        }
    }

    private void GameInput_OnInteractionActionAlternate(object sender, EventArgs e)
    {
        if (!KicthenGameManeger.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }

    }

    private void GameInput_OnInteractionAction(object sender, System.EventArgs e)
    {
        if (!KicthenGameManeger.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }

    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        HandleMovement();
        //HandleMovementServerAuth();
        HandleInteractions();
    }
    public bool IsWalking()
    {
        return isWalking;
    }

    public void HandleInteractions()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistan = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistan, countersLayerMask))
        {

            //Has clearCounter

            //ClearCounter clearCounter1 = raycastHit.transform.GetComponent<ClearCounter>();
            //if(clearCounter1 != null)
            //{
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)
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
        // Debug.Log(selectedCounter);
    }

    //private void HandleMovementServerAuth()
    //{

    //    Vector2 inputVector = GameInput.Instance.GetMovementVector();
    //    HandleMovementServerRpc(inputVector);
    //}

    //[ServerRpc(RequireOwnership = false)]
    //private void HandleMovementServerRpc(Vector2 inputVector)
    //{
    //    Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
    //    float moveDistance = moveSpeed * Time.deltaTime;
    //    float playerHeight = 2f;
    //    float playerRadius = 0.7f;
    //    bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);


    //    if (!canMove)
    //    {
    //        // cannot move towards moveDir


    //        // Attempt only X movement
    //        Vector3 moveDir_X = new Vector3(moveDir.x, 0f, 0f).normalized;
    //        canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir_X, moveDistance);
    //        if (canMove)
    //        {
    //            // Can move only on the X
    //            moveDir = moveDir_X;
    //        }
    //        else
    //        {
    //            // Cannot move only on the X


    //            // Attempt only Z movement
    //            Vector3 moveDir_Z = new Vector3(0, 0, moveDir.z).normalized;
    //            canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir_Z, moveDistance);
    //            if (canMove)
    //            {
    //                //Can move only on the Z
    //                moveDir = moveDir_Z;
    //            }
    //            else
    //            {
    //                // Cannot move in any direction
    //            }
    //        }

    //    }
    //    if (canMove)
    //    {
    //        transform.position += moveDir * moveDistance;
    //    }


    //    isWalking = moveDir != Vector3.zero;
    //    float rotateSpeed = 10f;
    //    transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

    //}

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerHeight = 2f;
        float playerRadius = 0.7f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance, collisionsLayerMask);


        if (!canMove)
        {
            // cannot move towards moveDir


            // Attempt only X movement
            Vector3 moveDir_X = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir_X, moveDistance, collisionsLayerMask);
            if (canMove)
            {
                // Can move only on the X
                moveDir = moveDir_X;
            }
            else
            {
                // Cannot move only on the X


                // Attempt only Z movement
                Vector3 moveDir_Z = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir_Z, moveDistance, collisionsLayerMask);
                if (canMove)
                {
                    //Can move only on the Z
                    moveDir = moveDir_Z;
                }
                else
                {
                    // Cannot move in any direction
                }
            }

        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }


        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed); // плавный поворот 

    }
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSlectedCounterChange?.Invoke(this, new OnSlectedCounterChangeEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return objHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPlayerPicked?.Invoke(this, EventArgs.Empty);
            OnAnyPlayerPicked?.Invoke(this, EventArgs.Empty);

        }
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
    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
