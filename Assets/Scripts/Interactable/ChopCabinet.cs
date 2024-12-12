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

    public Image fillProgressUI; // Assign this in the Inspector (clock image)
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
                    // Kabin bo�, objeyi yerle�tir
                    base.Interact(player);
                    Debug.Log($"Placed a {carriedObject.name} on the chop station. (Full: {choppable.IsFull}, Chopped: {choppable.IsChopped})");
                }
                else
                {
                    Debug.Log("You need to carry a choppable item (lime, orange, chocolate) to place it here or the placement point is already occupied.");
                }

                // E�er yerle�tirilmi� obje Lime ise ve do�ranm��sa, MojitoGlass'e ekle
                if (placedObject != null && placedObject.GetComponent<Lime>() != null)
                {
                    Lime lime = placedObject.GetComponent<Lime>();
                    if (lime.IsChopped)
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
                if (placedObject != null && placedObject.GetComponent<Chocolate>() != null)
                {
                    Chocolate chocolate = placedObject.GetComponent<Chocolate>();
                    if (chocolate.IsChopped)
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

            else
            {
                // Oyuncu bir �ey ta��m�yor
                if (placedObject != null)
                {
                    // Doldurma i�lemi ba�lamad�ysa, bardak al�nabilir
                    if (!isChopping)
                    {
                        base.Interact(player); // Bu, yerle�tirilmi� objeyi al�r
                        Debug.Log("Picked up the item from the chop station.");

                        // isCompleted bayra��n� s�f�rla
                        // Chopping ile ilgili flag'ler s�f�rlanmal�
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

    // Implement IHoldInteractable
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
                    if (choppableItem != null)
                    {
                        // Choppable item t�r�ne g�re do�ru metodlar� tetikle
                        if (choppableItem is Lime)
                        {
                            animationController.SetChopping(false);
                        }
                        else if (choppableItem is Chocolate)
                        {
                            animationController.SetChopping(false);
                        }
                        // Di�er choppable item'lar i�in eklemeler yap�labilir
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
                    // Chopping animasyonunu tetikleyin. �rne�in:
                    if (choppableItem is Lime)
                    {
                        animationController.TriggerChopping();
                    }
                    else if (choppableItem is Chocolate)
                    {
                        animationController.TriggerChopping();
                    }
                    // Di�er choppable item'lar i�in eklemeler yap�labilir
                }
                Debug.Log("Started chopping. Hold Ctrl for " + chopDuration + " seconds.");

                // Fill progress UI'� g�ster
                if (fillProgressUI != null)
                {
                    fillProgressUI.gameObject.SetActive(true);
                }
                isClockVisible = true;
            }
            else
            {
                // E�er item full de�ilse chop ba�lat�lmaz
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
                // E�er MojitoGlass ta��yor ise (belki ba�ka i�lemler i�in)
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
