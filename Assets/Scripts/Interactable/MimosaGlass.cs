using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimosaGlass : Carryable, IWashableGlass, IInteractableItem
{
    public enum GlassState
    {
        DirtyEmpty,
        CleanEmpty,
        Filled
    }

    public GlassState CurrentState = GlassState.CleanEmpty;
    public GlassType glassType = GlassType.Mimosa;
    public GameObject cleanVisual; // Temiz bardak görseli
    public GameObject dirtyVisual; // Kirli bardak görseli
    public GameObject filledVisual; // Kirli bardak görseli
    public GameObject orangeJuice;
    public GameObject champagne;
    public bool HasOrangeJuice = false;
    public bool HasChampagne = false;

    private void Start()
    {
        UpdateVisuals();
    }

    public void Dirty()
    {
        CurrentState = GlassState.DirtyEmpty;
        isReady = false;
        UpdateVisuals();
        Debug.Log("The mojito glass is now dirty.");
    }

    public void Clean()
    {
        CurrentState = GlassState.CleanEmpty;
        isReady = false;
        UpdateVisuals();
        Debug.Log("The mojito glass is now clean.");
    }
    public void AddOrangeJuice()
    {
        HasOrangeJuice = true;
        orangeJuice.SetActive(true);
        isDone();
    }
   
    public void AddJuice()
    {
        HasChampagne = true;
        champagne.SetActive(true);
        isDone();
    }
    public void isDone()
    {
        if (HasChampagne && HasOrangeJuice)
        {
            isReady = true;
            CurrentState = GlassState.Filled;
            return;

        }
        isReady = false;
    }
    public void InteractWith(GameObject target, EmptyCabinet cabinet)
    {
        // Hedef nesne Orange ise
        Orange orange = target.GetComponent<Orange>();
        if (orange != null && orange.CurrentState == Orange.OrangeState.ChoppedOrange)
        {
            if (!HasOrangeJuice)
            {
                AddOrangeJuice();
                Debug.Log("Orange added to the glass.");

                // Kabindeki orange'ý yok et
                Destroy(target);

                // Oyuncunun elindeki bardaðý býrak ve kabine yerleþtir
                PlaceGlassOnCabinet(cabinet);
            }
            else
            {
                Debug.Log("Glass already has orange.");
            }
            return;
        }

        Debug.Log("MimosaGlass cannot interact with this object.");
    }

    private void PlaceGlassOnCabinet(EmptyCabinet cabinet)
    {
        // Oyuncunun elindeki bardaðý býrak ve kabine yerleþtir
        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
        {
            // Oyuncunun elindeki bardaðý býrak
            playerInteraction.DropCarriedObject();

            // Bardak kabine yerleþtirilir
            if (cabinet != null)
            {
                cabinet.PlaceObject(gameObject);
                playerInteraction.CarriedObject = null;
                playerInteraction.isCarrying = false; // Merkezi yönetim için
                playerInteraction.animator.SetBool("isCarry", false);
                Debug.Log("Placed glass on cabinet.");
            }
            else
            {
                Debug.LogError("EmptyCabinet is null.");
            }
        }
        else
        {
            Debug.LogError("PlayerInteraction not found.");
        }
    }

    public void Fill()
    {
        if (CurrentState == GlassState.CleanEmpty)
        {
            CurrentState = GlassState.Filled;
            isReady = true;
            //UpdateVisuals();
            Debug.Log("Filled the mojito glass with wine.");
        }
        else
        {
            Debug.Log("Cannot fill the mojito glass in its current state.");
        }
    }

    private void UpdateVisuals()
    {
        if (cleanVisual != null)
        {
            cleanVisual.SetActive(CurrentState == GlassState.CleanEmpty);
            foreach (Transform child in cleanVisual.transform)
            {
                child.gameObject.SetActive(false);
            }
        }


        if (dirtyVisual != null)
        {
            dirtyVisual.SetActive(CurrentState == GlassState.DirtyEmpty);
            orangeJuice.SetActive(false);
            champagne.SetActive(false);
        }

    }
    public bool IsDirty
    {
        get { return CurrentState == GlassState.DirtyEmpty; }
    }
    public override void OnPickUp()
    {
        base.OnPickUp();
        Debug.Log("Picked up a mimosa glass.");
    }

    public override void OnDrop()
    {
        base.OnDrop();
        Debug.Log("Dropped the mimosa glass.");
    }
}