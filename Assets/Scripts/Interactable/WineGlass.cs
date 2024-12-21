using UnityEngine;

public class WineGlass : Carryable, IWashableGlass
{
    public enum GlassState
    {
        DirtyEmpty,
        CleanEmpty,
        Filled
    }
    public GlassType Type => GlassType.Wine;
    public GlassState CurrentState = GlassState.CleanEmpty;
    public GlassType glassType = GlassType.Wine;
    public GameObject cleanVisual; // Temiz bardak görseli
    public GameObject dirtyVisual; // Kirli bardak görseli
    public GameObject filledVisual; // Kirli bardak görseli

    private void Start()
    {
        UpdateVisuals();
    }

    public void Dirty()
    {
        CurrentState = GlassState.DirtyEmpty;
        isReady = false;
        UpdateVisuals();
        Debug.Log("The wine glass is now dirty.");
    }

    public void Clean()
    {
        CurrentState = GlassState.CleanEmpty;
        isReady = false;
        UpdateVisuals();
        Debug.Log("The wine glass is now clean.");
    }

    public void Fill()
    {
        if (CurrentState == GlassState.CleanEmpty)
        {
            CurrentState = GlassState.Filled;
            isReady = true;
            //UpdateVisuals();
            Debug.Log("Filled the wine glass with wine.");
        }
        else
        {
            Debug.Log("Cannot fill the wine glass in its current state.");
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
    public bool IsDirty
    {
        get { return CurrentState == GlassState.DirtyEmpty; }
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
