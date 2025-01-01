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
    private MimosaGlass mimosaBeingFilled;
    private WhiskeyGlass whiskeyBeingFilled; 

    public Animator alcoholPointAnimator;

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
                MojitoGlass mojitoGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                MimosaGlass mimosaGlass = playerInteraction.CarriedObject.GetComponent<MimosaGlass>();
                WhiskeyGlass whiskeyGlass = playerInteraction.CarriedObject.GetComponent<WhiskeyGlass>();

                if (mojitoGlass != null)
                {
                    if (placedObject == null)
                    {
                        base.Interact(player);
                        Debug.Log("Placed a Mojito glass on the alcohol fill station.");
                    }
                    else
                    {
                        Debug.Log("Cannot place object. Placement point is already occupied.");
                    }
                }
                else if (mimosaGlass != null)
                {
                    if (placedObject == null)
                    {
                        base.Interact(player);
                        Debug.Log("Placed a Mimosa glass on the alcohol fill station.");
                    }
                    else
                    {
                        Debug.Log("Cannot place object. Placement point is already occupied.");
                    }
                }
                else if (whiskeyGlass != null) 
                {
                    if (placedObject == null)
                    {
                        base.Interact(player);
                        Debug.Log("Placed a Whiskey glass on the alcohol fill station.");
                    }
                    else
                    {
                        Debug.Log("Cannot place object. Placement point is already occupied.");
                    }
                }


                else if (placedObject != null && placedObject.GetComponent<MojitoGlass>() != null)
                {
                    MojitoGlass mojitoGlassplaced = placedObject.GetComponent<MojitoGlass>();

                    if (playerInteraction.CarriedObject != null && playerInteraction.CarriedObject.GetComponent<Lime>() != null)
                    {
                        Lime lime = playerInteraction.CarriedObject.GetComponent<Lime>();
                        if (mojitoGlassplaced != null && !mojitoGlassplaced.HasLime && lime.CurrentState == Lime.LimeState.ChoppedLime)
                        {
                            mojitoGlassplaced.AddLime();
                            Destroy(playerInteraction.CarriedObject.gameObject);
                            playerInteraction.CarriedObject = null;
                            playerInteraction.isCarrying = false;
                            playerInteraction.animator.SetBool("isCarry", false);
                            Debug.Log("Added a chopped Lime to the MojitoGlass.");
                        }
                    }

                }
                
                else if (placedObject != null && placedObject.GetComponent<WhiskeyGlass>() != null)
                {
                    WhiskeyGlass whiskeyGlassplaced = placedObject.GetComponent<WhiskeyGlass>();

                    if (playerInteraction.CarriedObject != null && playerInteraction.CarriedObject.GetComponent<Chocolate>() != null)
                    {
                        Chocolate chocolate = playerInteraction.CarriedObject.GetComponent<Chocolate>();
                        if (whiskeyGlassplaced != null && !whiskeyGlassplaced.HasChocolate && chocolate.CurrentState == Chocolate.ChocolateState.ChoppedChocolate)
                        {
                            whiskeyGlassplaced.AddChocolate();
                           Destroy( playerInteraction.CarriedObject.gameObject);
                            playerInteraction.CarriedObject = null;
                            playerInteraction.isCarrying = false;
                            playerInteraction.animator.SetBool("isCarry", false);
                            Debug.Log("Added a chopped chocolate to the WhiskeyGlass.");
                        }
                    }
                }
                else
                {
                    Debug.Log("You need to carry a Mojito, Mimosa, or Whiskey glass to place it here.");
                }
            }
            else if (playerInteraction.CarriedObject == null)
            {
                if (placedObject != null)
                {
                    if (!isFilling)
                    {
                        base.Interact(player); 
                        Debug.Log("Picked up the glass from the alcohol station.");

                        isCompleted = false;
                        Debug.Log("isCompleted has been reset to false.");
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
            MojitoGlass mojitoGlass = placedObject.GetComponent<MojitoGlass>();
            MimosaGlass mimosaGlass = placedObject.GetComponent<MimosaGlass>();
            WhiskeyGlass whiskeyGlass = placedObject.GetComponent<WhiskeyGlass>(); 

            if (isFilling)
            {
                return true;
            }
            else if (mojitoGlass != null || mimosaGlass != null || whiskeyGlass != null)
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
            Debug.LogError("PlayerAnimator is missing on player!");
            return;
        }

        if (isCompleted)
        {
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

            if (placedObject != null)
            {
                float normalizedTime = fillProgress / fillDuration;

                if (isFillStart)
                {
                    isFillStart = false;
                    if (mojitoBeingFilled != null)
                    {
                        animationController.SetFillingBeer(false);
                    }
                    else if (mimosaBeingFilled != null)
                    {
                        animationController.SetFillingBeer(false);
                    }
                    else if (whiskeyBeingFilled != null)
                    {
                        animationController.SetFillingBeer(false);
                    }
                }
                player.GetComponent<Animator>().Play("Fill", 0, normalizedTime);
                alcoholPointAnimator.Play("Fill", 0, normalizedTime);
            }

            if (fillProgress >= fillDuration)
            {
                isFilling = false;

                if (mojitoBeingFilled != null)
                {
                    mojitoBeingFilled.AddJuice();
                    mojitoBeingFilled = null;
                }

                if (mimosaBeingFilled != null)
                {
                    mimosaBeingFilled.AddChampagne();
                    mimosaBeingFilled = null;
                }

                if (whiskeyBeingFilled != null)
                {
                    whiskeyBeingFilled.AddWhiskey();
                    whiskeyBeingFilled = null;
                }

                if (isFillStart)
                {
                    isFillStart = false;
                    alcoholPointAnimator.Play("Fill", 0, 1f); 
                    alcoholPointAnimator.speed = 0f;
                    if (mojitoBeingFilled != null)
                    {
                        animationController.SetFillingBeer(false);
                    }
                    else if (mimosaBeingFilled != null)
                    {
                        animationController.SetFillingBeer(false);
                    }
                    else if (whiskeyBeingFilled != null)
                    {
                        animationController.SetFillingBeer(false);
                    }
                }
                Debug.Log("Finished filling the glass.");

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
                mimosaBeingFilled = placedObject.GetComponent<MimosaGlass>();
                whiskeyBeingFilled = placedObject.GetComponent<WhiskeyGlass>();

                if (mojitoBeingFilled == null && mimosaBeingFilled == null && whiskeyBeingFilled == null)
                {
                    Debug.LogError("Placed object does not have MojitoGlass, MimosaGlass, or WhiskeyGlass component.");
                    isFilling = false;
                    return;
                }

                if (!isFillStart)
                {
                    isFillStart = true;
                    alcoholPointAnimator.Play("Fill", 0, 0f);
                    alcoholPointAnimator.speed = 0f;
                    if (mojitoBeingFilled != null)
                    {
                        animationController.SetFillingBeer(true);
                        animationController.TriggerFillingBeer();
                    }
                    else if (mimosaBeingFilled != null)
                    {
                        animationController.SetFillingBeer(true);
                        animationController.TriggerFillingBeer();
                    }
                    else if (whiskeyBeingFilled != null)
                    {
                        animationController.SetFillingBeer(true);
                        animationController.TriggerFillingBeer();
                    }
                }
                Debug.Log("Started filling the glass. Hold Ctrl for " + fillDuration + " seconds.");

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
                MojitoGlass mojitoGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                MimosaGlass mimosaGlass = playerInteraction.CarriedObject.GetComponent<MimosaGlass>();
                WhiskeyGlass whiskeyGlass = playerInteraction.CarriedObject.GetComponent<WhiskeyGlass>();
                bool canPlace = (mojitoGlass != null || mimosaGlass != null || whiskeyGlass != null) && placedObject == null;
                Debug.Log($"CanInteract (Carrying): {canPlace}");
                if (playerInteraction.CarriedObject.GetComponent<Lime>() != null)
                {
                    return true;
                }
                if (playerInteraction.CarriedObject.GetComponent<Chocolate>() != null)
                {
                    return true;
                }
                return canPlace;
            }
            else
            {
                if (placedObject != null)
                {
                    IBlendable blendable = placedObject.GetComponent<IBlendable>();
                    if (blendable != null && blendable.IsBlended)
                    {
                        Debug.Log("CanInteract: Blender has a blended item, but player is not carrying MimosaGlass.");
                        return false;
                    }
                    else
                    {
                        bool canPickUp = !isFilling;
                        Debug.Log($"CanInteract (Not Carrying): {canPickUp}");
                        return canPickUp;
                    }
                }
            }
        }
        Debug.Log("CanInteract: PlayerInteraction component is missing or conditions not met.");
        return false;
    }
}
