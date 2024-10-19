using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableComponent : MonoBehaviour, IInteractable
{
    public InteractableObject interactableObject;

    public void Interact(GameObject interactor)
    {
        if (interactableObject != null)
        {
            interactableObject.Interact(interactor);
        }
        else
        {
            Debug.LogWarning("InteractableObject referansý atanmamýþ.");
        }
    }
}
