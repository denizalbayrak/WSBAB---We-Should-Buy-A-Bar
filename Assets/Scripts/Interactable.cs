using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact(GameObject player)
    {
       
    }

    public virtual bool CanInteract(GameObject player)
    {
        
        return true;
    }
}
