using UnityEngine;

public class BeerGlass : Carryable
{
    public enum GlassState
    {
        DirtyEmpty,
        CleanEmpty,
        Filled
    }
   [SerializeField] private Material dirtyMat;
   [SerializeField] private Material cleanMat;
    public GlassState CurrentState = GlassState.DirtyEmpty;

    public void Dirty()
    {
        CurrentState = GlassState.DirtyEmpty;
        GetComponent<Renderer>().material = dirtyMat;
    }
    public void Clean()
    {
        if (CurrentState == GlassState.DirtyEmpty)
        {
            CurrentState = GlassState.CleanEmpty;
            // Update the beer glass's appearance to look clean
            // For example, change material or texture
            // Optionally, play a sound or particle effect
            GetComponent<Renderer>().material = cleanMat;
            Debug.Log("The beer glass is now clean.");
        }
        else
        {
            Debug.Log("The beer glass does not need cleaning.");
        }
    }

    public void Fill()
    {
        if (CurrentState == GlassState.CleanEmpty)
        {
            CurrentState = GlassState.Filled;
            // Update the beer glass's appearance to show it's filled
            // For example, add liquid mesh or particle effect
            Debug.Log("Filled the beer glass with beer.");
        }
        else
        {
            Debug.Log("Cannot fill the beer glass in its current state.");
        }
    }

    public override void OnPickUp()
    {
        // Specific behavior when the beer glass is picked up
        base.OnPickUp();
        Debug.Log("Picked up a beer glass.");
    }

    public override void OnDrop()
    {
        base.OnDrop();
        Debug.Log("Dropped the beer glass.");
    }
}
