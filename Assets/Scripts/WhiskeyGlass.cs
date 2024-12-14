using UnityEngine;

public class WhiskeyGlass : Carryable, IWashableGlass, IInteractableItem
{
    public enum GlassState
    {
        DirtyEmpty,
        CleanEmpty,
        Filled
    }

    public GlassState CurrentState = GlassState.CleanEmpty;
    public GlassType glassType = GlassType.Whiskey;
    public GameObject cleanVisual;
    public GameObject dirtyVisual;
    public GameObject chocolate;
    public GameObject whiskey;
    public bool HasChocolate = false;
    public bool HasWhiskey = false;

    private void Start()
    {
        UpdateVisuals();
    }

    public void Dirty()
    {
        CurrentState = GlassState.DirtyEmpty;
        isReady = false;
        HasChocolate = false;
        HasWhiskey = false;
        UpdateVisuals();
        Debug.Log("The whiskey glass is now dirty.");
    }

    public void Clean()
    {
        CurrentState = GlassState.CleanEmpty;
        isReady = false;
        UpdateVisuals();
        Debug.Log("The whiskey glass is now clean.");
    }
    public void AddChocolate()
    {
        HasChocolate = true;
        chocolate.SetActive(true);
        isDone();
    }
    public void AddWhiskey()
    {
        HasWhiskey = true;
        whiskey.SetActive(true);
        isDone();
    }
   
    public void isDone()
    {
        if (HasWhiskey && HasChocolate)
        {
            isReady = true;
            CurrentState = GlassState.Filled;
            return;

        }
        isReady = false;
    }
    public void InteractWith(GameObject target, EmptyCabinet cabinet)
    {
        Chocolate chocolate = target.GetComponent<Chocolate>();
        if (chocolate != null && chocolate.CurrentState == Chocolate.ChocolateState.ChoppedChocolate)
        {
            if (!HasChocolate)
            {
                AddChocolate();
                Debug.Log("Chocolate added to the glass.");

                // Kabindeki Chocolate'ý yok et
                Destroy(target);

                // Oyuncunun elindeki bardaðý býrak ve kabine yerleþtir
                PlaceGlassOnCabinet(cabinet);
            }
            else
            {
                Debug.Log("Glass already has Chocolate.");
            }
            return;
        }

        Debug.Log("WhiskeyGlass cannot interact with this object.");
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
            chocolate.SetActive(false);
            whiskey.SetActive(false);
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
