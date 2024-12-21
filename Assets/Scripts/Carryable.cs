using UnityEngine;
public interface IWashableGlass
{
    GlassType Type { get; }
    bool IsDirty { get; }
    void Clean();
}

public interface IChoppable
{
    bool IsFull { get; }     
    bool IsChopped { get; }  

    void Chop();             
    void UpdateVisuals(); 
}
public interface IBlendable
{
    bool IsBlendable { get; } 
    bool IsBlended { get; }   

    void Blend();            
    void UpdateVisuals();   
}
public interface IInteractableItem
{
    void InteractWith(GameObject target, EmptyCabinet cabinet);
}
public class Carryable : MonoBehaviour
{
    public Transform objectTransform;
    private Collider objectCollider;
    public bool isReady = false;
    private void Awake()
    {
        objectCollider = GetComponent<Collider>();
        objectTransform = GetComponent<Transform>();
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
