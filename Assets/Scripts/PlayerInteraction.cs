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
    // public List<Renderer> objectRenderers = new List<Renderer>(); // Unused in this class
    // private List<List<Color>> originalColors = new List<List<Color>>(); // Unused in this class
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
        // En uygun etkile�imli objeyi bul
        Interactable interactable = GetTopInteractableInFront();

        if (interactable != null)
        {
            Debug.Log("Found interactable: " + interactable.name);

            if (interactable.CanInteract(gameObject))
            {
                Debug.Log("Can interact with: " + interactable.name);
                // Objeyle etkile�ime gir
                interactable.Interact(gameObject);
            }
            else
            {
                Debug.Log("Cannot interact with the object.");
            }
        }
        else
        {
            // Etkile�imli obje bulunamad�ysa
            if (carriedObject != null)
            {
                IInteractableItem interactableItem = carriedObject.GetComponent<IInteractableItem>();
                if (interactableItem != null)
                {
                    // E�er elinizde bir bardak ta��yorsan�z ve ba�ka bir objeyle etkile�ime ge�mek istiyorsan�z
                    // �rne�in, bir blender ile etkile�im
                    // Bu k�sm� kendi oyun mekaniklerinize g�re d�zenleyin
                    // �rnek:
                    // BlenderPoint blender = GetBlenderInFront();
                    // if (blender != null)
                    // {
                    //     interactableItem.InteractWith(blender.gameObject, null);
                    //     // E�er nesne yok edildiyse carriedObject'i s�f�rla
                    //     if (carriedObject == null)
                    //     {
                    //         isCarrying = false;
                    //         animator.SetBool("isCarry", false);
                    //     }
                    //     return;
                    // }

                    Debug.Log("Carried object can interact with something else.");
                }
            }
            else
            {
                // Ta��y�c� nesne yoksa, bir carryable obje olup olmad���n� kontrol et
                Carryable carryable = GetTopCarryableInFront();
                if (carryable != null)
                {
                    Debug.Log("Picking up carryable object: " + carryable.name);
                    // Carryable objeyi al
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
            // En uygun etkile�imli objeyi bul
            Interactable interactable = GetTopInteractableInFront();

            if (interactable != null)
            {
                // Etkile�imli objenin hold etme yetene�i olup olmad���n� kontrol et
                IHoldInteractable holdInteractable = interactable as IHoldInteractable;
                if (holdInteractable != null && holdInteractable.CanHoldInteract(gameObject))
                {
                    // Hold etme i�lemini objeye ge�ir
                    holdInteractable.OnHoldInteract(gameObject, Time.deltaTime);
                }
            }
        }
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

        // Objeyi do�ru pozisyona yerle�tir
        if (carriedObject.GetComponent<WineGlass>() != null)
        {
            carriedObject.transform.localPosition = new Vector3(-0.621f, 0.874f, 0);
        }
        else if (carriedObject.GetComponent<Lime>() != null)
        {
            carriedObject.transform.position = new Vector3(0, 0.5f, 0);
        }
        else
        {
            carriedObject.transform.localPosition = Vector3.zero;
        }

        isCarrying = true;
        animator.SetBool("isCarry", true);

        // Carryable objenin durumunu ayarla
        Carryable carryable = carriedObject.GetComponent<Carryable>();
        if (carryable != null)
        {
            carryable.OnPickUp();
        }

        // Highlight'� kald�r
        RemoveHighlightFromObject(carriedObject);
    }

    #endregion

    #region Detection Methods

    /// <summary>
    /// Gets the top interactable object in front of the player based on priority.
    /// </summary>
    /// <returns>The top Interactable object, or null if none found.</returns>
    private Interactable GetTopInteractableInFront()
    {
        Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);

        Collider[] hitColliders = Physics.OverlapBox(
            boxCenter,
            overlapBoxSize / 2,
            transform.rotation,
            interactableLayer
        );

        // Objeleri uzakl��a g�re s�rala
        List<Collider> sortedColliders = new List<Collider>(hitColliders);
        sortedColliders.Sort((a, b) =>
        {
            float distanceA = Vector3.Distance(boxCenter, a.transform.position);
            float distanceB = Vector3.Distance(boxCenter, b.transform.position);
            return distanceA.CompareTo(distanceB);
        });

        foreach (var hitCollider in sortedColliders)
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
    /// Gets the top carryable object in front of the player based on priority.
    /// </summary>
    /// <returns>The top Carryable object, or null if none found.</returns>
    private Carryable GetTopCarryableInFront()
    {
        Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);

        Collider[] hitColliders = Physics.OverlapBox(
            boxCenter,
            overlapBoxSize / 2,
            transform.rotation,
            interactableLayer
        );

        // Objeleri uzakl��a g�re s�rala
        List<Collider> sortedColliders = new List<Collider>(hitColliders);
        sortedColliders.Sort((a, b) =>
        {
            float distanceA = Vector3.Distance(boxCenter, a.transform.position);
            float distanceB = Vector3.Distance(boxCenter, b.transform.position);
            return distanceA.CompareTo(distanceB);
        });

        foreach (var hitCollider in sortedColliders)
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
    /// Detects and highlights the top interactable or carryable object.
    /// </summary>
    private void UpdateHighlighting()
    {
        // En uygun objeyi bul
        Interactable topInteractable = GetTopInteractableInFront();
        Carryable topCarryable = GetTopCarryableInFront();

        GameObject selectedObject = null;

        if (topInteractable != null)
        {
            selectedObject = topInteractable.gameObject;
        }
        else if (topCarryable != null)
        {
            selectedObject = topCarryable.gameObject;
        }

        List<GameObject> detectedObjects = new List<GameObject>();

        if (selectedObject != null)
        {
            detectedObjects.Add(selectedObject);

            if (!highlightedObjects.Contains(selectedObject))
            {
                // Highlight the object
                HighlightObject(selectedObject);
                highlightedObjects.Add(selectedObject);
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

    #region Drop Carried Object

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

    #endregion
}
