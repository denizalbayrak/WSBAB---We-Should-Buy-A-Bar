using UnityEngine;

public class Carryable : MonoBehaviour
{
    private Collider objectCollider;

    private void Awake()
    {
        objectCollider = GetComponent<Collider>();
    }

    public virtual void OnPickUp()
    {
        // Disable the collider or set it to trigger
        if (objectCollider != null)
        {
             objectCollider.isTrigger = true;
        }

        // Additional logic for when the object is picked up
        Debug.Log(gameObject.name + " picked up.");
    }

    public virtual void OnDrop()
    {
        // Re-enable the collider or unset the trigger
        if (objectCollider != null)
        {
            objectCollider.isTrigger = false;
        }

        // Additional logic for when the object is dropped
        Debug.Log(gameObject.name + " dropped.");
    }
}
