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
    private WhiskeyGlass whiskeyBeingFilled; // Yeni eklenen

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
                // Oyuncu bir nesne ta��yor
                MojitoGlass mojitoGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                MimosaGlass mimosaGlass = playerInteraction.CarriedObject.GetComponent<MimosaGlass>();
                WhiskeyGlass whiskeyGlass = playerInteraction.CarriedObject.GetComponent<WhiskeyGlass>(); // Yeni eklenen

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
                else if (whiskeyGlass != null) // Yeni eklenen
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
                else
                {
                    Debug.Log("You need to carry a Mojito, Mimosa, or Whiskey glass to place it here.");
                }
            }
            else if (playerInteraction.CarriedObject == null)
            {
                // Oyuncu bir �ey ta��m�yor
                if (placedObject != null)
                {
                    // Doldurma i�lemi ba�lamad�ysa, bardak al�nabilir
                    if (!isFilling)
                    {
                        base.Interact(player); // Bu, yerle�tirilmi� objeyi al�r
                        Debug.Log("Picked up the glass from the alcohol station.");

                        // isCompleted bayra��n� s�f�rla
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

    // Implement IHoldInteractable
    public bool CanHoldInteract(GameObject player)
    {
        // Bardak olup olmad���n� kontrol et
        if (placedObject != null)
        {
            // Bardak t�r�n� kontrol et
            MojitoGlass mojitoGlass = placedObject.GetComponent<MojitoGlass>();
            MimosaGlass mimosaGlass = placedObject.GetComponent<MimosaGlass>();
            WhiskeyGlass whiskeyGlass = placedObject.GetComponent<WhiskeyGlass>(); // Yeni eklenen

            if (isFilling)
            {
                // Doldurma devam ediyorsa bas�l� tutabilir
                return true;
            }
            else if (mojitoGlass != null || mimosaGlass != null || whiskeyGlass != null) // Yeni eklenen
            {
                // Doldurma i�lemini ba�latmak i�in bas�l� tutulabilir
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
            // Doldurma i�lemi devam ediyor
            fillProgress += deltaTime;
            if (fillProgress > fillDuration)
            {
                fillProgress = fillDuration;
            }
            // Doldurma ilerlemesini UI'da g�ncelle
            UpdateFillProgressUI();

            // Animator'�n oynatma zaman�n� g�ncelle
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
                    alcoholPointAnimator.Play("Fill", 0, 1f); // Animasyonun tamamland���ndan emin olun
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

                // Blender g�rsellerini s�f�rla (Bu k�s�mlar� kendi projenize g�re d�zenleyin)
                // emptyBlender.SetActive(true);
                // orangeBlender.SetActive(false);
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
                    else if (whiskeyBeingFilled != null) // Yeni eklenen
                    {
                        animationController.SetFillingBeer(true);
                        animationController.TriggerFillingBeer();
                    }
                }
                Debug.Log("Started filling the glass. Hold Ctrl for " + fillDuration + " seconds.");

                // Fill progress UI'� g�ster
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
            // UI Image'in dolum miktar�n� g�ncelle
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
                // Player bir �ey ta��yor
                MojitoGlass mojitoGlass = playerInteraction.CarriedObject.GetComponent<MojitoGlass>();
                MimosaGlass mimosaGlass = playerInteraction.CarriedObject.GetComponent<MimosaGlass>();
                WhiskeyGlass whiskeyGlass = playerInteraction.CarriedObject.GetComponent<WhiskeyGlass>(); 
                bool canPlace = (mojitoGlass != null || mimosaGlass != null || whiskeyGlass != null) && placedObject == null; 
                Debug.Log($"CanInteract (Carrying): {canPlace}");
                return canPlace;
            }
            else
            {
                // Player bir �ey ta��m�yor
                if (placedObject != null)
                {
                    // Doldurma i�lemi ba�lamam��sa, bardak al�nabilir
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
