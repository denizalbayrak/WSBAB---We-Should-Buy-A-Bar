using UnityEngine;

public class Carryable : MonoBehaviour
{
    public float dropYOffset = 0f; // Offset when dropping the object

    // Methods that can be overridden by derived classes
    public virtual void OnPickUp()
    {
        // Default behavior when picked up
    }

    public virtual void OnDrop()
    {
        // Default behavior when dropped
    }
}
