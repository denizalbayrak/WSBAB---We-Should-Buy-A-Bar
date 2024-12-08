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

    // Blender i�indeki g�rseller
    public GameObject emptyBlender;
    public GameObject orangeBlender;

    public override void Interact(GameObject player)
    {
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction == null) return;

        GameObject carriedObject = playerInteraction.CarriedObject;

        if (carriedObject != null)
        {
            // Oyuncu bir nesne ta��yor
            IBlendable blendable = carriedObject.GetComponent<IBlendable>();

            if (blendable != null && blendable.IsBlendable && !blendable.IsBlended && placedObject == null)
            {
                // Chop edilmi� ve blend edilmemi� itemi blendera koyma
                base.Interact(player);
                Debug.Log($"Placed a {carriedObject.name} in the blender.");

                // Orange ise chopped haldeyse orangeBlender aktif
                Orange orange = placedObject.GetComponent<Orange>();
                if (orange != null && orange.IsChopped)
                {
                    placedObject.transform.GetChild(0).gameObject.SetActive(false);
                    emptyBlender.SetActive(false);
                    orangeBlender.SetActive(true);
                }
                else
                {
                    // �leride farkl� blendable itemler i�in benzer mant�k
                    emptyBlender.SetActive(false);
                    orangeBlender.SetActive(true);
                }

            }
            else
            {
                Orange blendedObj = placedObject.GetComponent<Orange>();
                if (blendedObj != null && blendedObj.IsBlended)
                {
                    Debug.Log("giriyorrrrrrrrrr");
                    // Blend tamamland�, MimosaGlass gerekli
                    MimosaGlass mimosaGlass = playerInteraction.CarriedObject != null ? playerInteraction.CarriedObject.GetComponent<MimosaGlass>() : null;
                    if (mimosaGlass != null)
                    {
                        // MimosaGlass var, blend edilmi� juice'� ekle
                        mimosaGlass.AddOrangeJuice();
                        Debug.Log("Transferred blended orange juice into MimosaGlass.");

                        // Blender'� bo�alt
                        Destroy(placedObject);
                        placedObject = null;

                        // G�rselleri resetle
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
            // Oyuncu elinde bir �ey ta��m�yor
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
                    // Blend tamamland�, MimosaGlass gerekli
                    MimosaGlass mimosaGlass = playerInteraction.CarriedObject != null ? playerInteraction.CarriedObject.GetComponent<MimosaGlass>() : null;
                    if (mimosaGlass != null)
                    {
                        // MimosaGlass var, blend edilmi� juice'� ekle
                        mimosaGlass.AddOrangeJuice();
                        Debug.Log("Transferred blended orange juice into MimosaGlass.");

                        // Blender'� bo�alt
                        Destroy(placedObject);
                        placedObject = null;

                        // G�rselleri resetle
                        emptyBlender.SetActive(true);
                        orangeBlender.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("You need to have a MimosaGlass to pick up the blended juice.");
                    }
                }
                //else
                //{
                //    // Blend edilmemi� veya IsBlended olmayan bir itemi geri almak isterseniz
                //    base.Interact(player);
                //    Debug.Log("Picked up the unblended item from the blender station.");

                //    // Blender'� bo�alt
                //    Destroy(placedObject);
                //    placedObject = null;

                //    // G�rselleri resetle
                //    emptyBlender.SetActive(true);
                //    orangeBlender.SetActive(false);
                //}
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
                    return true; // Blending devam ediyorsa bas�l� tutabilir
                }
                else if (blendable.IsBlendable && !blendable.IsBlended)
                {
                    // Chop edilmi� ama hen�z blend edilmemi�se blend ba�lat�labilir
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

                // Blend tamamland�ktan sonra orangeBlender g�rselini kapat�p emptyBlender a�
                //if (orangeBlender != null)
                //{
                //    orangeBlender.SetActive(false);
                //}
                //if (emptyBlender != null)
                //{
                //    emptyBlender.SetActive(true);
                //}
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
        Debug.Log("000000000");
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            Debug.Log("11111111");
            GameObject carriedObject = playerInteraction.CarriedObject;
            if (carriedObject != null)
            {
                Debug.Log("222222222");
                IBlendable blendable = carriedObject.GetComponent<IBlendable>();
                Debug.Log(" carriedObject " + carriedObject);

                // Blender i�in item chop edilmi� (IsBlendable = true) ve hen�z blend edilmemi� olmal�
                if (blendable != null && blendable.IsBlendable && !blendable.IsBlended && placedObject == null)
                {
                    Debug.Log("3333333");
                    return true;
                }

                MimosaGlass mimosaGlass = carriedObject.GetComponent<MimosaGlass>();
                if (mimosaGlass != null && placedObject != null)
                {
                    IBlendable blendableInBlender = placedObject.GetComponent<IBlendable>();
                    if (blendableInBlender != null && blendableInBlender.IsBlended)
                    {
                        Debug.Log("9999999");
                        return true;
                    }
                }
            }
            else
            {
                Debug.Log("444444");
                // Oyuncu bir �ey ta��m�yor
                if (placedObject != null)
                {
                    Debug.Log("5555555");
                    IBlendable blendable = placedObject.GetComponent<IBlendable>();
                    if (blendable != null && blendable.IsBlended)
                    {
                        Debug.Log("6666");
                        // Blend tamamlanm�� itemi almak i�in MimosaGlass gerekli
                        // Oyuncunun elinde hi�bir �ey yok -> MimosaGlass yok -> false
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
