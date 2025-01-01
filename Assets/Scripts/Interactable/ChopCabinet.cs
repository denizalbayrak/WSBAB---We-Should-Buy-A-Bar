using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChopCabinet : PlacableInteractable, IHoldInteractable
{
    private bool isChopping = false;
    private float chopProgress = 0f;
    private float chopDuration = 4f;

    private IChoppable choppableItem;

    public Image fillProgressUI; 
    private bool isClockVisible = false;
    private bool isChopStart = false;
    public GameObject knife;

    public override void Interact(GameObject player)
    {
        var animationController = player.GetComponent<PlayerAnimator>();
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            GameObject carriedObject = playerInteraction.CarriedObject;

            if (carriedObject != null)
            {
                IChoppable choppable = carriedObject.GetComponent<IChoppable>();
                if (choppable != null && placedObject == null)
                {
                    
                    base.Interact(player);
                    Debug.Log($"Placed a {carriedObject.name} on the chop station. (Full: {choppable.IsFull}, Chopped: {choppable.IsChopped})");
                }
                else
                {
                    Debug.Log("You need to carry a choppable item (lime, orange, chocolate) to place it here or the placement point is already occupied.");
                }

               
                if (placedObject != null && placedObject.GetComponent<Lime>() != null)
                {
                    Lime lime = placedObject.GetComponent<Lime>();
                    if (lime.IsChopped)
                    {
                        if (playerInteraction.CarriedObject != null && playerInteraction.CarriedObject.GetComponent<MojitoGlass>() != null)
                        {
                            MojitoGlass mojitoGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                            if (mojitoGlass != null && !mojitoGlass.HasLime)
                            {
                                mojitoGlass.AddLime();
                                placedObject.gameObject.SetActive(false);
                                placedObject = null;
                                Debug.Log("Added a chopped Lime to the MojitoGlass.");
                            }
                        }
                        
                    }
                } 
                if (placedObject != null && placedObject.GetComponent<Chocolate>() != null)
                {
                    Chocolate chocolate = placedObject.GetComponent<Chocolate>();
                    if (chocolate.IsChopped)
                    {
                        if (playerInteraction.CarriedObject != null && playerInteraction.CarriedObject.GetComponent<WhiskeyGlass>() != null)
                        {
                            WhiskeyGlass whiskeyGlass = playerInteraction.CarriedObject.GetComponent<WhiskeyGlass>();
                        if (whiskeyGlass != null && !whiskeyGlass.HasChocolate)
                        {
                            whiskeyGlass.AddChocolate();
                            placedObject.gameObject.SetActive(false);
                            placedObject = null;
                            Debug.Log("Added a chopped Chocolate to the WhiskeyGlass.");
                        }
                        }
                    }
                }
                }

            else
            {
                if (placedObject != null)
                {
                    if (!isChopping)
                    {
                        base.Interact(player); 
                        Debug.Log("Picked up the item from the chop station.");
                        animationController.SetChopping(false);
                        isChopStart = false;
                        isChopping = false;
                        chopProgress = 0f;
                        Debug.Log("Chopping status has been reset.");
                    }
                    else
                    {
                        Debug.Log("Cannot pick up the item. Chopping in progress.");
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
            IChoppable choppable = placedObject.GetComponent<IChoppable>();
            if (choppable != null)
            {
                if (isChopping)
                {
     
                    return true;
                }
                else if (choppable.IsFull)
                {
                   
                    return true;
                }
              
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
            knife.SetActive(false);
            chopProgress += deltaTime;
            if (chopProgress > chopDuration)
            {
                chopProgress = chopDuration;
            }
            UpdateFillProgressUI();

            if (placedObject != null)
            {
                float normalizedTime = chopProgress / chopDuration;
                if (isChopStart)
                {
                    isChopStart = false;
                    if (choppableItem != null)
                    {
                        if (choppableItem is Lime)
                        {
                            animationController.SetChopping(false);
                        }
                        else if (choppableItem is Chocolate)
                        {
                            animationController.SetChopping(false);
                        }
                    }
                }
                player.GetComponent<Animator>().Play("Chop", 0, normalizedTime);
            }

            if (chopProgress >= chopDuration)
            {
                isChopping = false;

                if (choppableItem != null)
                {
                    choppableItem.Chop();
                    choppableItem = null;
                }

                if (isChopStart)
                {
                    isChopStart = false;
                    animationController.SetChopping(false);
                }
                Debug.Log("Finished chopping the item.");
                knife.SetActive(true);
                if (fillProgressUI != null)
                {
                    fillProgressUI.gameObject.SetActive(false);
                }
                isClockVisible = false;
            }
        }
        else
        {
            knife.SetActive(true);
            choppableItem = placedObject != null ? placedObject.GetComponent<IChoppable>() : null;

            if (choppableItem != null && choppableItem.IsFull)
            {
                isChopping = true;
                chopProgress = 0f;

                if (!isChopStart)
                {
                    isChopStart = true;
                    animationController.SetChopping(true);
                    if (choppableItem is Lime)
                    {
                        animationController.TriggerChopping();
                    }
                    else if (choppableItem is Chocolate)
                    {
                        animationController.TriggerChopping();
                    }
               
                }
                Debug.Log("Started chopping. Hold Ctrl for " + chopDuration + " seconds.");

                if (fillProgressUI != null)
                {
                    fillProgressUI.gameObject.SetActive(true);
                }
                isClockVisible = true;
            }
            else
            {
                Debug.Log("Object cannot be chopped (item is not full).");
            }
        }
    }

    private void UpdateFillProgressUI()
    {
        if (fillProgressUI != null)
        {
            float fillAmount = chopProgress / chopDuration;
            fillProgressUI.fillAmount = fillAmount;
        }
    }

    public override bool CanInteract(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            GameObject carriedObject = playerInteraction.CarriedObject;
            if (carriedObject != null)
            {
                IChoppable choppable = carriedObject.GetComponent<IChoppable>();
                if (choppable != null && placedObject == null)
                {
                    return true;
                }
                if (playerInteraction.CarriedObject.GetComponent<MojitoGlass>() != null)
                {
                    return true;
                } 
                if (playerInteraction.CarriedObject.GetComponent<WhiskeyGlass>() != null)
                {
                    return true;
                }
            }
            else
            {
                if (placedObject != null)
                {
                    knife.SetActive(true);
                    
                    return !isChopping;
                }
            }
        }
        return false;
    }
}
