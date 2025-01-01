using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WineStation : PlacableInteractable, IHoldInteractable
{
    private bool isFilling = false;
    private float fillProgress = 0f;
    private float fillDuration = 4f; 
    private WineGlass glassBeingFilled;
    private Animator glassAnimator;

    public Image fillProgressUI; 
    private bool isClockVisible = false;
    private bool isFillStart = false;
    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (playerInteraction.CarriedObject != null)
            {
              
                WineGlass wineGlass = playerInteraction.CarriedObject.GetComponent<WineGlass>();
                if (wineGlass != null)
                {
                    if (wineGlass.CurrentState == WineGlass.GlassState.CleanEmpty)
                    {
                        if (placedObject == null)
                        {
                            
                            base.Interact(player); 
                            Debug.Log("Placed a clean, empty wine glass on the wine station.");
                        }
                        else
                        {
                            Debug.Log("Cannot place object. Placement point is already occupied.");
                        }
                    }
                    else
                    {
                        Debug.Log("You can only place a clean, empty wine glass here.");
                    }
                }
                else
                {
                    Debug.Log("You need to carry a wine glass to place it here.");
                }
            }
            else if (playerInteraction.CarriedObject == null)
            {
                if (placedObject != null)
                {
                    
                    if (!isFilling)
                    {
                        base.Interact(player); 
                        Debug.Log("Picked up the wine glass from the wine station.");
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

    public bool CanHoldInteract(GameObject player)
    {
       
        if (placedObject != null)
        {
            
            WineGlass wineGlass = placedObject.GetComponent<WineGlass>();

            if (isFilling)
            {
                return true;
            }
            else if (wineGlass != null && wineGlass.CurrentState == WineGlass.GlassState.CleanEmpty)
            {
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
            Debug.LogError("PlayerAnimationController is missing on player!");
            return;
        }

        if (isFilling)
        {
            fillProgress += deltaTime;
            if (fillProgress > fillDuration)
            {
                fillProgress = fillDuration;
            }
            UpdateFillProgressUI();

            if (glassAnimator != null)
            {
                float normalizedTime = fillProgress / fillDuration;
                glassAnimator.Play("WineFill", 0, normalizedTime);
                glassAnimator.speed = 0f; 
                if (isFillStart)
                {
                    isFillStart = false;
                    animationController.SetFillingBeer(false);
                }
                player.GetComponent<Animator>().Play("FillBeer", 0, normalizedTime);
            }

            if (fillProgress >= fillDuration)
            {
                isFilling = false;
                glassBeingFilled.Fill();
                glassAnimator.Play("WineFill", 0, 1f); 
                glassAnimator.speed = 0f;
                if (isFillStart)
                {
                    isFillStart = false;
                    animationController.SetFillingBeer(false);
                }
                glassBeingFilled = null;
                Debug.Log("Finished filling the wine glass.");

                if (fillProgressUI != null)
                {
                    fillProgressUI.gameObject.SetActive(false);
                }
                isClockVisible = false;
            }
        }
        else
        {
            isFilling = true;
            fillProgress = 0f;
            glassBeingFilled = placedObject.GetComponent<WineGlass>();

            glassAnimator = glassBeingFilled.GetComponent<Animator>();
            if (glassAnimator != null)
            {
                glassAnimator.Play("WineFill", 0, 0f);
                glassAnimator.speed = 0f; 
                                          
            }
            if (!isFillStart)
            {
                isFillStart = true;
                animationController.SetFillingBeer(true);
                animationController.TriggerFillingBeer();
            }
            Debug.Log("Started filling the wine glass. Hold Ctrl for " + fillDuration + " seconds.");

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
                WineGlass wineGlass = playerInteraction.CarriedObject.GetComponent<WineGlass>();
                return wineGlass != null && wineGlass.CurrentState == WineGlass.GlassState.CleanEmpty && placedObject == null;
            }
            else
            {
                if (placedObject != null)
                {
                    return !isFilling;
                }
            }
        }
        return false;
    }
}