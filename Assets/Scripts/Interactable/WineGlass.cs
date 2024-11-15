using UnityEngine;

public class WineGlass : Carryable
{
    public enum GlassState
    {
        DirtyEmpty,
        CleanEmpty,
        Filled
    }

    public GlassState CurrentState = GlassState.DirtyEmpty;

    public GameObject cleanVisual;
    public GameObject dirtyVisual; 
    public GameObject filledVisual;

    private void Start()
    {
        UpdateVisuals();
    }

    public void Dirty()
    {
        CurrentState = GlassState.DirtyEmpty;
        UpdateVisuals();
        Debug.Log("The wine glass is now dirty.");
    }

    public void Clean()
    {
        CurrentState = GlassState.CleanEmpty;
        UpdateVisuals();
        Debug.Log("The wine glass is now clean.");
    }

    public void Fill()
    {
        if (CurrentState == GlassState.CleanEmpty)
        {
            CurrentState = GlassState.Filled;
            //UpdateVisuals();
            Debug.Log("Filled the wine glass with beer.");
        }
        else
        {
            Debug.Log("Cannot fill the wine glass in its current state.");
        }
    }

    private void UpdateVisuals()
    {
        if (cleanVisual != null)
            cleanVisual.SetActive(CurrentState == GlassState.CleanEmpty);

        if (dirtyVisual != null)
            dirtyVisual.SetActive(CurrentState == GlassState.DirtyEmpty);

        //if (dirtyVisual != null)
        //    cleanVisual.SetActive(CurrentState == GlassState.Filled);
    }

    public override void OnPickUp()
    {
        base.OnPickUp();
        Debug.Log("Picked up a wine glass.");
    }

    public override void OnDrop()
    {
        base.OnDrop();
        Debug.Log("Dropped the wine glass.");
    }
}
