using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public LayerMask interactionLayerMask;
    public Vector3 boxSize = new Vector3(1f, 1f, 1f); // Etkile�im alan�n�n boyutu
    public Vector3 boxOffset = new Vector3(0f, 0.5f, 1f); // Oyuncuya g�re etkile�im alan�n�n konumu

    private PlayerInputActions inputActions;
    private InputAction moveAction;
    private InputAction interactAction;
    private Vector2 moveInput;
    private Rigidbody rb;
    public float speed = 5f;

    private PlayerCarry playerCarry;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();

        moveAction = inputActions.Player.Move;
        interactAction = inputActions.Player.Interact;

        moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => moveInput = Vector2.zero;

        interactAction.performed += ctx => Interact();

        playerCarry = GetComponent<PlayerCarry>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);

        // Oyuncunun bakt��� y�n� hareket y�n�ne �evir
        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }
    }

    private void Interact()
    {
        Vector3 boxCenter = transform.position + transform.TransformDirection(boxOffset);

        Collider[] hitColliders = Physics.OverlapBox(
            boxCenter,
            boxSize / 2f,
            transform.rotation,
            interactionLayerMask
        );

        IInteractable closestInteractable = null;
        Counter closestCounter = null;
        float closestDistance = Mathf.Infinity;

        foreach (var collider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                // �ncelikle Interactable nesneleri kontrol ediyoruz
                IInteractable interactable = collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                    continue;
                }

                // E�er Interactable de�ilse, Counter olup olmad���n� kontrol ediyoruz
                Counter counter = collider.GetComponent<Counter>();
                if (counter != null)
                {
                    closestDistance = distance;
                    closestCounter = counter;
                }
            }
        }

        if (closestInteractable != null)
        {
            // Etkile�imli nesneyle etkile�ime gir
            closestInteractable.Interact(gameObject);
        }
        else if (closestCounter != null && playerCarry != null && playerCarry.IsCarrying)
        {
            // Tezgah ile etkile�ime girme senaryolar�n� kontrol et
            HandleCounterInteraction(closestCounter);
        }
        else if (playerCarry != null && playerCarry.IsCarrying)
        {
            // �n�n�zde hi�bir �ey yok, nesneyi b�rak�n
            playerCarry.Drop();
        }
    }
    private void HandleCounterInteraction(Counter counter)
    {
        CarryableObject carriedObject = playerCarry.GetCarriedObject();

        if (counter.CanInteractWith(carriedObject))
        {
            // Senaryo 3: Nesne tezgahla etkile�ime girer
            // Burada etkile�im mant���n�z� uygulay�n
            // �rne�in, nesneyi tezgaha yerle�tirin ve etkile�imi ba�lat�n
            if (counter.PlaceObject(carriedObject))
            {
                playerCarry.Drop(counter.transform.position + Vector3.up * 0.5f, Quaternion.identity);
                // Ekstra etkile�im mant��� ekleyin
            }
            else
            {
                // Tezgah doluysa veya ba�ka bir nedenle nesne yerle�tirilemezse
                playerCarry.Drop();
            }
        }
        else if (!counter.IsOccupied)
        {
            // Senaryo 2: Tezgah bo�, nesneyi tezgah�n ortas�na yerle�tir
            if (counter.PlaceObject(carriedObject))
            {
                playerCarry.Drop(counter.transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }
            else
            {
                // Tezgaha yerle�tirilemezse, nesneyi yere b�rak
                playerCarry.Drop();
            }
        }
        else
        {
            // Tezgah dolu ve nesne etkile�ime giremiyor, nesneyi yere b�rak
            playerCarry.Drop();
        }
    }
    private void OnDrawGizmos()
    {
        // Etkile�im alan�n� sahnede g�rselle�tirmek i�in
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f); // Yar� saydam k�rm�z�
        Vector3 boxCenter = transform.position + transform.TransformDirection(boxOffset);
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, boxSize);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
