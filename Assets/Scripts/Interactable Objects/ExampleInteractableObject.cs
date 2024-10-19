using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Interactable Objects/Example")]
public class ExampleInteractableObject : InteractableObject
{
    public override void Interact(GameObject interactor)
    {
        Debug.Log($"{objectName} ile {interactor.name} etkileþime girdi.");
    }
}
