using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MojitoGlass : Carryable, IWashableGlass, IInteractableItem
{
    public enum GlassState
    {
        DirtyEmpty,
        CleanEmpty,
        Filled
    }
    public GlassType Type => GlassType.Mojito;
    public GlassState CurrentState = GlassState.CleanEmpty;
    public GlassType glassType = GlassType.Mojito;
    public GameObject cleanVisual; 
    public GameObject dirtyVisual; 
    public GameObject filledVisual; 
    public GameObject ice;
    public GameObject lime;
    public GameObject juice;
    public bool HasIce = false;
    public bool HasLime = false;
    public bool HasJuice = false;

    private void Start()
    {
        UpdateVisuals();
    }

    public void Dirty()
    {
        CurrentState = GlassState.DirtyEmpty;
        isReady = false;
        HasIce = false;
     HasLime = false;
   HasJuice = false;
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
    public void AddIce()
    {
        HasIce = true;
        ice.SetActive(true);
        isDone();
    }
    public void AddLime()
    {
        HasLime = true;
        lime.SetActive(true);
        isDone();
    } 
    public void AddJuice()
    {
        HasJuice = true;
        juice.SetActive(true);
        isDone();
    }  
    public void isDone()
    {
        if (HasJuice && HasLime && HasIce)
        {
            isReady = true;
            CurrentState = GlassState.Filled;
            return;

        }
        isReady = false;
    }
    public void InteractWith(GameObject target, EmptyCabinet cabinet)
    {
        Ice ice = target.GetComponent<Ice>();
        if (ice != null)
        {
            if (!HasIce)
            {
                AddIce();
                Debug.Log("Ice added to the glass.");

                Destroy(target);

                PlaceGlassOnCabinet(cabinet);
            }
            else
            {
                Debug.Log("Glass already has ice.");
            }
            return;
        }

        Lime lime = target.GetComponent<Lime>();
        if (lime != null && lime.CurrentState == Lime.LimeState.ChoppedLime)
        {
            if (!HasLime)
            {
                AddLime();
                Debug.Log("Lime added to the glass.");

                Destroy(target);

                PlaceGlassOnCabinet(cabinet);
            }
            else
            {
                Debug.Log("Glass already has lime.");
            }
            return;
        }

        Debug.Log("MojitoGlass cannot interact with this object.");
    }

    private void PlaceGlassOnCabinet(EmptyCabinet cabinet)
    {
        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
        {
            playerInteraction.DropCarriedObject();

            if (cabinet != null)
            {
                cabinet.PlaceObject(gameObject);
                playerInteraction.CarriedObject = null;
                playerInteraction.isCarrying = false; 
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
            ice.SetActive(false);
            lime.SetActive(false);
            juice.SetActive(false);
        }

    }
    public bool IsDirty
    {
        get { return CurrentState == GlassState.DirtyEmpty; }
    }
    public override void OnPickUp()
    {
        base.OnPickUp();
        Debug.Log("Picked up a mojito glass.");
    }

    public override void OnDrop()
    {
        base.OnDrop();
        Debug.Log("Dropped the mojito glass.");
    }
}
