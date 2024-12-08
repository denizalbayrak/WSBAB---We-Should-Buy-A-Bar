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
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            GameObject carriedObject = playerInteraction.CarriedObject;

            if (carriedObject != null)
            {
                // Oyuncu bir nesne ta��yor
                IChoppable choppable = carriedObject.GetComponent<IChoppable>();
                if (choppable != null && placedObject == null)
                {
                    // Kabin bo�, hem full hem de chopped item koyabiliriz
                    // Ama sadece full item chop yap�labilir. Chopped itemi koymak depolama ama�l� olur.
                    base.Interact(player);
                    Debug.Log($"Placed a {carriedObject.name} on the chop station. (Full: {choppable.IsFull}, Chopped: {choppable.IsChopped})");
                }
                else
                {
                    Debug.Log("You need to carry a choppable item (lime, orange, chocolate) to place it here.");
                }
                if (placedObject.GetComponent<Lime>() != null)
                {
                    if (placedObject.GetComponent<Lime>().IsChopped)
                    {
                        MojitoGlass mojitoGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                        if (mojitoGlass != null && !mojitoGlass.HasLime)
                        {
                            mojitoGlass.AddLime();
                            placedObject.gameObject.SetActive(false);
                            placedObject = null;
                            Debug.Log("Placed a Lime into the cabinet.");
                        }
                    }
                }
            }
            else
            {
                // Oyuncu bir �ey ta��m�yor
                if (placedObject != null)
                {
                    if (!isChopping)
                    {
                        base.Interact(player);
                        Debug.Log("Picked up the item from the chop station.");
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
                    // Chop devam ediyorsa bas�l� tutabilir
                    return true;
                }
                else if (choppable.IsFull)
                {
                    // E�er item full ise chop ba�lat�labilir
                    return true;
                }
                // E�er chopped item ise, chop yap�lamaz ama oyuncu bas�l� tutmas� gerekmiyor
                // Bu durumda chopped item ile OnHoldInteract'tan bir i�lem yapmay�z.
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
                    animationController.SetChopping(false);
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
                    animationController.TriggerChopping();
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
                // E�er item full de�ilse chop ba�lat�lmaz
                // Chopped itemi koyduk diyelim, OnHoldInteract �a�r�ld���nda full de�il diye chop yok.
                // Bu durumda chop i�lemi ba�lamaz.
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
            }
            else
            {
                // Oyuncu bir �ey ta��m�yor
                if (placedObject != null)
                {
                    knife.SetActive(true);
                    // E�er chopping yap�lm�yorsa itemi alabilir
                    return !isChopping;
                }
            }
        }
        return false;
    }
}
