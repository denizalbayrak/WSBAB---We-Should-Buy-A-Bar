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
    //public float rotationSpeed = 720f; // Derece cinsinden d�n�� h�z�

    //private Animator animator; // Animator referans�

    //// Ta��nan obje referans�
    //private GameObject carriedObject = null;

    //// Ta��nan objenin orijinal localPosition de�eri
    //private Vector3 carriedObjectOriginalLocalPosition;

    //// Obje b�rakma noktas�na yak�nken mi?
    //private bool isNearDropPoint = false;

    //// Yak�ndaki b�rakma noktas� referans�
    //private Transform currentDropPoint;

    //// Interact LayerMask
    //public LayerMask interactLayer;

    //// OverlapBox parametreleri Inspector �zerinden ayarlanabilir
    //public Vector3 overlapBoxSize = new Vector3(1f, 1f, 1f);
    //public Vector3 overlapBoxOffset = new Vector3(0f, 0f, 1.5f);

    //// Ta��ma noktas� (carryPoint) referans�
    //public Transform carryPoint;

    //// B�rakma noktas� LayerMask
    //public LayerMask dropPointLayer; // B�rakma noktalar�n�n bulundu�u Layer

    //// DropPoints GameObject referans�
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

    //    animator = GetComponent<Animator>(); // Animator bile�enini al
    //    if (animator == null)
    //    {
    //        Debug.LogError("Animator component is missing on the player!");
    //    }

    //    // DropPoints'� etiketle bul
    //    dropPoints = GameObject.FindGameObjectWithTag("DropPoints");
    //    if (dropPoints != null)
    //    {
    //        dropPoints.SetActive(false); // Ba�lang��ta devre d��� b�rak
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
    //    // Hareket i�lemleri
    //    Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y).normalized;
    //    Vector3 newPosition = transform.position + movement * speed * Time.fixedDeltaTime;
    //    rb.MovePosition(newPosition);

    //    // Oyuncunun bakt��� y�n� hareket y�n�ne do�ru yumu�ak bir �ekilde �evir
    //    if (movement != Vector3.zero)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
    //        transform.rotation = Quaternion.RotateTowards(
    //            transform.rotation,
    //            targetRotation,
    //            rotationSpeed * Time.fixedDeltaTime
    //        );
    //    }

    //    // Animasyonlar� g�ncelle
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
    ///// Animasyon parametrelerini g�nceller.
    ///// </summary>
    ///// <param name="movement">Hareket vekt�r�.</param>
    //private void UpdateAnimations(Vector3 movement)
    //{
    //    if (animator != null)
    //    {
    //        bool isWalking = movement.magnitude > 0.1f;
    //        animator.SetBool("isWalking", isWalking);
    //    }
    //}

    ///// <summary>
    ///// Interact aksiyonu ger�ekle�ti�inde �a�r�l�r.
    ///// </summary>
    //private void Interact()
    //{
    //    // Oyun durumunu kontrol et
    //    if (GameManager.Instance.currentGameState == GameState.InGame || GameManager.Instance.currentGameState == GameState.PreLevel)
    //    {
    //        // Oyun Normal Level veya PreLevel'de, objeleri ta��yabiliriz
    //        if (carriedObject == null)
    //        {
    //            // Obje almaya �al��
    //            TryPickUpObject();
    //        }
    //        else
    //        {
    //            // Objeyi b�rakmay� dene
    //            TryDropObject();
    //        }
    //    }
    //    else
    //    {
    //        // Di�er durumlarda objeleri ta��yamazs�n�z
    //        Debug.Log("�u anda objeleri ta��yamazs�n.");
    //    }
    //}

    ///// <summary>
    ///// Objeyi ta��maya �al���r.
    ///// </summary>
    //private void TryPickUpObject()
    //{
    //    // OverlapBox'�n merkezini hesapla
    //    Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);

    //    // Etraf�m�zdaki ta��nabilir objeleri bul
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
    //            // Objeyi ta��
    //            PickUpObject(hitCollider.gameObject);
    //            break;
    //        }
    //        // Ba�ka etkile�imler i�in ek ko�ullar ekleyebilirsiniz
    //    }
    //}

    ///// <summary>
    ///// Objeyi ta��maya ba�lar.
    ///// </summary>
    ///// <param name="obj">Ta��nacak obje.</param>
    //private void PickUpObject(GameObject obj)
    //{
    //    carriedObject = obj;
    //    obj.transform.SetParent(carryPoint); // Objeyi carryPoint'in child'� yap

    //    // Objenin pozisyonunu ayarla ve orijinal localPosition de�erini kaydet
    //    obj.transform.localPosition = carriedObjectOriginalLocalPosition = Vector3.zero;
    //    obj.transform.localRotation = Quaternion.identity;

    //    // Objeyi ta��rken collider'�n� devre d��� b�rak
    //    Collider objCollider = obj.GetComponent<Collider>();
    //    if (objCollider != null)
    //    {
    //        objCollider.enabled = false;
    //    }

    //    Debug.Log("Objeyi ta��yorsun: " + obj.name);

    //    // DropPoints'� etkinle�tir
    //    if (dropPoints != null)
    //    {
    //        dropPoints.SetActive(true);
    //        Debug.Log("DropPoints etkinle�tirildi.");
    //    }

    //    // Ta��ma s�ras�nda objeyi daha koyu renge getir
    //    PortableObject portableObject = obj.GetComponent<PortableObject>();
    //    if (portableObject != null)
    //    {
    //        portableObject.SetHighlight(HighlightState.Carried); // Ta��ma durumu
    //        highlightedObjects.Remove(obj); // Ta��ma s�ras�nda overlap box vurgusunu kald�r
    //        Debug.Log("Objeyi ta��rken rengini daha koyu hale getirdin: " + obj.name);
    //    }
    //}

    ///// <summary>
    ///// Objeyi b�rakmay� dener.
    ///// </summary>
    //private void TryDropObject()
    //{
    //    if (isNearDropPoint && currentDropPoint != null)
    //    {
    //        // Objeyi b�rak
    //        DropObject(currentDropPoint.position);
    //    }
    //    else
    //    {
    //        // Yak�nda b�rakma noktas� yok, objeyi b�rakamazs�n
    //        Debug.Log("Burada objeyi b�rakamazs�n.");
    //        // �ste�e ba�l� olarak oyuncuya g�rsel veya i�itsel geri bildirim sa�layabilirsiniz
    //    }
    //}

    ///// <summary>
    ///// Ta��nan objeyi b�rak�r ve b�rakma noktas�na yerle�tirir.
    ///// </summary>
    ///// <param name="dropPosition">Objenin b�rak�laca�� pozisyon.</param>
    //private void DropObject(Vector3 dropPosition)
    //{
    //    // Ta��nan objeden PortableObject bile�enini al
    //    PortableObject portableObject = carriedObject.GetComponent<PortableObject>();
    //    if (portableObject != null)
    //    {
    //        float yOffset = portableObject.dropYOffset;

    //        // Y offset'ini uygulayarak pozisyonu g�ncelle
    //        dropPosition.y += yOffset;
    //    }
    //    else
    //    {
    //        Debug.LogError("Carried object does not have a PortableObject component!");
    //    }

    //    carriedObject.transform.SetParent(null);
    //    carriedObject.transform.position = dropPosition;
    //    carriedObject.transform.rotation = currentDropPoint.rotation;

    //    // Objenin collider'�n� tekrar etkinle�tir
    //    Collider objCollider = carriedObject.GetComponent<Collider>();
    //    if (objCollider != null)
    //    {
    //        objCollider.enabled = true;
    //    }

    //    Debug.Log("Objeyi b�rakt�n: " + carriedObject.name);

    //    // DropPoints'� devre d��� b�rak
    //    if (dropPoints != null)
    //    {
    //        dropPoints.SetActive(false);
    //        Debug.Log("DropPoints devre d��� b�rak�ld�.");
    //    }

    //    // Objeyi b�rakt�ktan sonra rengini geri d�nd�r
    //    if (portableObject != null)
    //    {
    //        portableObject.SetHighlight(HighlightState.None);
    //        Debug.Log("Objeyi b�rakt�ktan sonra rengini orijinal haline d�nd�rd�n: " + carriedObject.name);
    //    }

    //    carriedObject = null;
    //    currentDropPoint = null;
    //    isNearDropPoint = false;
    //}

    ///// <summary>
    ///// Yak�ndaki b�rakma noktalar�n� kontrol eder.
    ///// </summary>
    //private void CheckForNearbyDropPoint()
    //{
    //    // OverlapBox'�n merkezini hesapla
    //    Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);

    //    // Etraf�m�zdaki b�rakma noktalar�n� bul
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
    //            // B�rakma noktas�n� bulduk
    //            isNearDropPoint = true;
    //            currentDropPoint = hitCollider.transform;
    //            break;
    //        }
    //    }
    //}

    ///// <summary>
    ///// Overlap box i�inde ta��nabilir objeleri tespit eder ve vurgular.
    ///// </summary>
    //private void UpdateHighlighting()
    //{
    //    // OverlapBox'�n merkezini hesapla
    //    Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);

    //    // Etraf�m�zdaki ta��nabilir objeleri bul
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
    //                    portableObject.SetHighlight(HighlightState.CanCarry); // Ta��ma ihtimali durumu
    //                    highlightedObjects.Add(hitCollider.gameObject);
    //                    Debug.Log("Objeyi vurgulad�n (CanCarry): " + hitCollider.gameObject.name);
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
    //            Debug.Log("Objenin vurgusunu kald�rd�n (None): " + obj.name);
    //        }
    //    }
    //}

    ///// <summary>
    ///// Ta��nan objenin pozisyonunu g�nceller.
    ///// </summary>
    //private void UpdateCarriedObjectPosition()
    //{
    //    if (isNearDropPoint && currentDropPoint != null)
    //    {
    //        // Ta��nan objeden PortableObject bile�enini al
    //        PortableObject portableObject = carriedObject.GetComponent<PortableObject>();
    //        if (portableObject != null)
    //        {
    //            float yOffset = portableObject.dropYOffset;
    //            Vector3 targetPosition = currentDropPoint.position + new Vector3(0, yOffset, 0); // yOffset

    //            // Objenin carryPoint'e g�re yerel pozisyonunu hesapla
    //            Vector3 localPosition = carryPoint.InverseTransformPoint(targetPosition);

    //            carriedObject.transform.localPosition = localPosition;
    //            carriedObject.transform.rotation = currentDropPoint.rotation;

    //            // Debug bilgisi ekle
    //            Debug.Log($"Objeyi drop point'e yerle�tiriliyor: Position={carriedObject.transform.position}, Rotation={carriedObject.transform.rotation}");
    //        }
    //        else
    //        {
    //            Debug.LogError("Carried object does not have a PortableObject component!");
    //        }
    //    }
    //    else
    //    {
    //        // Objenin pozisyonunu carryPoint'e g�re ayarla
    //        carriedObject.transform.localPosition = carriedObjectOriginalLocalPosition;
    //        carriedObject.transform.localRotation = Quaternion.identity;

    //        // Debug bilgisi ekle
    //        Debug.Log($"Objeyi carryPoint'e g�re konumland�r�l�yor: LocalPosition={carriedObject.transform.localPosition}");
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;

    //    // OverlapBox'�n merkezini hesapla
    //    Vector3 boxCenter = transform.position + transform.TransformDirection(overlapBoxOffset);

    //    // OverlapBox'� Scene view'da �iz
    //    Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
    //    Gizmos.DrawWireCube(Vector3.zero, overlapBoxSize);
    //}
}
