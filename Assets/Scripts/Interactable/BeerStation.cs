using UnityEngine;
using UnityEngine.UI; // For UI elements

public class BeerStation : PlacableInteractable, IHoldInteractable
{
    private bool isFilling = false;
    private float fillProgress = 0f;
    private float fillDuration = 10f; // 10 seconds to fill
    private BeerGlass glassBeingFilled;
    private Animator glassAnimator;

    // UI Elements
    public Image fillProgressUI; // Assign this in the Inspector (clock image)
    private bool isClockVisible = false;

    // Removed inputActions and holdAction variables and methods

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                // Player is carrying an object
                BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
                if (beerGlass != null)
                {
                    if (beerGlass.CurrentState == BeerGlass.GlassState.CleanEmpty)
                    {
                        if (placedObject == null)
                        {
                            // Place the glass on the station
                            base.Interact(player); // This places the object and sets placedObject

                            Debug.Log("Placed a clean, empty beer glass on the beer station.");
                            // The filling process does NOT start automatically
                            // Player can start filling by holding the Ctrl key
                        }
                        else
                        {
                            Debug.Log("Cannot place object. Placement point is already occupied.");
                        }
                    }
                    else
                    {
                        Debug.Log("You can only place a clean, empty beer glass here.");
                    }
                }
                else
                {
                    Debug.Log("You need to carry a beer glass to place it here.");
                }
            }
            else if (playerInteraction.CarriedObject == null)
            {
                // Player is not carrying anything
                if (placedObject != null)
                {
                    // Check if filling has started
                    if (!isFilling)
                    {
                        // Player picks up the glass
                        base.Interact(player); // This picks up the placed object
                        Debug.Log("Picked up the beer glass from the beer station.");
                    }
                    else
                    {
                        Debug.Log("Cannot pick up the glass. Filling in progress.");
                        // Optionally, provide feedback to the player
                    }
                }
                else
                {
                    Debug.Log("Nothing to pick up here.");
                }
            }
        }
    }

    // Implement IHoldInteractable
    public bool CanHoldInteract(GameObject player)
    {
        // Check if there is a glass on the station
        if (placedObject != null)
        {
            // Get the BeerGlass component from placedObject
            BeerGlass beerGlass = placedObject.GetComponent<BeerGlass>();

            if (isFilling)
            {
                // Allow holding if filling is in progress
                return true;
            }
            else if (beerGlass != null && beerGlass.CurrentState == BeerGlass.GlassState.CleanEmpty)
            {
                // Allow holding to start filling process
                return true;
            }
        }

        return false;
    }

    public void OnHoldInteract(GameObject player, float deltaTime)
    {
        if (isFilling)
        {
            // Filling process in progress
            fillProgress += deltaTime;
            if (fillProgress > fillDuration)
            {
                fillProgress = fillDuration;
            }

            // Update the Animator's playback time
            if (glassAnimator != null)
            {
                float normalizedTime = fillProgress / fillDuration;
                glassAnimator.Play("BeerFill", 0, normalizedTime);
                glassAnimator.speed = 0f; // Keep the animator paused
            }

            // Update the fill progress UI
            UpdateFillProgressUI();

            if (fillProgress >= fillDuration)
            {
                // Filling completed
                isFilling = false;
                glassBeingFilled.Fill();
                glassAnimator.Play("BeerFill", 0, 1f); // Ensure animation is complete
                glassAnimator.speed = 0f;
                glassBeingFilled = null;
                Debug.Log("Finished filling the beer glass.");

                // Hide the fill progress UI
                fillProgressUI.gameObject.SetActive(false);
                isClockVisible = false;
            }
        }
        else
        {
            // Start filling process
            isFilling = true;
            fillProgress = 0f;
            glassBeingFilled = placedObject.GetComponent<BeerGlass>();

            // Get the Animator component from the glass
            glassAnimator = glassBeingFilled.GetComponent<Animator>();
            if (glassAnimator != null)
            {
                // Start the BeerFill animation at the beginning
                glassAnimator.Play("BeerFill", 0, 0f);
                glassAnimator.speed = 0f; // Pause the animation initially
            }

            Debug.Log("Started filling the beer glass. Hold Ctrl for 10 seconds.");

            // Show the fill progress UI
            fillProgressUI.gameObject.SetActive(true);
            isClockVisible = true;
        }
    }

    private void UpdateFillProgressUI()
    {
        if (fillProgressUI != null)
        {
            // Assuming the UI Image is a circular fill (e.g., radial progress bar)
            float fillAmount = fillProgress / fillDuration;
            fillProgressUI.fillAmount = fillAmount;
        }
    }

    public override bool CanInteract(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                // Player is carrying something
                BeerGlass beerGlass = playerInteraction.CarriedObject.GetComponent<BeerGlass>();
                return beerGlass != null && beerGlass.CurrentState == BeerGlass.GlassState.CleanEmpty && placedObject == null;
            }
            else
            {
                // Player is not carrying anything
                if (placedObject != null)
                {
                    // Can pick up the glass if filling hasn't started
                    return !isFilling;
                }
            }
        }
        return false;
    }
}
