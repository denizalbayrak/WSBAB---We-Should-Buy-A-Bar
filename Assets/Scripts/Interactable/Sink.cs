using UnityEngine;
using UnityEngine.InputSystem;

public class Sink : PlacableInteractable
{
    private bool isWashing = false;
    private float washProgress = 0f;
    private float washDuration = 10f; // 10 seconds
    private BeerGlass glassBeingWashed;
    private PlayerInputActions inputActions;
    private InputAction holdAction;

    private void Awake()
    {
        // Initialize input actions
        inputActions = new PlayerInputActions();
        // Reference the hold action
        holdAction = inputActions.Player.InteractHold;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();

        if (playerInteraction != null && playerInteraction.CarriedObject != null)
        {
            // First, perform the base interaction (placement)
            base.Interact(player);

            // Get the placed object
            if (placedObject != null)
            {
                BeerGlass beerGlass = placedObject.GetComponent<BeerGlass>();
                if (beerGlass != null && beerGlass.CurrentState == BeerGlass.GlassState.DirtyEmpty)
                {
                    // Start washing process
                    isWashing = true;
                    washProgress = 0f;
                    glassBeingWashed = beerGlass;
                    Debug.Log("Started washing the beer glass. Hold Ctrl for 10 seconds.");
                }
                else
                {
                    Debug.Log("Placed a beer glass on the sink, but it's not dirty.");
                }
            }
        }
        else if (playerInteraction != null && playerInteraction.CarriedObject == null && placedObject != null)
        {
            // Player picks up the glass if not carrying anything
            base.Interact(player);
        }
        else
        {
            Debug.Log("Cannot interact with the sink.");
        }
    }

    private void Update()
    {
        if (isWashing)
        {
            // Check if the player is holding the Ctrl key via the input action
            if (holdAction.IsPressed())
            {
                washProgress += Time.deltaTime;
                // Optional: Update washing animation progress here
                if (washProgress >= washDuration)
                {
                    // Washing completed
                    isWashing = false;
                    glassBeingWashed.Clean();
                    glassBeingWashed = null;
                    Debug.Log("Finished washing the beer glass.");
                }
            }
            else
            {
                // Player released the Ctrl key
                if (washProgress > 0f)
                {
                    Debug.Log("Washing interrupted. Hold Ctrl continuously for 10 seconds to wash.");
                }
                washProgress = 0f;
            }
        }
    }


    public override bool CanInteract(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                // Can place a beer glass if the placement point is empty
                return placedObject == null && playerInteraction.CarriedObject.GetComponent<BeerGlass>() != null;
            }
            else
            {
                // Can pick up the glass if one is placed and not currently washing
                return placedObject != null && !isWashing;
            }
        }
        return false;
    }
}
