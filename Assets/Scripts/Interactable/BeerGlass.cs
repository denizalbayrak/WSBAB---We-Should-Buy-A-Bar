using UnityEngine;

public class BeerGlass : Carryable, IWashableGlass
{
    public enum GlassState
    {
        DirtyEmpty,
        CleanEmpty,
        Filled
    }
    public GlassType Type => GlassType.Beer;
    public GlassState CurrentState = GlassState.DirtyEmpty;
    public GlassType glassType = GlassType.Beer;
    public GameObject cleanVisual;
    public GameObject dirtyVisual;
    public GameObject filledVisual; 

    private void Start()
    {
        UpdateVisuals();
    }
    public bool IsDirty
{
    get { return CurrentState == GlassState.DirtyEmpty; }
}
    public void Dirty()
    {
        CurrentState = GlassState.DirtyEmpty;
        isReady = false;
        UpdateVisuals();
        Debug.Log("The beer glass is now dirty.");
    }

    public void Clean()
    {
        CurrentState = GlassState.CleanEmpty;
        isReady = false;
        UpdateVisuals();
        Debug.Log("The beer glass is now clean.");
    }

    public void Fill()
    {
        if (CurrentState == GlassState.CleanEmpty)
        {
            CurrentState = GlassState.Filled;
            isReady = true;
            Debug.Log("Filled the beer glass with beer.");
        }
        else
        {
            Debug.Log("Cannot fill the beer glass in its current state.");
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
            dirtyVisual.SetActive(CurrentState == GlassState.DirtyEmpty);
        
 }

    public override void OnPickUp()
    {
        base.OnPickUp();
        Debug.Log("Picked up a beer glass.");
    }

    public override void OnDrop()
    {
        base.OnDrop();
        Debug.Log("Dropped the beer glass.");
    }
}
