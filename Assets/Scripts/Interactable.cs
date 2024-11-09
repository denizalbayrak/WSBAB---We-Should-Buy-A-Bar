using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Methods that can be overridden by derived classes
    public virtual void Interact(GameObject player)
    {
        // Default interaction behavior
    }

    public virtual bool CanInteract(GameObject player)
    {
        // Return true if interaction is possible
        return true;
    }
}
