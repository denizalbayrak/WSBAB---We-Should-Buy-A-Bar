using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChopCabinet : PlacableInteractable, IHoldInteractable
{
    private bool isChopping = false;
    private float chopProgress = 0f;
    private float chopDuration = 4f;
    private Lime limeBeingChopped;

    // UI Elements
    public Image fillProgressUI; // Assign this in the Inspector (clock image)
    private bool isClockVisible = false;
    private bool isChopStart = false;

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                // Player is carrying an object
                Lime lime = playerInteraction.CarriedObject.GetComponent<Lime>();
                if (lime != null)
                {
                    if (lime.CurrentState == Lime.LimeState.FullLime)
                    {
                        if (placedObject == null)
                        {
                            // Place the lime on the station
                            base.Interact(player); // This places the object and sets placedObject
                            Debug.Log("Placed a clean, full lime on the chop station.");
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
                        Debug.Log("You can only place a full lime here.");
                    }
                }
                else
                {
                    Debug.Log("You need to carry a lime to place it here.");
                }
            }
            else if (playerInteraction.CarriedObject == null)
            {
                // Player is not carrying anything
                if (placedObject != null)
                {
                    // Check if filling has started
                    if (!isChopping)
                    {
                        // Player picks up the glass
                        base.Interact(player); // This picks up the placed object
                        Debug.Log("Picked up the lime from the chop station.");
                    }
                    else
                    {
                        Debug.Log("Cannot pick up the lime. Chopping in progress.");
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
            // Get the WineGlass component from placedObject
            Lime lime = placedObject.GetComponent<Lime>();

            if (isChopping)
            {
                // Allow holding if filling is in progress
                return true;
            }
            else if (lime != null && lime.CurrentState == Lime.LimeState.FullLime)
            {
                // Allow holding to start filling process
                return true;
            }
        }

        return false;
    }

    public void OnHoldInteract(GameObject player, float deltaTime)
    {
        var animationController = player.GetComponent<PlayerAnimator>();
        if (animationController == null)
        {
            Debug.LogError("PlayerAnimator is missing on player!");
            return;
        }

        if (isChopping)
        {
            // Filling process in progress
            chopProgress += deltaTime;
            if (chopProgress > chopDuration)
            {
                chopProgress = chopDuration;
            }
            // Update the fill progress UI
            UpdateFillProgressUI();

            // Update the Animator's playback time
            if (placedObject != null)
            {
                float normalizedTime = chopProgress / chopDuration;
            //    glassAnimator.Play("WineFill", 0, normalizedTime);
              //  glassAnimator.speed = 0f; // Keep the animator paused
                if (isChopStart)
                {
                    isChopStart = false;
                    animationController.SetChopping(false);
                }
                player.GetComponent<Animator>().Play("Chop", 0, normalizedTime);
            }

            if (chopProgress >= chopDuration)
            {
                // Filling completed
                isChopping = false;
                limeBeingChopped.Chop();
                //   glassAnimator.Play("WineFill", 0, 1f); // Ensure animation is complete
                //     glassAnimator.speed = 0f;
                if (isChopStart)
                {
                    isChopStart = false;
                    animationController.SetChopping(false);
                }
                //    PlayerInteraction.Instance.GetComponent<Animator>().Play("FillBeer", 0, 1f);
                limeBeingChopped = null;
                Debug.Log("Finished filling the wine glass.");

                // Hide the fill progress UI
                if (fillProgressUI != null)
                {
                    fillProgressUI.gameObject.SetActive(false);
                }
                isClockVisible = false;
            }
        }
        else
        {
            // Start filling process
            isChopping = true;
            chopProgress = 0f;
            limeBeingChopped = placedObject.GetComponent<Lime>();

            // Get the Animator component from the glass
            //glassAnimator = glassBeingFilled.GetComponent<Animator>();
            //if (glassAnimator != null)
            //{
            //    // Start the BeerFill animation at the beginning
            //    glassAnimator.Play("WineFill", 0, 0f);
            //    glassAnimator.speed = 0f; // Pause the animation initially
            //                              //    PlayerInteraction.Instance.GetComponent<Animator>().Play("FillBeer", 0, 0);
            //}
            if (!isChopStart)
            {
                isChopStart = true;
                animationController.SetChopping(true);
                animationController.TriggerChopping();
            }
            Debug.Log("Started filling the wine glass. Hold Ctrl for " + chopDuration + " seconds.");

            // Show the fill progress UI
            if (fillProgressUI != null)
            {
                fillProgressUI.gameObject.SetActive(true);
            }
            isClockVisible = true;
        }
    }


    private void UpdateFillProgressUI()
    {
        if (fillProgressUI != null)
        {
            // Assuming the UI Image is a circular fill (e.g., radial progress bar)
            float fillAmount = chopProgress / chopDuration;
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
                Lime lime = playerInteraction.CarriedObject.GetComponent<Lime>();
                return lime != null && lime.CurrentState == Lime.LimeState.FullLime && placedObject == null;
            }
            else
            {
                // Player is not carrying anything
                if (placedObject != null)
                {
                    // Can pick up the glass if filling hasn't started
                    return !isChopping;
                }
            }
        }
        return false;
    }
}
