using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// Bu script, oyuncunun taþýnabilir objelerle etkileþimini ve objelerin vurgulanmasýný yönetir.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    #region Public Variables

    [Header("Interaction Settings")]
    [Tooltip("Taþýnabilir objelerin bulunduðu Layer.")]
    public LayerMask interactLayer;

    [Tooltip("Drop point objelerinin bulunduðu Layer.")]
    public LayerMask dropPointLayer;

    [Tooltip("Oyuncunun objeleri taþýdýðý nokta.")]
    public Transform carryPoint;

    [Tooltip("Overlap box boyutu.")]
    public Vector3 overlapBoxSize = new Vector3(1f, 1f, 1f);

    [Tooltip("Overlap box ofseti.")]
    public Vector3 overlapBoxOffset = new Vector3(0f, 0f, 1.5f);

    [Header("Highlight Settings")]
    [Tooltip("Taþýma ihtimali durumunda renk çarpaný.")]
    public Color canCarryColorMultiplier = new Color(0.6f, 0.6f, 0.6f); // Taþýma ihtimali durumu

    [Tooltip("Taþýma durumunda renk çarpaný.")]
    public Color carriedColorMultiplier = new Color(0.4f, 0.4f, 0.4f);  // Taþýma durumu

    #endregion

    #region Private Variables

    private PlayerInputActions inputActions;
    private InputAction interactAction;
    private PlayerMovement movementController;
    private Animator animator;
    private Rigidbody rb;
    private GameObject dropPoints;
    private GameObject carriedObject = null;
    private Vector3 carriedObjectOriginalLocalPosition;
    private bool isNearDropPoint = false;
    private Transform currentDropPoint;
    private HashSet<GameObject> highlightedObjects = new HashSet<GameObject>();
    private LevelManager levelManager;
    #endregion

    #region Unity Methods

    private void Awake()
    {
        // Bileþen Referanslarý
        rb = GetComponent<Rigidbody>();
        movementController = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        levelManager = LevelManager.Instance;
        if (movementController == null)
        {
            Debug.LogError("PlayerMovement component is missing on the player!");
        }

        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the player!");
        }

        // Input Ayarlarý
        inputActions = new PlayerInputActions();
        interactAction = inputActions.Player.Interact;
        interactAction.performed += ctx => Interact();

        //// DropPoints'ý Bulma
        //dropPoints = GameObject.FindGameObjectWithTag("DropPoints");
        //if (dropPoints != null)
        //{
        //    dropPoints.SetActive(false);
        //}
        //else
        //{
        //    Debug.LogError("DropPoints GameObject with tag 'DropPoints' not found in the scene!");
        //}
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        // Hareket ve Animasyon Yönetimi
        HandleMovement();
    }

    private void Update()
    {
        // Vurgulama ve Taþýma Durumu Yönetimi
        UpdateHighlighting();
        if (carriedObject != null)
        {
            CheckForNearbyDropPoint();
            UpdateCarriedObjectPosition();
            
        }
       
    }
    
    #endregion

    #region Interaction Methods

    /// <summary>
    /// Oyuncu etkileþimde bulunduðunda çaðrýlýr.
    /// </summary>
    private void Interact()
    {
        if (GameManager.Instance.currentGameState == GameState.InGame || GameManager.Instance.currentGameState == GameState.PreLevel)
        {
            if (carriedObject == null)
            {
                TryPickUpObject();
            }
            else
            {
                TryDropObject();
            }
        }
        else
        {
            Debug.Log("Þu anda objeleri taþýyamazsýn.");
        }
    }

    /// <summary>
    /// Objeyi taþýmaya çalýþýr.
    /// </summary>
    private void TryPickUpObject()
    {
        Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);
        Collider[] hitColliders = Physics.OverlapBox(
            boxCenter,
            overlapBoxSize / 2,
            transform.rotation,
            interactLayer
        );

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Portable"))
            {
                PickUpObject(hitCollider.gameObject);
                break;
            }
        }
    }

    /// <summary>
    /// Objeyi taþýmaya baþlar.
    /// </summary>
    /// <param name="obj">Taþýnacak obje.</param>
    private void PickUpObject(GameObject obj)
    {
        carriedObject = obj;
        DropPoint dropPointObj = obj.transform.parent.GetComponent<DropPoint>();
        obj.transform.SetParent(carryPoint);
        dropPointObj.isEmpty=true;
        dropPointObj.deliveredObject = null;
        obj.transform.localPosition = carriedObjectOriginalLocalPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        Collider objCollider = obj.GetComponent<Collider>();
        if (objCollider != null)
        {
            objCollider.enabled = false;
        }       

        PortableObject portableObject = obj.GetComponent<PortableObject>();
        if (portableObject != null)
        {
            portableObject.SetHighlight(HighlightState.Carried);
            highlightedObjects.Remove(obj);
        }

        Debug.Log("Objeyi taþýyorsun: " + obj.name);
        levelManager.UpdateDropPointPlanes(true);
    }

    /// <summary>
    /// Objeyi býrakmayý dener.
    /// </summary>
    private void TryDropObject()
    {
        if (isNearDropPoint && currentDropPoint != null)
        {
            DropPoint dropPointComponent = currentDropPoint.GetComponent<DropPoint>();

            if (dropPointComponent != null && dropPointComponent.isEmpty)
            {
                DropObject(currentDropPoint.position, dropPointComponent);  // Eðer drop point boþsa objeyi býrak
            }
            else
            {
                Debug.LogWarning("This drop point is already occupied! Cannot place another object.");
            }
        }
        else
        {
            Debug.Log("Yakýnda geçerli bir býrakma noktasý yok.");
        }
    }



    /// <summary>
    /// Taþýnan objeyi býrakýr ve býrakma noktasýna yerleþtirir.
    /// </summary>
    /// <param name="dropPosition">Objenin býrakýlacaðý pozisyon.</param>
    private void DropObject(Vector3 dropPosition, DropPoint dropPointComponent)
    {
        PortableObject portableObject = carriedObject.GetComponent<PortableObject>();
        if (portableObject != null)
        {
            float yOffset = portableObject.dropYOffset;
            dropPosition.y += yOffset;
        }
        else
        {
            Debug.LogError("Carried object does not have a PortableObject component!");
        }

        carriedObject.transform.SetParent(dropPointComponent.gameObject.transform);
        dropPointComponent.DeliverObject(carriedObject);        
        carriedObject.transform.position = dropPosition;
        carriedObject.transform.rotation = currentDropPoint.rotation;
        Collider objCollider = carriedObject.GetComponent<Collider>();
        if (objCollider != null)
        {
            objCollider.enabled = true;
        }
       
        if (portableObject != null)
        {
            portableObject.SetHighlight(HighlightState.None); // Rengi orijinal haline döndür
        }

        Debug.Log("Objeyi býraktýn: " + carriedObject.name);
        levelManager.UpdateDropPointPlanes(false);
        carriedObject = null;
        currentDropPoint = null;
        isNearDropPoint = false;
    }

    #endregion

    #region Highlighting Methods

    /// <summary>
    /// Taþýnabilir objeleri tespit eder ve vurgular.
    /// </summary>
    private void UpdateHighlighting()
    {
        Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);
        Collider[] hitColliders = Physics.OverlapBox(
            boxCenter,
            overlapBoxSize / 2,
            transform.rotation,
            interactLayer
        );

        List<GameObject> detectedObjects = new List<GameObject>();

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Portable"))
            {
                detectedObjects.Add(hitCollider.gameObject);
            }
        }

        // Yeni tespit edilen objeleri vurgula
        foreach (var obj in detectedObjects)
        {
            if (!highlightedObjects.Contains(obj) && obj != carriedObject)
            {
                PortableObject portableObject = obj.GetComponent<PortableObject>();
                if (portableObject != null)
                {
                    portableObject.SetHighlight(HighlightState.CanCarry);
                    highlightedObjects.Add(obj);
                    Debug.Log("Objeyi vurguladýn (CanCarry): " + obj.name);
                }
            }
        }

        // Artýk tespit edilmeyen objelerin vurgusunu kaldýr
        HashSet<GameObject> objectsToUnhighlight = new HashSet<GameObject>(highlightedObjects);
        objectsToUnhighlight.ExceptWith(detectedObjects);

        foreach (var obj in objectsToUnhighlight)
        {
            PortableObject portableObject = obj.GetComponent<PortableObject>();
            if (portableObject != null)
            {
                portableObject.SetHighlight(HighlightState.None);
                highlightedObjects.Remove(obj);
                Debug.Log("Objenin vurgusunu kaldýrdýn (None): " + obj.name);
            }
        }
    }

    #endregion

    #region Drop Point Methods

    /// <summary>
    /// Yakýndaki býrakma noktalarýný kontrol eder.
    /// </summary>
    private void CheckForNearbyDropPoint()
    {
        Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);
        Collider[] hitColliders = Physics.OverlapBox(
            boxCenter,
            overlapBoxSize / 2,
            transform.rotation,
            dropPointLayer // Drop point'leri kontrol et
        );

        isNearDropPoint = false;
        currentDropPoint = null;

        foreach (var hitCollider in hitColliders)
        {
            // Depo ya da bar alaný olup olmadýðýný kontrol et
            if (hitCollider.CompareTag("StorageDropPoint") || hitCollider.CompareTag("ShopDropPoint"))
            {
                isNearDropPoint = true;
                currentDropPoint = hitCollider.transform;
                break;
            }
        }

        if (!isNearDropPoint)
        {
            Debug.Log("Yakýnda býrakma noktasý bulunamadý!");
        }
    }



    #endregion

    #region Movement and Animation

    /// <summary>
    /// Oyuncunun hareketini yönetir ve animasyon parametrelerini günceller.
    /// </summary>
    private void HandleMovement()
    {
        Vector2 moveInput = movementController.MoveInput;
        bool isWalking = moveInput.magnitude > 0.1f;
        animator.SetBool("isWalking", isWalking);
    }

    #endregion

    #region Carried Object Positioning

    /// <summary>
    /// Taþýma durumunda olan objenin pozisyonunu günceller.
    /// </summary>
    private void UpdateCarriedObjectPosition()
    {
        if (isNearDropPoint && currentDropPoint != null)
        {
            PortableObject portableObject = carriedObject.GetComponent<PortableObject>();
            if (portableObject != null)
            {
                float yOffset = portableObject.dropYOffset;
                Vector3 targetPosition = currentDropPoint.position + new Vector3(0, yOffset, 0);

                // carryPoint'in dönüþüne göre objenin lokal pozisyonunu hesapla
                Vector3 localPosition = carryPoint.InverseTransformPoint(targetPosition);

                carriedObject.transform.localPosition = localPosition;
                carriedObject.transform.rotation = currentDropPoint.rotation;

                Debug.Log($"Objeyi drop point'e yerleþtiriliyor: Position={carriedObject.transform.position}, Rotation={carriedObject.transform.rotation}");
            }
            else
            {
                Debug.LogError("Carried object does not have a PortableObject component!");
            }
        }
        else
        {
            carriedObject.transform.localPosition = carriedObjectOriginalLocalPosition;
            carriedObject.transform.localRotation = Quaternion.identity;

            Debug.Log($"Objeyi carryPoint'e göre konumlandýrýlýyor: LocalPosition={carriedObject.transform.localPosition}");
        }
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
}