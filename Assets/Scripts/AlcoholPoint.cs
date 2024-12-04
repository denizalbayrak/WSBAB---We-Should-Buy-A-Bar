using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlcoholPoint : PlacableInteractable, IHoldInteractable
{
    private bool isFilling = false;
    private bool isCompleted = false;
    private float fillProgress = 0f;
    private float fillDuration = 4f;
    private MojitoGlass mojitoBeingFilled;
    public Animator alcoholPointAnimator;

    // UI Elements
    public Image fillProgressUI; // Assign this in the Inspector (clock image)
    private bool isClockVisible = false;
    private bool isFillStart = false;
   
    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
                // Player is carrying an object
                if (playerInteraction.CarriedObject.GetComponent<MojitoGlass>())
                {                
                MojitoGlass lime = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                if (lime != null)
                {                   
                        if (placedObject == null)
                        {
                            base.Interact(player);
                            Debug.Log("Placed a mojito glass on the alcohol fill station.");
                        }
                        else
                        {
                            Debug.Log("Cannot place object. Placement point is already occupied.");
                        }                  
                }
                else
                {
                    Debug.Log("You need to carry a mojito glass to place it here.");
                }
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
                        Debug.Log("Picked up the mojito from the alcohol station.");
                    }
                    else
                    {
                        Debug.Log("Cannot pick up the glass. Filling in progress.");
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
            // Get the MojitoGlass component from placedObject
            MojitoGlass mojitoGlass = placedObject.GetComponent<MojitoGlass>();

            if (isFilling)
            {
                // Allow holding if filling is in progress
                return true;
            }
            else if (mojitoGlass != null)
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
        if (isCompleted)
        {
            return;
        }

        if (isFilling)
        {
            // Filling process in progress
            fillProgress += deltaTime;
            if (fillProgress > fillDuration)
            {
                fillProgress = fillDuration;
            }
            // Update the fill progress UI
            UpdateFillProgressUI();

            // Update the Animator's playback time
            if (placedObject != null)
            {
                float normalizedTime = fillProgress / fillDuration;

                if (isFillStart)
                {
                    isFillStart = false;
                    animationController.SetFillingBeer(false);
                }
                player.GetComponent<Animator>().Play("FillBeer", 0, normalizedTime);
                alcoholPointAnimator.Play("Fill", 0, normalizedTime);
            }

            if (fillProgress >= fillDuration)
            {
                isFilling = false;
                mojitoBeingFilled.AddJuice();
              
                if (isFillStart)
                {
                    isFillStart = false;
                    alcoholPointAnimator.Play("Fill", 0, 1f); // Ensure animation is complete
                    alcoholPointAnimator.speed = 0f;
                    animationController.SetFillingBeer(false);
                }
                mojitoBeingFilled = null;
                Debug.Log("Finished filling the wine glass.");

                if (fillProgressUI != null)
                {
                    fillProgressUI.gameObject.SetActive(false);
                }
                isClockVisible = false;
                isCompleted = true;
            }
        }
        else
        {
            if (!isCompleted)
            {
                isFilling = true;
                fillProgress = 0f;
                mojitoBeingFilled = placedObject.GetComponent<MojitoGlass>();

                if (!isFillStart)
                {
                    isFillStart = true;
                    alcoholPointAnimator.Play("Fill", 0, 0f);
                    alcoholPointAnimator.speed = 0f;
                    animationController.SetFillingBeer(true);
                    animationController.TriggerFillingBeer();
                }
                Debug.Log("Started filling the wine glass. Hold Ctrl for " + fillDuration + " seconds.");

                // Show the fill progress UI
                if (fillProgressUI != null)
                {
                    fillProgressUI.gameObject.SetActive(true);
                }
                isClockVisible = true;
            }
           
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
                MojitoGlass mojitoGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                return mojitoGlass != null && placedObject == null;
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

