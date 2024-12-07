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

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            GameObject carriedObject = playerInteraction.CarriedObject;

            if (carriedObject != null)
            {
                IBlendable blendable = carriedObject.GetComponent<IBlendable>();
                // BlenderStation'a hem full hem de chopped item koymayaca��z.
                // Sadece IsBlendable = true olan, yani chop edilmi� ama blend edilmemi� itemleri koyaca��z.
                // B�ylece blenderda sadece chop edilmi� itemler i�lenir.

                if (blendable != null && blendable.IsBlendable && !blendable.IsBlended && placedObject == null)
                {
                    // Blend edilebilir bir item ve daha �nce blend yap�lmam��
                    base.Interact(player);
                    Debug.Log($"Placed a {carriedObject.name} in the blender.");
                }
                else
                {
                    Debug.Log("You need to carry a blendable (and chopped) item to place it here.");
                }
            }
            else
            {
                // Oyuncu bir �ey ta��m�yor
                if (placedObject != null)
                {
                    if (!isBlending)
                    {
                        base.Interact(player);
                        Debug.Log("Picked up the blended item from the blender station.");
                    }
                    else
                    {
                        Debug.Log("Cannot pick up the item. Blending in progress.");
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
            IBlendable blendable = placedObject.GetComponent<IBlendable>();
            if (blendable != null)
            {
                if (isBlending)
                {
                    return true; // Blending devam ediyorsa bas�l� tutabilir
                }
                else if (blendable.IsBlendable && !blendable.IsBlended)
                {
                    // Item blend edilebilir durumda ve hen�z blend edilmemi�se blend ba�latabilir
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
                // Player animasyonu �rnek olarak FillBeer animasyonu yerine bir Blend animasyonu yapabilirsiniz.
                player.GetComponent<Animator>().Play("Blend", 0, normalizedTime);
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
                // Blender i�in item'in chop edilmi� (IsBlendable = true) ama hen�z blend edilmemi� olmas� gerekiyor.
                if (blendable != null && blendable.IsBlendable && !blendable.IsBlended && placedObject == null)
                {
                    return true;
                }
            }
            else
            {
                // Oyuncu bir �ey ta��m�yor, e�er placedObject varsa ve blending yap�lm�yorsa alabilir
                if (placedObject != null)
                {
                    return !isBlending;
                }
            }
        }
        return false;
    }
}
