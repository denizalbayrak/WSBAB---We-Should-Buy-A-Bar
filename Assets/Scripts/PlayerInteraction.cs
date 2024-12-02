using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// Manages player interaction with carryable and interactable objects.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    #region Public Variables

    [Header("Interaction Settings")]
    [Tooltip("Layer for interactable and carryable objects.")]
    public LayerMask interactableLayer;

    [Tooltip("The point where the player carries objects.")]
    public Transform carryPoint;

    [Tooltip("Overlap box size.")]
    public Vector3 overlapBoxSize = new Vector3(1f, 1f, 1f);

    [Tooltip("Overlap box offset.")]
    public Vector3 overlapBoxOffset = new Vector3(0f, 0f, 1.5f);

    [Header("Highlight Settings")]
    [Tooltip("Color multiplier when object can be carried.")]
    public Color canCarryColorMultiplier = new Color(0.6f, 0.6f, 0.6f); // Can carry

    [Tooltip("Color multiplier when object is being carried.")]
    public Color carriedColorMultiplier = new Color(0.4f, 0.4f, 0.4f);  // Carried

    #endregion

    #region Private Variables
    private PlayerInputActions inputActions;
    private InputAction interactAction;
    private InputAction holdAction;
    private PlayerMovement movementController;
    public Animator animator;
    private Rigidbody rb;
    private GameObject carriedObject = null;
    private Vector3 carriedObjectOriginalLocalPosition;
    private HashSet<GameObject> highlightedObjects = new HashSet<GameObject>();
    public bool isCarrying = false;
    public List<Renderer> objectRenderers = new List<Renderer>(); // Tüm Renderer'lar
    private List<List<Color>> originalColors = new List<List<Color>>(); // Her Renderer için orijinal renkler listesi

    #endregion

    #region Properties

    public GameObject CarriedObject
    {
        get { return carriedObject; }
        set { carriedObject = value; }
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
       
        // Component References
        rb = GetComponent<Rigidbody>();
        movementController = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

        if (movementController == null)
        {
            Debug.LogError("PlayerMovement component is missing on the player!");
        }

        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the player!");
        }

        // Input Setup
        inputActions = new PlayerInputActions();
        interactAction = inputActions.Player.Interact;
        interactAction.performed += ctx => Interact();

        // Initialize holdAction
        holdAction = inputActions.Player.InteractHold;
    }

    private void OnEnable()
    {
        if (inputActions != null)
        {
            inputActions.Player.Enable();
        }
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.Player.Disable();
        }
    }

    private void FixedUpdate()
    {
        // Movement and Animation Management
        HandleMovement();
    }

    private void Update()
    {
        // Highlighting and Carrying State Management
        UpdateHighlighting();
        if (carriedObject != null)
        {
            UpdateCarriedObjectPosition();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.currentGameState == GameState.InGame)
                {
                    GameUIManager.Instance.PauseGame();
                }
                else if (GameManager.Instance.currentGameState == GameState.Paused)
                {
                    GameUIManager.Instance.ResumeGame();
                }
            }
        }
        // Handle holdAction input
        HandleHoldAction();
    }

    #endregion

    #region Interaction Methods

    /// <summary>
    /// Called when the player interacts.
    /// </summary>
    private void Interact()
    {

        // Try to find an interactable object in front
        Interactable interactable = GetInteractableInFront();

        if (interactable != null)
        {
            Debug.Log("Found interactable: " + interactable.name);

            if (interactable.CanInteract(gameObject))
            {
                Debug.Log("Can interact with: " + interactable.name);

                // Interact with the object
                interactable.Interact(gameObject);
            }
            else
            {
                Debug.Log("Cannot interact with the object.");
            }
        }
        else
        {
            // No interactable object found
            if (carriedObject != null)
            {
                IInteractableItem interactableItem = carriedObject.GetComponent<IInteractableItem>();
                if (interactableItem != null)
                {
                    MojitoGlass glass = GetGlassInFront();
                    if (glass != null)
                    {
                        interactableItem.InteractWith(glass.gameObject, null);
                        // Nesne etkileþim sonrasý yok edildiyse carriedObject'i sýfýrla
                        if (carriedObject == null)
                        {
                            isCarrying = false;
                            animator.SetBool("isCarry", false);
                        }
                        return;
                    }
                }
            }
            else
            {
                // Player is not carrying anything
                // Try to pick up a carryable object if one is in front
                Carryable carryable = GetCarryableInFront();
                if (carryable != null)
                {
                    Debug.Log("Picking up carryable object: " + carryable.name);

                    // Pick up the carryable object
                    PickUpObject(carryable.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Handles the hold action input (e.g., holding the Ctrl key).
    /// </summary>
    private void HandleHoldAction()
    {
        if (holdAction.IsPressed())
        {
            // Check for interactable objects in front
            Interactable interactable = GetInteractableInFront();

            if (interactable != null)
            {
                // Check if the interactable can handle hold actions
                IHoldInteractable holdInteractable = interactable as IHoldInteractable;
                if (holdInteractable != null && holdInteractable.CanHoldInteract(gameObject))
                {
                    // Pass the hold action to the interactable
                    holdInteractable.OnHoldInteract(gameObject, Time.deltaTime);
                }
            }
        }
    }
    private MojitoGlass GetGlassInFront()
    {
        Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);
        Collider[] hitColliders = Physics.OverlapBox(
            boxCenter,
            overlapBoxSize / 2,
            transform.rotation,
            interactableLayer
        );

        foreach (var hitCollider in hitColliders)
        {
            MojitoGlass glass = hitCollider.GetComponent<MojitoGlass>();
            if (glass != null)
            {
                return glass;
            }
        }

        return null;
    }
    /// <summary>
    /// Picks up the specified object.
    /// </summary>
    /// <param name="obj">The object to pick up.</param>
    public void PickUpObject(GameObject obj)
    {
        if (carriedObject != null)
        {
            Debug.LogWarning("Already carrying an object!");
            return;
        }

        carriedObject = obj;
        carriedObjectOriginalLocalPosition = carriedObject.transform.localPosition;

        // Parent the object to carryPoint
        carriedObject.transform.SetParent(carryPoint);

        carriedObject.transform.localRotation = Quaternion.identity;
        Debug.Log("carriedObject " + carriedObject.name);
        if (carriedObject.GetComponent<WineGlass>() !=null)
        {
            carriedObject.transform.localPosition = new Vector3(-0.621f, 0.874f, 0);
        } 
        else if (carriedObject.GetComponent<Lime>() !=null)
        {
            carriedObject.transform.position = new Vector3(0, 0.5f, 0);
        }
        else
        {
            carriedObject.transform.localPosition = Vector3.zero;
        }
        isCarrying = true;
        // Set the carried object's state
        Carryable carryable = carriedObject.GetComponent<Carryable>();
        if (carryable != null)
        {
            carryable.OnPickUp();
        }

        // Remove highlighting
        RemoveHighlightFromObject(carriedObject);
    }

    #endregion

    #region Detection Methods

    /// <summary>
    /// Gets the interactable object in front of the player.
    /// </summary>
    /// <returns>The Interactable object, or null if none found.</returns>
    private Interactable GetInteractableInFront()
    {
        Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);

        Collider[] hitColliders = Physics.OverlapBox(
            boxCenter,
            overlapBoxSize / 2,
            transform.rotation,
            interactableLayer
        );

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("InteractableStatic"))
            {
                Interactable interactable = hitCollider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    return interactable;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the carryable object in front of the player.
    /// </summary>
    /// <returns>The Carryable object, or null if none found.</returns>
    private Carryable GetCarryableInFront()
    {
        Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);

        Collider[] hitColliders = Physics.OverlapBox(
            boxCenter,
            overlapBoxSize / 2,
            transform.rotation,
            interactableLayer
        );

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Carryable"))
            {
                Carryable carryable = hitCollider.GetComponent<Carryable>();
                if (carryable != null)
                {
                    return carryable;
                }
            }
        }

        return null;
    }

    #endregion

    #region Highlighting Methods

    /// <summary>
    /// Detects and highlights carryable and interactable objects.
    /// </summary>
    private void UpdateHighlighting()
    {
        Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);
        Collider[] hitColliders = Physics.OverlapBox(
            boxCenter,
            overlapBoxSize / 2,
            transform.rotation,
            interactableLayer
        );
        List<GameObject> detectedObjects = new List<GameObject>();

        foreach (var hitCollider in hitColliders)
        {
            GameObject obj = hitCollider.gameObject;

            // Etkileþimli objeyi parent'larda ara
            GameObject interactableObj = FindInteractableInParents(obj);

            if (interactableObj != null)
            {
                if (interactableObj == carriedObject)
                {
                    continue;
                }

                detectedObjects.Add(interactableObj);

                if (!highlightedObjects.Contains(interactableObj))
                {
                    // Highlight the object
                    HighlightObject(interactableObj);
                    highlightedObjects.Add(interactableObj);
                }
            }
        }

        // Unhighlight objects that are no longer detected
        HashSet<GameObject> objectsToUnhighlight = new HashSet<GameObject>(highlightedObjects);
        objectsToUnhighlight.ExceptWith(detectedObjects);

        foreach (var obj in objectsToUnhighlight)
        {
            RemoveHighlightFromObject(obj);
            highlightedObjects.Remove(obj);
        }
    }
    private GameObject FindInteractableInParents(GameObject obj)
    {
        Transform t = obj.transform;
        while (t != null)
        {
            if (t.CompareTag("Carryable") || t.CompareTag("InteractableStatic"))
            {
                return t.gameObject;
            }
            t = t.parent;
        }
        return null;
    }
    private void HighlightObject(GameObject obj)
    {
        Highlightable highlightable = obj.GetComponent<Highlightable>();
        if (highlightable != null)
        {
            highlightable.SetHighlight(canCarryColorMultiplier);
        }
    }

    private void RemoveHighlightFromObject(GameObject obj)
    {
        Highlightable highlightable = obj.GetComponent<Highlightable>();
        if (highlightable != null)
        {
            highlightable.SetHighlight(Color.white);
        }
    }

    #endregion

    #region Movement and Animation

    /// <summary>
    /// Manages player movement and updates animation parameters.
    /// </summary>
    private void HandleMovement()
    {
        Vector2 moveInput;
        bool isWalking;
        if (isCarrying)
        {
            animator.SetBool("isCarry", true);
            moveInput = movementController.MoveInput;
            isWalking = moveInput.magnitude > 0.1f;
            animator.SetBool("isWalking", false);
            animator.SetBool("isCarryWalking", isWalking);
            return;
        }
        moveInput = movementController.MoveInput;
        isWalking = moveInput.magnitude > 0.1f;
        animator.SetBool("isCarryWalking", false);
        animator.SetBool("isWalking", isWalking);
    }

    #endregion

    #region Carried Object Positioning

    /// <summary>
    /// Updates the position of the carried object.
    /// </summary>
    private void UpdateCarriedObjectPosition()
    {
        // Keep the carried object at the carry point
        carriedObject.transform.localPosition = Vector3.zero;
        carriedObject.transform.localRotation = Quaternion.identity;


    }

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, overlapBoxSize);
    }

    #endregion
    public void DropCarriedObject()
    {
        if (carriedObject != null)
        {
            carriedObject.transform.SetParent(null);
            carriedObject = null;
            isCarrying = false;
            animator.SetBool("isCarry", false);
        }
    }

}
