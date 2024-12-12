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
                // Oyuncu bir nesne taþýyor
                IChoppable choppable = carriedObject.GetComponent<IChoppable>();
                if (choppable != null && placedObject == null)
                {
                    // Kabin boþ, objeyi yerleþtir
                    base.Interact(player);
                    Debug.Log($"Placed a {carriedObject.name} on the chop station. (Full: {choppable.IsFull}, Chopped: {choppable.IsChopped})");
                }
                else
                {
                    Debug.Log("You need to carry a choppable item (lime, orange, chocolate) to place it here or the placement point is already occupied.");
                }

                // Eðer yerleþtirilmiþ obje Lime ise ve doðranmýþsa, MojitoGlass'e ekle
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
                // Oyuncu bir þey taþýmýyor
                if (placedObject != null)
                {
                    // Doldurma iþlemi baþlamadýysa, bardak alýnabilir
                    if (!isChopping)
                    {
                        base.Interact(player); // Bu, yerleþtirilmiþ objeyi alýr
                        Debug.Log("Picked up the item from the chop station.");

                        // isCompleted bayraðýný sýfýrla
                        // Chopping ile ilgili flag'ler sýfýrlanmalý
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
                    // Chop devam ediyorsa basýlý tutabilir
                    return true;
                }
                else if (choppable.IsFull)
                {
                    // Eðer item full ise chop baþlatýlabilir
                    return true;
                }
                // Eðer chopped item ise, chop yapýlamaz ama oyuncu basýlý tutmasý gerekmiyor
                // Bu durumda chopped item ile OnHoldInteract'tan bir iþlem yapmayýz.
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
                        // Choppable item türüne göre doðru metodlarý tetikle
                        if (choppableItem is Lime)
                        {
                            animationController.SetChopping(false);
                        }
                        else if (choppableItem is Chocolate)
                        {
                            animationController.SetChopping(false);
                        }
                        // Diðer choppable item'lar için eklemeler yapýlabilir
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
                    // Chopping animasyonunu tetikleyin. Örneðin:
                    if (choppableItem is Lime)
                    {
                        animationController.TriggerChopping();
                    }
                    else if (choppableItem is Chocolate)
                    {
                        animationController.TriggerChopping();
                    }
                    // Diðer choppable item'lar için eklemeler yapýlabilir
                }
                Debug.Log("Started chopping. Hold Ctrl for " + chopDuration + " seconds.");

                // Fill progress UI'ý göster
                if (fillProgressUI != null)
                {
                    fillProgressUI.gameObject.SetActive(true);
                }
                isClockVisible = true;
            }
            else
            {
                // Eðer item full deðilse chop baþlatýlmaz
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
                // Eðer MojitoGlass taþýyor ise (belki baþka iþlemler için)
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
                // Oyuncu bir þey taþýmýyor
                if (placedObject != null)
                {
                    knife.SetActive(true);
                    // Eðer chopping yapýlmýyorsa itemi alabilir
                    return !isChopping;
                }
            }
        }
        return false;
    }
}
