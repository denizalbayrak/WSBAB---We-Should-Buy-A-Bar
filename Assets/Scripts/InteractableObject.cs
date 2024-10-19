using UnityEngine;

public abstract class InteractableObject : ScriptableObject
{
    public string objectName;
    public string description;

    public abstract void Interact(GameObject interactor);
}
