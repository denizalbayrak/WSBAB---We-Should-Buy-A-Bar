using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public LayerMask interactionLayerMask;
    public Vector3 boxSize = new Vector3(1f, 1f, 1f); // Etkileþim alanýnýn boyutu
    public Vector3 boxOffset = new Vector3(0f, 0.5f, 1f); // Oyuncuya göre etkileþim alanýnýn konumu

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

        // Oyuncunun baktýðý yönü hareket yönüne çevir
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
                // Öncelikle Interactable nesneleri kontrol ediyoruz
                IInteractable interactable = collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                    continue;
                }

                // Eðer Interactable deðilse, Counter olup olmadýðýný kontrol ediyoruz
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
            // Etkileþimli nesneyle etkileþime gir
            closestInteractable.Interact(gameObject);
        }
        else if (closestCounter != null && playerCarry != null && playerCarry.IsCarrying)
        {
            // Tezgah ile etkileþime girme senaryolarýný kontrol et
            HandleCounterInteraction(closestCounter);
        }
        else if (playerCarry != null && playerCarry.IsCarrying)
        {
            // Önünüzde hiçbir þey yok, nesneyi býrakýn
            playerCarry.Drop();
        }
    }
    private void HandleCounterInteraction(Counter counter)
    {
        CarryableObject carriedObject = playerCarry.GetCarriedObject();

        if (counter.CanInteractWith(carriedObject))
        {
            // Senaryo 3: Nesne tezgahla etkileþime girer
            // Burada etkileþim mantýðýnýzý uygulayýn
            // Örneðin, nesneyi tezgaha yerleþtirin ve etkileþimi baþlatýn
            if (counter.PlaceObject(carriedObject))
            {
                playerCarry.Drop(counter.transform.position + Vector3.up * 0.5f, Quaternion.identity);
                // Ekstra etkileþim mantýðý ekleyin
            }
            else
            {
                // Tezgah doluysa veya baþka bir nedenle nesne yerleþtirilemezse
                playerCarry.Drop();
            }
        }
        else if (!counter.IsOccupied)
        {
            // Senaryo 2: Tezgah boþ, nesneyi tezgahýn ortasýna yerleþtir
            if (counter.PlaceObject(carriedObject))
            {
                playerCarry.Drop(counter.transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }
            else
            {
                // Tezgaha yerleþtirilemezse, nesneyi yere býrak
                playerCarry.Drop();
            }
        }
        else
        {
            // Tezgah dolu ve nesne etkileþime giremiyor, nesneyi yere býrak
            playerCarry.Drop();
        }
    }
    private void OnDrawGizmos()
    {
        // Etkileþim alanýný sahnede görselleþtirmek için
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f); // Yarý saydam kýrmýzý
        Vector3 boxCenter = transform.position + transform.TransformDirection(boxOffset);
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, boxSize);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
