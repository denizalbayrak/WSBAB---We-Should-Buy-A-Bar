using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlenderStation : PlacableInteractable, IHoldInteractable
{
    private bool isBlending = false;
    private float blendProgress = 0f;
    private float blendDuration = 4f;

    private IBlendable blendableItem;

    public Image fillProgressUI;
    private bool isClockVisible = false;
    private bool isBlendStart = false;

    public GameObject emptyBlender;
    public GameObject orangeBlender;
    public Animator blenderAnimator;
    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction == null) return;

        GameObject carriedObject = playerInteraction.CarriedObject;

        if (carriedObject != null)
        {
            IBlendable blendable = carriedObject.GetComponent<IBlendable>();

            if (blendable != null && blendable.IsBlendable && !blendable.IsBlended && placedObject == null)
            {
                base.Interact(player);
                Debug.Log($"Placed a {carriedObject.name} in the blender.");

                Orange orange = placedObject.GetComponent<Orange>();
                if (orange != null && orange.IsChopped)
                {
                    placedObject.transform.GetChild(0).gameObject.SetActive(false);
                    emptyBlender.SetActive(false);
                    orangeBlender.SetActive(true);
                }
                else
                {
                    emptyBlender.SetActive(false);
                    orangeBlender.SetActive(true);
                }

            }
            else
            {
                Orange blendedObj = placedObject.GetComponent<Orange>();
                if (blendedObj != null && blendedObj.IsBlended)
                {
                    MimosaGlass mimosaGlass = playerInteraction.CarriedObject != null ? playerInteraction.CarriedObject.GetComponent<MimosaGlass>() : null;
                    if (mimosaGlass != null)
                    {
                        mimosaGlass.AddOrangeJuice();
                        Debug.Log("Transferred blended orange juice into MimosaGlass.");

                        Destroy(placedObject);
                        placedObject = null;

                        emptyBlender.SetActive(true);
                        orangeBlender.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("You need to have a MimosaGlass to pick up the blended juice.");
                    }
                }
                Debug.Log("You need to carry a chopped, blendable item (like chopped orange) to place it here.");
            }
        }
        else
        {
            if (placedObject != null)
            {
                if (isBlending)
                {
                    Debug.Log("Cannot pick up the item. Blending in progress.");
                    return;
                }

                IBlendable blendable = placedObject.GetComponent<IBlendable>();
                if (blendable != null && blendable.IsBlended)
                {
                    MimosaGlass mimosaGlass = playerInteraction.CarriedObject != null ? playerInteraction.CarriedObject.GetComponent<MimosaGlass>() : null;
                    if (mimosaGlass != null)
                    {
                        mimosaGlass.AddOrangeJuice();
                        Debug.Log("Transferred blended orange juice into MimosaGlass.");

                        Destroy(placedObject);
                        placedObject = null;

                        emptyBlender.SetActive(true);
                        orangeBlender.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("You need to have a MimosaGlass to pick up the blended juice.");
                    }
                }
              
            }
            else
            {
                Debug.Log("Nothing to pick up here.");
            }
        }
    }



    public bool CanHoldInteract(GameObject player)
    {
        if (placedObject != null)
        {
            IBlendable blendable = placedObject.GetComponent<IBlendable>();
            if (blendable != null)
            {
                if (isBlending)
                {
                    return true;
                }
                else if (blendable.IsBlendable && !blendable.IsBlended)
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

        if (isBlending)
        {
            blendProgress += deltaTime;
            if (blendProgress > blendDuration)
            {
                blendProgress = blendDuration;
            }
            UpdateFillProgressUI();

            if (placedObject != null)
            {
                float normalizedTime = blendProgress / blendDuration;
                if (isBlendStart)
                {
                    isBlendStart = false;
                    animationController.SetBlending(false);
                }
                player.GetComponent<Animator>().Play("BlenderAnim", 0, normalizedTime);
                blenderAnimator.Play("BlenderAnim", 0, normalizedTime);
            }

            if (blendProgress >= blendDuration)
            {
                isBlending = false;

                if (blendableItem != null)
                {
                    blendableItem.Blend();
                    blendableItem = null;
                }

                if (isBlendStart)
                {
                    isBlendStart = false;
                    animationController.SetBlending(false);
                    blenderAnimator.Play("BlenderAnim", 0, 1f);
                    blenderAnimator.speed = 0f;
                }
                Debug.Log("Finished blending the item.");

                if (fillProgressUI != null)
                {
                    fillProgressUI.gameObject.SetActive(false);
                }
                isClockVisible = false;
              
            }
        }
        else
        {
            blendableItem = placedObject != null ? placedObject.GetComponent<IBlendable>() : null;

            if (blendableItem != null && blendableItem.IsBlendable && !blendableItem.IsBlended)
            {
                isBlending = true;
                blendProgress = 0f;

                if (!isBlendStart)
                {
                    isBlendStart = true;
                    animationController.SetBlending(true);
                    animationController.TriggerBlending();
                }
                Debug.Log("Started blending. Hold Ctrl for " + blendDuration + " seconds.");

                if (fillProgressUI != null)
                {
                    fillProgressUI.gameObject.SetActive(true);
                }
                isClockVisible = true;
            }
            else
            {
                Debug.Log("Object cannot be blended (item is not blendable or already blended).");
            }
        }
    }


    private void UpdateFillProgressUI()
    {
        if (fillProgressUI != null)
        {
            float fillAmount = blendProgress / blendDuration;
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
                IBlendable blendable = carriedObject.GetComponent<IBlendable>();

                if (blendable != null && blendable.IsBlendable && !blendable.IsBlended && placedObject == null)
                {
                    return true;
                }

                MimosaGlass mimosaGlass = carriedObject.GetComponent<MimosaGlass>();
                if (mimosaGlass != null && placedObject != null)
                {
                    IBlendable blendableInBlender = placedObject.GetComponent<IBlendable>();
                    if (blendableInBlender != null && blendableInBlender.IsBlended)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (placedObject != null)
                {
                    IBlendable blendable = placedObject.GetComponent<IBlendable>();
                    if (blendable != null && blendable.IsBlended)
                    {
                        return false;
                    }
                    else
                    {
                        return !isBlending;
                    }
                }
            }
        }
        return false;
    }

}
