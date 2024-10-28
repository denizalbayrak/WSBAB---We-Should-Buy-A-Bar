using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    //private PlayerInputActions inputActions;
    //private InputAction moveAction;
    //private InputAction interactAction;
    //private Vector2 moveInput;
    //private Rigidbody rb;
    //public float speed = 5f;
    //public float rotationSpeed = 720f; // Derece cinsinden dönüþ hýzý

    //private Animator animator; // Animator referansý

    //// Taþýnan obje referansý
    //private GameObject carriedObject = null;

    //// Taþýnan objenin orijinal localPosition deðeri
    //private Vector3 carriedObjectOriginalLocalPosition;

    //// Obje býrakma noktasýna yakýnken mi?
    //private bool isNearDropPoint = false;

    //// Yakýndaki býrakma noktasý referansý
    //private Transform currentDropPoint;

    //// Interact LayerMask
    //public LayerMask interactLayer;

    //// OverlapBox parametreleri Inspector üzerinden ayarlanabilir
    //public Vector3 overlapBoxSize = new Vector3(1f, 1f, 1f);
    //public Vector3 overlapBoxOffset = new Vector3(0f, 0f, 1.5f);

    //// Taþýma noktasý (carryPoint) referansý
    //public Transform carryPoint;

    //// Býrakma noktasý LayerMask
    //public LayerMask dropPointLayer; // Býrakma noktalarýnýn bulunduðu Layer

    //// DropPoints GameObject referansý
    //private GameObject dropPoints; // Dinamik olarak bulunacak

    //// Highlighted objects
    //private HashSet<GameObject> highlightedObjects = new HashSet<GameObject>();

    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody>();
    //    inputActions = new PlayerInputActions();

    //    moveAction = inputActions.Player.Move;
    //    interactAction = inputActions.Player.Interact;

    //    moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
    //    moveAction.canceled += ctx => moveInput = Vector2.zero;

    //    interactAction.performed += ctx => Interact();

    //    animator = GetComponent<Animator>(); // Animator bileþenini al
    //    if (animator == null)
    //    {
    //        Debug.LogError("Animator component is missing on the player!");
    //    }

    //    // DropPoints'ý etiketle bul
    //    dropPoints = GameObject.FindGameObjectWithTag("DropPoints");
    //    if (dropPoints != null)
    //    {
    //        dropPoints.SetActive(false); // Baþlangýçta devre dýþý býrak
    //    }
    //    else
    //    {
    //        Debug.LogError("DropPoints GameObject with tag 'DropPoints' not found in the scene!");
    //    }
    //}

    //private void OnEnable()
    //{
    //    inputActions.Player.Enable();
    //}

    //private void OnDisable()
    //{
    //    inputActions.Player.Disable();
    //}

    //private void FixedUpdate()
    //{
    //    // Hareket iþlemleri
    //    Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y).normalized;
    //    Vector3 newPosition = transform.position + movement * speed * Time.fixedDeltaTime;
    //    rb.MovePosition(newPosition);

    //    // Oyuncunun baktýðý yönü hareket yönüne doðru yumuþak bir þekilde çevir
    //    if (movement != Vector3.zero)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
    //        transform.rotation = Quaternion.RotateTowards(
    //            transform.rotation,
    //            targetRotation,
    //            rotationSpeed * Time.fixedDeltaTime
    //        );
    //    }

    //    // Animasyonlarý güncelle
    //    UpdateAnimations(movement);
    //}

    //private void Update()
    //{
    //    // Objeleri vurgulama
    //    UpdateHighlighting();

    //    if (carriedObject != null)
    //    {
    //        CheckForNearbyDropPoint();
    //        UpdateCarriedObjectPosition();
    //    }
    //}

    ///// <summary>
    ///// Animasyon parametrelerini günceller.
    ///// </summary>
    ///// <param name="movement">Hareket vektörü.</param>
    //private void UpdateAnimations(Vector3 movement)
    //{
    //    if (animator != null)
    //    {
    //        bool isWalking = movement.magnitude > 0.1f;
    //        animator.SetBool("isWalking", isWalking);
    //    }
    //}

    ///// <summary>
    ///// Interact aksiyonu gerçekleþtiðinde çaðrýlýr.
    ///// </summary>
    //private void Interact()
    //{
    //    // Oyun durumunu kontrol et
    //    if (GameManager.Instance.currentGameState == GameState.InGame || GameManager.Instance.currentGameState == GameState.PreLevel)
    //    {
    //        // Oyun Normal Level veya PreLevel'de, objeleri taþýyabiliriz
    //        if (carriedObject == null)
    //        {
    //            // Obje almaya çalýþ
    //            TryPickUpObject();
    //        }
    //        else
    //        {
    //            // Objeyi býrakmayý dene
    //            TryDropObject();
    //        }
    //    }
    //    else
    //    {
    //        // Diðer durumlarda objeleri taþýyamazsýnýz
    //        Debug.Log("Þu anda objeleri taþýyamazsýn.");
    //    }
    //}

    ///// <summary>
    ///// Objeyi taþýmaya çalýþýr.
    ///// </summary>
    //private void TryPickUpObject()
    //{
    //    // OverlapBox'ýn merkezini hesapla
    //    Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);

    //    // Etrafýmýzdaki taþýnabilir objeleri bul
    //    Collider[] hitColliders = Physics.OverlapBox(
    //        boxCenter,
    //        overlapBoxSize / 2,
    //        transform.rotation,
    //        interactLayer
    //    );

    //    foreach (var hitCollider in hitColliders)
    //    {
    //        if (hitCollider.CompareTag("Portable"))
    //        {
    //            // Objeyi taþý
    //            PickUpObject(hitCollider.gameObject);
    //            break;
    //        }
    //        // Baþka etkileþimler için ek koþullar ekleyebilirsiniz
    //    }
    //}

    ///// <summary>
    ///// Objeyi taþýmaya baþlar.
    ///// </summary>
    ///// <param name="obj">Taþýnacak obje.</param>
    //private void PickUpObject(GameObject obj)
    //{
    //    carriedObject = obj;
    //    obj.transform.SetParent(carryPoint); // Objeyi carryPoint'in child'ý yap

    //    // Objenin pozisyonunu ayarla ve orijinal localPosition deðerini kaydet
    //    obj.transform.localPosition = carriedObjectOriginalLocalPosition = Vector3.zero;
    //    obj.transform.localRotation = Quaternion.identity;

    //    // Objeyi taþýrken collider'ýný devre dýþý býrak
    //    Collider objCollider = obj.GetComponent<Collider>();
    //    if (objCollider != null)
    //    {
    //        objCollider.enabled = false;
    //    }

    //    Debug.Log("Objeyi taþýyorsun: " + obj.name);

    //    // DropPoints'ý etkinleþtir
    //    if (dropPoints != null)
    //    {
    //        dropPoints.SetActive(true);
    //        Debug.Log("DropPoints etkinleþtirildi.");
    //    }

    //    // Taþýma sýrasýnda objeyi daha koyu renge getir
    //    PortableObject portableObject = obj.GetComponent<PortableObject>();
    //    if (portableObject != null)
    //    {
    //        portableObject.SetHighlight(HighlightState.Carried); // Taþýma durumu
    //        highlightedObjects.Remove(obj); // Taþýma sýrasýnda overlap box vurgusunu kaldýr
    //        Debug.Log("Objeyi taþýrken rengini daha koyu hale getirdin: " + obj.name);
    //    }
    //}

    ///// <summary>
    ///// Objeyi býrakmayý dener.
    ///// </summary>
    //private void TryDropObject()
    //{
    //    if (isNearDropPoint && currentDropPoint != null)
    //    {
    //        // Objeyi býrak
    //        DropObject(currentDropPoint.position);
    //    }
    //    else
    //    {
    //        // Yakýnda býrakma noktasý yok, objeyi býrakamazsýn
    //        Debug.Log("Burada objeyi býrakamazsýn.");
    //        // Ýsteðe baðlý olarak oyuncuya görsel veya iþitsel geri bildirim saðlayabilirsiniz
    //    }
    //}

    ///// <summary>
    ///// Taþýnan objeyi býrakýr ve býrakma noktasýna yerleþtirir.
    ///// </summary>
    ///// <param name="dropPosition">Objenin býrakýlacaðý pozisyon.</param>
    //private void DropObject(Vector3 dropPosition)
    //{
    //    // Taþýnan objeden PortableObject bileþenini al
    //    PortableObject portableObject = carriedObject.GetComponent<PortableObject>();
    //    if (portableObject != null)
    //    {
    //        float yOffset = portableObject.dropYOffset;

    //        // Y offset'ini uygulayarak pozisyonu güncelle
    //        dropPosition.y += yOffset;
    //    }
    //    else
    //    {
    //        Debug.LogError("Carried object does not have a PortableObject component!");
    //    }

    //    carriedObject.transform.SetParent(null);
    //    carriedObject.transform.position = dropPosition;
    //    carriedObject.transform.rotation = currentDropPoint.rotation;

    //    // Objenin collider'ýný tekrar etkinleþtir
    //    Collider objCollider = carriedObject.GetComponent<Collider>();
    //    if (objCollider != null)
    //    {
    //        objCollider.enabled = true;
    //    }

    //    Debug.Log("Objeyi býraktýn: " + carriedObject.name);

    //    // DropPoints'ý devre dýþý býrak
    //    if (dropPoints != null)
    //    {
    //        dropPoints.SetActive(false);
    //        Debug.Log("DropPoints devre dýþý býrakýldý.");
    //    }

    //    // Objeyi býraktýktan sonra rengini geri döndür
    //    if (portableObject != null)
    //    {
    //        portableObject.SetHighlight(HighlightState.None);
    //        Debug.Log("Objeyi býraktýktan sonra rengini orijinal haline döndürdün: " + carriedObject.name);
    //    }

    //    carriedObject = null;
    //    currentDropPoint = null;
    //    isNearDropPoint = false;
    //}

    ///// <summary>
    ///// Yakýndaki býrakma noktalarýný kontrol eder.
    ///// </summary>
    //private void CheckForNearbyDropPoint()
    //{
    //    // OverlapBox'ýn merkezini hesapla
    //    Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);

    //    // Etrafýmýzdaki býrakma noktalarýný bul
    //    Collider[] hitColliders = Physics.OverlapBox(
    //        boxCenter,
    //        overlapBoxSize / 2,
    //        transform.rotation,
    //        dropPointLayer
    //    );

    //    isNearDropPoint = false;
    //    currentDropPoint = null;

    //    foreach (var hitCollider in hitColliders)
    //    {
    //        if (hitCollider.CompareTag("DropPoint"))
    //        {
    //            // Býrakma noktasýný bulduk
    //            isNearDropPoint = true;
    //            currentDropPoint = hitCollider.transform;
    //            break;
    //        }
    //    }
    //}

    ///// <summary>
    ///// Overlap box içinde taþýnabilir objeleri tespit eder ve vurgular.
    ///// </summary>
    //private void UpdateHighlighting()
    //{
    //    // OverlapBox'ýn merkezini hesapla
    //    Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);

    //    // Etrafýmýzdaki taþýnabilir objeleri bul
    //    Collider[] hitColliders = Physics.OverlapBox(
    //        boxCenter,
    //        overlapBoxSize / 2,
    //        transform.rotation,
    //        interactLayer
    //    );

    //    HashSet<GameObject> currentlyDetected = new HashSet<GameObject>();

    //    foreach (var hitCollider in hitColliders)
    //    {
    //        if (hitCollider.CompareTag("Portable"))
    //        {
    //            currentlyDetected.Add(hitCollider.gameObject);

    //            if (!highlightedObjects.Contains(hitCollider.gameObject) && hitCollider.gameObject != carriedObject)
    //            {
    //                // Highlight the object with CanCarry state
    //                PortableObject portableObject = hitCollider.GetComponent<PortableObject>();
    //                if (portableObject != null)
    //                {
    //                    portableObject.SetHighlight(HighlightState.CanCarry); // Taþýma ihtimali durumu
    //                    highlightedObjects.Add(hitCollider.gameObject);
    //                    Debug.Log("Objeyi vurguladýn (CanCarry): " + hitCollider.gameObject.name);
    //                }
    //            }
    //        }
    //    }

    //    // Find objects that are no longer detected and unhighlight them
    //    HashSet<GameObject> objectsToUnhighlight = new HashSet<GameObject>(highlightedObjects);
    //    objectsToUnhighlight.ExceptWith(currentlyDetected);

    //    foreach (var obj in objectsToUnhighlight)
    //    {
    //        PortableObject portableObject = obj.GetComponent<PortableObject>();
    //        if (portableObject != null)
    //        {
    //            portableObject.SetHighlight(HighlightState.None);
    //            highlightedObjects.Remove(obj);
    //            Debug.Log("Objenin vurgusunu kaldýrdýn (None): " + obj.name);
    //        }
    //    }
    //}

    ///// <summary>
    ///// Taþýnan objenin pozisyonunu günceller.
    ///// </summary>
    //private void UpdateCarriedObjectPosition()
    //{
    //    if (isNearDropPoint && currentDropPoint != null)
    //    {
    //        // Taþýnan objeden PortableObject bileþenini al
    //        PortableObject portableObject = carriedObject.GetComponent<PortableObject>();
    //        if (portableObject != null)
    //        {
    //            float yOffset = portableObject.dropYOffset;
    //            Vector3 targetPosition = currentDropPoint.position + new Vector3(0, yOffset, 0); // yOffset

    //            // Objenin carryPoint'e göre yerel pozisyonunu hesapla
    //            Vector3 localPosition = carryPoint.InverseTransformPoint(targetPosition);

    //            carriedObject.transform.localPosition = localPosition;
    //            carriedObject.transform.rotation = currentDropPoint.rotation;

    //            // Debug bilgisi ekle
    //            Debug.Log($"Objeyi drop point'e yerleþtiriliyor: Position={carriedObject.transform.position}, Rotation={carriedObject.transform.rotation}");
    //        }
    //        else
    //        {
    //            Debug.LogError("Carried object does not have a PortableObject component!");
    //        }
    //    }
    //    else
    //    {
    //        // Objenin pozisyonunu carryPoint'e göre ayarla
    //        carriedObject.transform.localPosition = carriedObjectOriginalLocalPosition;
    //        carriedObject.transform.localRotation = Quaternion.identity;

    //        // Debug bilgisi ekle
    //        Debug.Log($"Objeyi carryPoint'e göre konumlandýrýlýyor: LocalPosition={carriedObject.transform.localPosition}");
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;

    //    // OverlapBox'ýn merkezini hesapla
    //    Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);

    //    // OverlapBox'ý Scene view'da çiz
    //    Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
    //    Gizmos.DrawWireCube(Vector3.zero, overlapBoxSize);
    //}
}
